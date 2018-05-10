using System;
using System.Linq;
using System.Net;

using Server;
using Server.Accounting;
using Server.Engines.Guilds;
using Server.Network;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/status" )]
	public class StatusController : BaseController
	{
		public override object HandleRequest( HttpListenerContext context, Parameters parameters )
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
