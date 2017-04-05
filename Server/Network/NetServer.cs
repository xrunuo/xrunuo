using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace Server.Network
{
	public delegate void ConnectionCreated( UOSocket state );
	public delegate void DataReceived( UOSocket state, ByteQueue buffer, out bool throttle );
	public delegate void ConnectionDisposed( UOSocket state );

	public class NetServer
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public event ConnectionCreated Connected;
		public event DataReceived Received;
		public event ConnectionDisposed Disconnected;

		private readonly List<Listener> m_Listeners;
		private readonly List<UOSocket> m_NetStates;
		private readonly Queue<UOSocket> m_PendingQueue;
		private readonly Queue<UOSocket> m_ThrottledQueue;
		private readonly ConcurrentQueue<UOSocket> m_DisposedQueue;

		public NetServer( Listener listener )
		{
			m_Listeners = new List<Listener>() { listener };
			m_NetStates = new List<UOSocket>();
			m_PendingQueue = new Queue<UOSocket>();
			m_ThrottledQueue = new Queue<UOSocket>();
			m_DisposedQueue = new ConcurrentQueue<UOSocket>();
		}

		public long Incoming { get; private set; }

		public long Outgoing { get; private set; }

		public void AddListener( Listener listener )
		{
			m_Listeners.Add( listener );
		}

		public void OnStarted( UOSocket ns )
		{
			m_NetStates.Add( ns );
		}

		public void OnSend( UOSocket ns, int byteCount )
		{
			Outgoing += byteCount;
		}

		public void OnReceive( UOSocket ns, int byteCount )
		{
			lock ( m_PendingQueue )
				m_PendingQueue.Enqueue( ns );

			Incoming += byteCount;

			//Core.WakeUp();
		}

		public void OnDisposed( UOSocket ns )
		{
			m_DisposedQueue.Enqueue( ns );
		}

		public void Initialize()
		{
			Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 1.5 ), new TimerCallback( CheckAllAlive ) );
		}

		public void CheckAllAlive()
		{
			try
			{
				foreach ( var state in m_NetStates )
					state.CheckAlive();
			}
			catch ( Exception ex )
			{
				log.Error( "NetServer: Error checking connection activity: {0}", ex );
			}
		}

		public void Slice()
		{
			CheckListener();
			HandlePending();
			FlushAll();
			ProcessDisposedQueue();
		}

		private void CheckListener()
		{
			foreach ( var socket in m_Listeners.SelectMany( listener => listener.Slice() ) )
			{
				var ns = new UOSocket( socket, this );
				ns.Start();

				if ( ns.Running )
					InvokeConnected( ns );
			}
		}

		private void HandlePending()
		{
			lock ( m_PendingQueue )
			{
				while ( m_PendingQueue.Count > 0 )
				{
					var ns = m_PendingQueue.Dequeue();

					if ( ns.Running )
					{
						var buffer = ns.Buffer;
						var throttle = false;

						if ( buffer != null && buffer.Length > 0 )
						{
							lock ( buffer )
							{
								InvokeReceived( ns, buffer, out throttle );
							}
						}

						if ( ns.Running )
						{
							if ( throttle )
								m_ThrottledQueue.Enqueue( ns );
							else
								ns.Continue();
						}
					}
				}

				while ( m_ThrottledQueue.Count > 0 )
					m_PendingQueue.Enqueue( m_ThrottledQueue.Dequeue() );
			}
		}

		public void FlushAll()
		{
			foreach ( var state in m_NetStates )
				state.Flush();
		}

		public void ProcessDisposedQueue()
		{
			var breakout = 0;

			while ( breakout < 200 && !m_DisposedQueue.IsEmpty )
			{
				++breakout;

				UOSocket ns;

				if ( m_DisposedQueue.TryDequeue( out ns ) )
				{
					m_NetStates.Remove( ns );

					InvokeDisconnected( ns );
				}
			}
		}

		private void InvokeConnected( UOSocket ns )
		{
			if ( Connected != null )
				Connected( ns );
		}

		private void InvokeDisconnected( UOSocket ns )
		{
			if ( Disconnected != null )
				Disconnected( ns );
		}

		private void InvokeReceived( UOSocket ns, ByteQueue buffer, out bool throttle )
		{
			throttle = false;

			if ( Received != null )
				Received( ns, buffer, out throttle );
		}
	}
}
