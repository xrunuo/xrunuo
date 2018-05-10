using System;
using System.Linq;
using System.Net;

using Server;
using Server.Accounting;
using Server.Engines.Guilds;
using Server.Network;

namespace Server.Engines.RestApi
{
	[Path( "/status" )]
	public class StatusController : BaseController
	{
		public override object HandleRequest( Request request )
		{
			var onlineCount = GameServer.Instance.ClientCount;
			var accountCount = Accounts.GetAccounts().Count( a => !a.Banned && a.Count > 0 );
			var playerCount = World.Mobiles.Count( m => m.Player );
			var guildCount = Guild.List.Count;

			return new
			{
				Status = new
				{
					Online = onlineCount,
					Accounts = accountCount,
					Characters = playerCount,
					Guilds = guildCount,
				}
			};
		}
	}
}
