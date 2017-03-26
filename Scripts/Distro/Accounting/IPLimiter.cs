using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Accounting;
using Server.Network;

namespace Server.Misc
{
	public class IPLimiter
	{
		public static readonly bool Enabled = Core.Config.Login.MaxLoginsPerIP > 0;
		public static readonly bool SocketBlock = false; // true to block at connection, false to block at login request

		public static readonly int MaxAddresses = Core.Config.Login.MaxLoginsPerIP;

		public static bool Verify( IPAddress ourAddress, Account account )
		{
			if ( !Enabled )
				return true;

			int count = 0;

			foreach ( var compState in GameServer.Instance.Clients )
			{
				if ( ourAddress.Equals( compState.Address ) && compState.Account != account )
				{
					++count;

					if ( count > MaxAddresses )
						return false;
				}
			}

			return true;
		}
	}
}