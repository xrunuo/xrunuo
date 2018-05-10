using System;
using System.Net;

using Server;
using Server.Mobiles;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/players/{id}" )]
	public class PlayerController : BaseProtectedController
	{
		public override AccessLevel RequiredAccessLevel => AccessLevel.Player;

		public override object HandleRequest( Request request )
		{
			var serial = Convert.ToInt32( request["id"] );
			var pm = World.FindMobile( serial ) as PlayerMobile;

			if ( pm == null )
				throw new NotFound( "No player with serial " + serial );

			var account = request.GetAccount();
			if ( account.AccessLevel <= AccessLevel.Player && account != pm.Account )
				throw new AccessDenied( "Cannot see other player details" );

			return new
			{
				Player = new
				{
					Name = pm.Name,
					Serial = pm.Serial,
					Fame = pm.Fame,
					Karma = pm.Karma,
					Kills = pm.Kills,
					ShortKills = pm.ShortTermMurders,
				}
			};
		}
	}
}
