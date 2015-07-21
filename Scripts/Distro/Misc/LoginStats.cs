using System;
using Server.Network;
using Server.Accounting;
using Server.Events;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Instance.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			int userCount = GameServer.Instance.ClientCount;
			Mobile m = args.Mobile;

			m.SendMessage( "Welcome to {0}, {1}! There {2} currently {3} user{4} online.",
				String.Format( "{0}{1}", Environment.Config.ServerName, TestCenter.Enabled ? " Test Center" : String.Empty ),
				args.Mobile.Name,
				userCount == 1 ? "is" : "are",
				userCount, userCount == 1 ? "" : "s" );

			if ( TestCenter.Enabled )
				m.SendMessage( "Server Version: X-RunUO {0}.{1}.{2}, Build {3}", Environment.CoreVersion.Major, Environment.CoreVersion.Minor, Environment.CoreVersion.Build, Environment.CoreVersion.Revision );
		}
	}
}