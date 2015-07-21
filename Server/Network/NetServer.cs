//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using Server.Accounting;

namespace Server.Network
{
	public delegate void ConnectionCreated( NetState state );
	public delegate void DataReceived( NetState state, ByteQueue buffer, out bool throttle );
	public delegate void ConnectionDisposed( NetState state );

	public class NetServer
	{
		public event ConnectionCreated Connected;
		public event DataReceived Received;
		public event ConnectionDisposed Disconnected;

		private List<Listener> m_Listeners;
		private List<NetState> m_NetStates;
		private Queue<NetState> m_PendingQueue;
		private Queue<NetState> m_ThrottledQueue;
		private ConcurrentQueue<NetState> m_DisposedQueue;
		private long m_Incoming, m_Outgoing;

		public NetServer( Listener listener )
		{
			m_Listeners = new List<Listener>() { listener };
			m_NetStates = new List<NetState>();
			m_PendingQueue = new Queue<NetState>();
			m_ThrottledQueue = new Queue<NetState>();
			m_DisposedQueue = new ConcurrentQueue<NetState>();
		}

		public long Incoming { get { return m_Incoming; } }
		public long Outgoing { get { return m_Outgoing; } }

		public void AddListener( Listener listener )
		{
			m_Listeners.Add( listener );
		}

		public void OnStarted( NetState ns )
		{
			m_NetStates.Add( ns );
		}

		public void OnSend( NetState ns, int byteCount )
		{
			m_Outgoing += byteCount;
		}

		public void OnReceive( NetState ns, int byteCount )
		{
			lock ( m_PendingQueue )
				m_PendingQueue.Enqueue( ns );

			m_Incoming += byteCount;

			//Core.WakeUp();
		}

		public void OnDisposed( NetState ns )
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
				Logger.Error( "NetServer: Error checking connection activity: {0}", ex );
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
				NetState ns = new NetState( socket, this );
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
					NetState ns = m_PendingQueue.Dequeue();

					if ( ns.Running )
					{
						ByteQueue buffer = ns.Buffer;
						bool throttle = false;

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
			int breakout = 0;

			while ( breakout < 200 && !m_DisposedQueue.IsEmpty )
			{
				++breakout;

				NetState ns;

				if ( m_DisposedQueue.TryDequeue( out ns ) )
				{
					m_NetStates.Remove( ns );

					InvokeDisconnected( ns );
				}
			}
		}

		private void InvokeConnected( NetState ns )
		{
			if ( Connected != null )
				Connected( ns );
		}

		private void InvokeDisconnected( NetState ns )
		{
			if ( Disconnected != null )
				Disconnected( ns );
		}

		private void InvokeReceived( NetState ns, ByteQueue buffer, out bool throttle )
		{
			throttle = false;

			if ( Received != null )
				Received( ns, buffer, out throttle );
		}
	}
}