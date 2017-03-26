using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server.Network
{
	public interface IPacketEncoder
	{
		void EncodeOutgoingPacket( UOSocket to, ref byte[] buffer, ref int length );
		void DecodeIncomingPacket( UOSocket from, ref byte[] buffer, ref int length );
	}

	public delegate void NetStateCreatedCallback( UOSocket ns );

	public class UOSocket
	{
		private Socket m_Socket;
		private NetServer m_NetServer;
		private IPAddress m_Address;
		private ByteQueue m_Buffer;
		private byte[] m_RecvBuffer;
		private SendQueue m_SendQueue;
		private bool m_Running, m_Disposing, m_DisposeFinished, m_Sending;
		private IPacketEncoder m_Encoder;
		private DateTime m_NextCheckActivity;
		private string m_ToString;

		#region Traffic
		private long m_Incoming = 0;
		private long m_Outgoing = 0;

		public long Incoming
		{
			get { return m_Incoming; }
			set { m_Incoming = value; }
		}

		public long Outgoing
		{
			get { return m_Outgoing; }
			set { m_Outgoing = value; }
		}
		#endregion

		public Socket Socket
		{
			get { return m_Socket; }
		}

		public ByteQueue Buffer
		{
			get { return m_Buffer; }
		}

		public IPacketEncoder PacketEncoder
		{
			get { return m_Encoder; }
			set { m_Encoder = value; }
		}

		public IPAddress Address
		{
			get { return m_Address; }
		}

		public bool Running
		{
			get { return m_Running; }
		}

		private static NetStateCreatedCallback m_CreatedCallback;

		public static NetStateCreatedCallback CreatedCallback
		{
			get { return m_CreatedCallback; }
			set { m_CreatedCallback = value; }
		}

		public void SetName( string name )
		{
			m_ToString = name;
		}

		public override string ToString()
		{
			return m_ToString;
		}

		private static BufferPool m_ReceiveBufferPool = new BufferPool( "Receive", 2048, 2048 );

		public UOSocket( Socket socket, NetServer netServer )
		{
			m_Socket = socket;
			m_NetServer = netServer;
			m_Buffer = new ByteQueue();
			m_Running = false;
			m_RecvBuffer = m_ReceiveBufferPool.AcquireBuffer();
			m_SendQueue = new SendQueue();

			m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 0.5 );

			try
			{
				m_Address = ( (IPEndPoint) m_Socket.RemoteEndPoint ).Address;
				m_ToString = m_Address.ToString();
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				m_Address = IPAddress.None;
				m_ToString = "(error)";
			}

			if ( m_CreatedCallback != null )
				m_CreatedCallback( this );
		}

		public void Start()
		{
			if ( m_Socket == null )
				return;

			m_Running = true;

			m_NetServer.OnStarted( this );

			try
			{
				m_Socket.BeginReceive( m_RecvBuffer, 0, 32, SocketFlags.None, OnReceive, null );
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				Dispose( false );
			}
		}

		public void Continue()
		{
			if ( m_Socket == null )
				return;

			try
			{
				m_Socket.BeginReceive( m_RecvBuffer, 0, 2048, SocketFlags.None, OnReceive, null );
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				Dispose( false );
			}
		}

		private void OnReceive( IAsyncResult asyncResult )
		{
			lock ( this )
			{
				if ( m_Socket == null )
					return;

				try
				{
					int byteCount = m_Socket.EndReceive( asyncResult );

					if ( byteCount > 0 )
					{
						m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 1.2 );

						byte[] buffer = m_RecvBuffer;

						if ( m_Encoder != null )
							m_Encoder.DecodeIncomingPacket( this, ref buffer, ref byteCount );

						m_Buffer.Enqueue( buffer, 0, byteCount );

						m_Incoming += byteCount;

						m_NetServer.OnReceive( this, byteCount );
					}
					else
					{
						Dispose( false );
					}
				}
				catch ( Exception ex )
				{
					TraceException( ex );
					Dispose( false );
				}
			}
		}

		public void Send( byte[] buffer, int length )
		{
			if ( m_Socket == null )
				return;

			if ( m_Encoder != null )
				m_Encoder.EncodeOutgoingPacket( this, ref buffer, ref length );

			bool shouldBegin = false;

			lock ( m_SendQueue )
				shouldBegin = ( m_SendQueue.Enqueue( buffer, length ) );

			if ( shouldBegin )
			{
				int sendLength = 0;
				byte[] sendBuffer = m_SendQueue.Peek( ref sendLength );

				try
				{
					m_Socket.BeginSend( sendBuffer, 0, sendLength, SocketFlags.None, OnSend, null );
					m_Sending = true;
				}
				catch ( Exception ex )
				{
					TraceException( ex );
					Dispose( false );
				}
			}
		}

		public bool Flush()
		{
			if ( m_Socket == null || !m_SendQueue.IsFlushReady )
				return false;

			int length = 0;
			byte[] buffer;

			lock ( m_SendQueue )
				buffer = m_SendQueue.CheckFlushReady( ref length );

			if ( buffer != null )
			{
				try
				{
					m_Socket.BeginSend( buffer, 0, length, SocketFlags.None, OnSend, null );
					m_Sending = true;
					return true;
				}
				catch ( Exception ex )
				{
					TraceException( ex );
					Dispose( false );
				}
			}

			return false;
		}

		private static int m_CoalesceSleep = -1;

		public static int CoalesceSleep
		{
			get { return m_CoalesceSleep; }
			set { m_CoalesceSleep = value; }
		}

		private void OnSend( IAsyncResult asyncResult )
		{
			m_Sending = false;

			if ( m_Socket == null )
				return;

			try
			{
				int bytes = m_Socket.EndSend( asyncResult );

				if ( bytes <= 0 )
				{
					Dispose( false );
					return;
				}

				m_Outgoing += bytes;
				m_NetServer.OnSend( this, bytes );

				if ( m_Disposing && !m_DisposeFinished )
				{
					FinishDispose();
					return;
				}

				m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 1.2 );

				if ( m_CoalesceSleep >= 0 )
					Thread.Sleep( m_CoalesceSleep );

				int length = 0;
				byte[] queued;

				lock ( m_SendQueue )
					queued = m_SendQueue.Dequeue( ref length );

				if ( queued != null )
				{
					m_Socket.BeginSend( queued, 0, length, SocketFlags.None, OnSend, null );
					m_Sending = true;
				}
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				Dispose( false );
			}
		}

		public bool CheckAlive()
		{
			if ( m_Disposing && !m_DisposeFinished )
			{
				FinishDispose();
				return false;
			}

			if ( m_Socket == null )
				return false;

			if ( DateTime.UtcNow < m_NextCheckActivity )
				return true;

			Console.WriteLine( "Client: {0}: Disconnecting due to inactivity...", this );

			Dispose();
			return false;
		}

		public void Dispose()
		{
			Dispose( true );
		}

		public void Dispose( bool flush )
		{
			if ( m_Disposing && !m_DisposeFinished )
			{
				// The second call forces disposal.
				FinishDispose();
				return;
			}

			if ( m_Socket == null || m_Disposing )
				return;

			m_Disposing = true;

			if ( flush )
				flush = Flush();

			// If we're currently sending the last packet, schedule the "real" dispose for later.
			if ( !m_Sending )
				FinishDispose();
		}

		public void FinishDispose()
		{
			if ( m_DisposeFinished )
				return;

			m_DisposeFinished = true;

			try
			{
				m_Socket.Shutdown( SocketShutdown.Both );
			}
			catch ( SocketException ex )
			{
				TraceException( ex );
			}

			try
			{
				m_Socket.Close();
			}
			catch ( SocketException ex )
			{
				TraceException( ex );
			}

			if ( m_RecvBuffer != null )
				m_ReceiveBufferPool.ReleaseBuffer( m_RecvBuffer );

			m_Socket = null;

			m_Buffer = null;
			m_RecvBuffer = null;
			m_Running = false;

			m_NetServer.OnDisposed( this );

			if ( !m_SendQueue.IsEmpty )
			{
				lock ( m_SendQueue )
					m_SendQueue.Clear();
			}
		}

		public static void TraceException( Exception ex )
		{
			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Core.Config.LogDirectory, "network-errors.log" ), true ) )
				{
					op.WriteLine( "# {0}", DateTime.UtcNow );

					op.WriteLine( ex );

					op.WriteLine();
					op.WriteLine();
				}
			}
			catch { }

			try
			{
				Console.WriteLine( ex );
			}
			catch { }
		}
	}
}
