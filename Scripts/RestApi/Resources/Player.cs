using System;
using System.Net;

using Server;
using Server.Mobiles;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/players/{id}" )]
	public class PlayerLocator : BaseLocator
	{
		public override BaseController Locate( Parameters parameters )
		{
			BaseController controller = null;

			try
			{
				var serial = Convert.ToInt32( parameters["id"] );
				var pm = World.FindMobile( serial ) as PlayerMobile;

				if ( pm != null )
					controller = new PlayerController( pm );
			}
			catch
			{
			}

			return controller;
		}
	}

	public class PlayerController : BaseProtectedController
	{
		public override AccessLevel RequiredAccessLevel { get { return AccessLevel.Player; } }

		private PlayerMobile m_Mobile;

		public PlayerController( PlayerMobile pm )
		{
			m_Mobile = pm;
		}

		public override void AccessCheck( HttpListenerContext context )
		{
			base.AccessCheck( context );

			var account = GetAccount( context );
			if ( account.AccessLevel <= AccessLevel.Player )
			{
				if ( account != m_Mobile.Account )
					throw new AccessDenied( "Cannot see other player details" );
			}
		}

		public override object HandleRequest( HttpListenerContext context )
		{
			return new
			{
				Player = new
				{
					Name = m_Mobile.Name,
					Serial = m_Mobile.Serial,
					Fame = m_Mobile.Fame,
					Karma = m_Mobile.Karma,
					Kills = m_Mobile.Kills,
					ShortKills = m_Mobile.ShortTermMurders,
				}
			};
		}
	}
}
