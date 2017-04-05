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
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private readonly NetServer m_NetServer;
		private byte[] m_RecvBuffer;
		private readonly SendQueue m_SendQueue;
		private bool m_Disposing, m_DisposeFinished, m_Sending;
		private DateTime m_NextCheckActivity;
		private string m_ToString;

		#region Traffic
		public long Incoming { get; set; } = 0;

		public long Outgoing { get; set; } = 0;
		#endregion

		public Socket Socket { get; private set; }

		public ByteQueue Buffer { get; private set; }

		public IPacketEncoder PacketEncoder { get; set; }

		public IPAddress Address { get; }

		public bool Running { get; private set; }

		public static NetStateCreatedCallback CreatedCallback { get; set; }

		public void SetName( string name )
		{
			m_ToString = name;
		}

		public override string ToString()
		{
			return m_ToString;
		}

		private static readonly BufferPool m_ReceiveBufferPool = new BufferPool( "Receive", 2048, 2048 );

		public UOSocket( Socket socket, NetServer netServer )
		{
			Socket = socket;
			m_NetServer = netServer;
			Buffer = new ByteQueue();
			Running = false;
			m_RecvBuffer = m_ReceiveBufferPool.AcquireBuffer();
			m_SendQueue = new SendQueue();

			m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 0.5 );

			try
			{
				Address = ( (IPEndPoint) Socket.RemoteEndPoint ).Address;
				m_ToString = Address.ToString();
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				Address = IPAddress.None;
				m_ToString = "(error)";
			}

			if ( CreatedCallback != null )
				CreatedCallback( this );
		}

		public void Start()
		{
			if ( Socket == null )
				return;

			Running = true;

			m_NetServer.OnStarted( this );

			try
			{
				Socket.BeginReceive( m_RecvBuffer, 0, 32, SocketFlags.None, OnReceive, null );
			}
			catch ( Exception ex )
			{
				TraceException( ex );
				Dispose( false );
			}
		}

		public void Continue()
		{
			if ( Socket == null )
				return;

			try
			{
				Socket.BeginReceive( m_RecvBuffer, 0, 2048, SocketFlags.None, OnReceive, null );
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
				if ( Socket == null )
					return;

				try
				{
					var byteCount = Socket.EndReceive( asyncResult );

					if ( byteCount > 0 )
					{
						m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 1.2 );

						var buffer = m_RecvBuffer;

						if ( PacketEncoder != null )
							PacketEncoder.DecodeIncomingPacket( this, ref buffer, ref byteCount );

						Buffer.Enqueue( buffer, 0, byteCount );

						Incoming += byteCount;

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
			if ( Socket == null )
				return;

			if ( PacketEncoder != null )
				PacketEncoder.EncodeOutgoingPacket( this, ref buffer, ref length );

			var shouldBegin = false;

			lock ( m_SendQueue )
				shouldBegin = ( m_SendQueue.Enqueue( buffer, length ) );

			if ( shouldBegin )
			{
				var sendLength = 0;
				var sendBuffer = m_SendQueue.Peek( ref sendLength );

				try
				{
					Socket.BeginSend( sendBuffer, 0, sendLength, SocketFlags.None, OnSend, null );
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
			if ( Socket == null || !m_SendQueue.IsFlushReady )
				return false;

			var length = 0;
			byte[] buffer;

			lock ( m_SendQueue )
				buffer = m_SendQueue.CheckFlushReady( ref length );

			if ( buffer != null )
			{
				try
				{
					Socket.BeginSend( buffer, 0, length, SocketFlags.None, OnSend, null );
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

		public static int CoalesceSleep { get; set; } = -1;

		private void OnSend( IAsyncResult asyncResult )
		{
			m_Sending = false;

			if ( Socket == null )
				return;

			try
			{
				var bytes = Socket.EndSend( asyncResult );

				if ( bytes <= 0 )
				{
					Dispose( false );
					return;
				}

				Outgoing += bytes;
				m_NetServer.OnSend( this, bytes );

				if ( m_Disposing && !m_DisposeFinished )
				{
					FinishDispose();
					return;
				}

				m_NextCheckActivity = DateTime.UtcNow + TimeSpan.FromMinutes( 1.2 );

				if ( CoalesceSleep >= 0 )
					Thread.Sleep( CoalesceSleep );

				var length = 0;
				byte[] queued;

				lock ( m_SendQueue )
					queued = m_SendQueue.Dequeue( ref length );

				if ( queued != null )
				{
					Socket.BeginSend( queued, 0, length, SocketFlags.None, OnSend, null );
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

			if ( Socket == null )
				return false;

			if ( DateTime.UtcNow < m_NextCheckActivity )
				return true;

			log.Info( "Client: {0}: Disconnecting due to inactivity...", this );

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

			if ( Socket == null || m_Disposing )
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
				Socket.Shutdown( SocketShutdown.Both );
			}
			catch ( SocketException ex )
			{
				TraceException( ex );
			}

			try
			{
				Socket.Close();
			}
			catch ( SocketException ex )
			{
				TraceException( ex );
			}

			if ( m_RecvBuffer != null )
				m_ReceiveBufferPool.ReleaseBuffer( m_RecvBuffer );

			Socket = null;

			Buffer = null;
			m_RecvBuffer = null;
			Running = false;

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
				using ( var op = new StreamWriter( Path.Combine( Core.Config.LogDirectory, "network-errors.log" ), true ) )
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
				log.Info( "{0}", ex );
			}
			catch { }
		}
	}
}
