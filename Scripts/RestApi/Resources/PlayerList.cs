using System;
using System.Linq;
using System.Net;

using Server;
using Server.Network;

namespace Server.Engines.RestApi
{
	[Path( "/v1/players" )]
	public class PlayerListController : BaseController
	{
		public override object HandleRequest( Request request )
		{
			var clients = GameServer.Instance.Clients;

			var onlinePlayers = clients
				.Select( client => client.Mobile )
				.Where( m => m != null );

			return new
			{
				Players = onlinePlayers.Select( m => new
				{
					Player = new
					{
						Name = m.Name,
						Serial = m.Serial.Value,
					}
				} )
			};
		}
	}
}
