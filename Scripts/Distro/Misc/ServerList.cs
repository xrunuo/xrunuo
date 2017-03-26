using System;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Network;
using Server.Accounting;
using Server.Events;

namespace Server.Misc
{
	public class ServerList
	{
		public static string ServerName = Core.Config.ServerName;
		public static string Address = Core.Config.Network.Bind[0].Address.ToString();

		public static void Initialize()
		{
			Listener.Port = Core.Config.Network.Bind[0].Port;
			EventSink.ServerList += new ServerListEventHandler( EventSink_ServerList );
		}

		public static void EventSink_ServerList( ServerListEventArgs e )
		{
			try
			{
				IPAddress ipAddr;

				if ( Resolve( Address, out ipAddr ) )
					e.AddServer( ServerName, new IPEndPoint( ipAddr, Listener.Port ) );
				else
					e.Rejected = true;
			}
			catch
			{
				e.Rejected = true;
			}
		}

		public static bool Resolve( string addr, out IPAddress outValue )
		{
			try
			{
				outValue = IPAddress.Parse( addr );
				return true;
			}
			catch
			{
				try
				{
					IPHostEntry iphe = Dns.GetHostEntry( addr );

					if ( iphe.AddressList.Length > 0 )
					{
						outValue = iphe.AddressList[iphe.AddressList.Length - 1];
						return true;
					}
				}
				catch
				{
				}
			}

			outValue = IPAddress.None;
			return false;
		}

		private static bool IsLocalMachine( NetState state )
		{
			IPAddress theirAddress = state.Address;

			if ( IPAddress.IsLoopback( theirAddress ) )
				return true;

			bool contains = false;
			IPHostEntry iphe = Dns.GetHostEntry( Dns.GetHostName() );

			for ( int i = 0; !contains && i < iphe.AddressList.Length; ++i )
				contains = theirAddress.Equals( iphe.AddressList[i] );

			return contains;
		}
	}
}
