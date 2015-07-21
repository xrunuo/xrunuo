using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Scripts.Gumps;

namespace Server.Scripts.Commands
{
	public class Caps
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Caps", AccessLevel.Counselor, new CommandEventHandler( Caps_OnCommand ) );
		}

		private class CapsTarget : Target
		{
			public CapsTarget()
				: base( -1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					from.SendGump( new CapsGump( from, (Mobile) o ) );
				}
			}
		}

		[Usage( "Caps" )]
		[Description( "Opens a menu where you can view or edit Caps of a targeted mobile." )]
		private static void Caps_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new CapsTarget();
		}
	}
}