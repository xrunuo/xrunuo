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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Events;

namespace Server.Network
{
	public class Listener : IDisposable
	{
		private Socket m_Listener;
		private bool m_Disposed;
		private int m_ThisPort;

		private Queue m_Accepted;

		private AsyncCallback m_OnAccept;

		private static Socket[] m_EmptySockets = new Socket[0];

		public int UsedPort
		{
			get { return m_ThisPort; }
		}

		private static int m_Port = 2593;

		public static int Port
		{
			get
			{
				return m_Port;
			}
			set
			{
				m_Port = value;
			}
		}

		public Listener( int port )
		{
			m_ThisPort = port;
			m_Disposed = false;
			m_Accepted = new Queue();
			m_OnAccept = new AsyncCallback( OnAccept );

			m_Listener = Bind( IPAddress.Any, port );

			try
			{
				IPHostEntry iphe = Dns.GetHostEntry( Dns.GetHostName() );

				List<IPAddress> list = new List<IPAddress>();
				list.Add( IPAddress.Loopback );

				Console.WriteLine( "Address: {0}:{1}", IPAddress.Loopback, port );

				IPAddress[] ips = iphe.AddressList;

				for ( int i = 0; i < ips.Length; ++i )
				{
					if ( !list.Contains( ips[i] ) )
					{
						list.Add( ips[i] );

						Console.WriteLine( "Address: {0}:{1}", ips[i], port );
					}
				}

				list.Clear();
			}
			catch
			{
			}
		}

		private Socket Bind( IPAddress ip, int port )
		{
			IPEndPoint ipep = new IPEndPoint( ip, port );

			Socket s = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

			try
			{
				s.Bind( ipep );
				s.Listen( 300 );

				s.BeginAccept( m_OnAccept, s );

				return s;
			}
			catch ( Exception e )
			{
				Console.WriteLine( "Listener bind exception:" );
				Console.WriteLine( e );

				try { s.Shutdown( SocketShutdown.Both ); }
				catch { }

				try { s.Close(); }
				catch { }

				return null;
			}
		}

		private void OnAccept( IAsyncResult asyncResult )
		{
			Socket accepted = null;

			try
			{
				accepted = m_Listener.EndAccept( asyncResult );
			}
			catch ( SocketException ex )
			{
				NetState.TraceException( ex );
			}
			catch ( ObjectDisposedException )
			{
				return;
			}

			if ( accepted != null )
			{
				if ( VerifySocket( accepted ) )
				{
					Enqueue( accepted );
				}
				else
				{
					Release( accepted );
				}
			}

			try
			{
				m_Listener.BeginAccept( m_OnAccept, m_Listener );
			}
			catch ( SocketException ex )
			{
				NetState.TraceException( ex );
			}
			catch ( ObjectDisposedException )
			{
			}
		}

		private bool VerifySocket( Socket socket )
		{
			try
			{
				SocketConnectEventArgs args = new SocketConnectEventArgs( socket );

				EventSink.Instance.InvokeSocketConnect( args );

				return args.AllowConnection;
			}
			catch ( Exception ex )
			{
				NetState.TraceException( ex );

				return false;
			}
		}

		private void Enqueue( Socket socket )
		{
			lock ( m_Accepted.SyncRoot )
			{
				m_Accepted.Enqueue( socket );
			}

			//Core.WakeUp();
		}

		private void Release( Socket socket )
		{
			try
			{
				socket.Shutdown( SocketShutdown.Both );
			}
			catch ( SocketException ex )
			{
				NetState.TraceException( ex );
			}

			try
			{
				socket.Close();
			}
			catch ( SocketException ex )
			{
				NetState.TraceException( ex );
			}
		}

		public Socket[] Slice()
		{
			lock ( m_Accepted.SyncRoot )
			{
				if ( m_Accepted.Count == 0 )
					return m_EmptySockets;

				object[] array = m_Accepted.ToArray();
				m_Accepted.Clear();

				Socket[] sockets = new Socket[array.Length];

				Array.Copy( array, sockets, array.Length );

				return sockets;
			}
		}

		public void Dispose()
		{
			if ( !m_Disposed )
			{
				m_Disposed = true;

				if ( m_Listener != null )
				{
					try { m_Listener.Shutdown( SocketShutdown.Both ); }
					catch { }

					try { m_Listener.Close(); }
					catch { }

					m_Listener = null;
				}
			}
		}
	}
}