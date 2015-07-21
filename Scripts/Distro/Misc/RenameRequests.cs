using System;
using Server;
using Server.Events;

namespace Server.Misc
{
	public class RenameRequests
	{
		public static void Initialize()
		{
			EventSink.Instance.RenameRequest += new RenameRequestEventHandler( EventSink_RenameRequest );
		}

		private static void EventSink_RenameRequest( RenameRequestEventArgs e )
		{
			Mobile from = e.From;
			Mobile targ = e.Target;
			string name = e.Name;

			if ( from.CanSee( targ ) && from.InRange( targ, 12 ) && targ.CanBeRenamedBy( from ) )
			{
				name = name.Trim();

				if ( NameVerification.Validate( name, 1, 16, true, false, true, false, 0, NameVerification.Empty ) )
					targ.Name = name;
				else
					from.SendMessage( "That name is unacceptable." );
			}
		}
	}
}