using System;

namespace Server.Scripts.Commands
{
	public static class Traffic
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Traffic", AccessLevel.Player, new CommandEventHandler( Traffic_OnCommand ) );
		}

		[Usage( "Traffic" )]
		[Description( "Showing incoming and outgoing traffic for your session" )]
		private static void Traffic_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Incoming traffic: " + Utility.FormatByteAmount( e.Mobile.Client.Incoming ) );
			e.Mobile.SendMessage( "Outgoing traffic: " + Utility.FormatByteAmount( e.Mobile.Client.Outgoing ) );
		}
	}
}
