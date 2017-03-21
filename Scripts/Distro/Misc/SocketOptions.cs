using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Misc;
using Server.Network;
using Server.Events;

namespace Server
{
	public class SocketOptions
	{
		private const bool NagleEnabled = false; // Should the Nagle algorithm be enabled? This may reduce performance
		private const int CoalesceBufferSize = 512; // MSS that the core will use when buffering packets

		public static void Initialize()
		{
			EventSink.SocketConnect += new SocketConnectEventHandler( EventSink_SocketConnect );

			SendQueue.CoalesceBufferSize = CoalesceBufferSize;
		}

		private static void EventSink_SocketConnect( SocketConnectEventArgs e )
		{
			if ( !e.AllowConnection )
				return;

			if ( !NagleEnabled )
			{
				// RunUO uses its own algorithm
				e.Socket.SetSocketOption( SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1 );
			}
		}
	}
}
