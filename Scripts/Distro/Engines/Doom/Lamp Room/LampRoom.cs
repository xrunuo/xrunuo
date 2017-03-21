using System;
using System.Xml;

using Server;
using Server.Mobiles;
using Server.Events;

namespace Server.Regions
{
	public class DoomLampRoom : DungeonRegion
	{
		public static void Initialize()
		{
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
		}

		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			// at OSI for logout from Lamp Room, account may be banned
			Mobile m = e.Mobile;

			if ( m != null && m.IsPlayer && m.Region.Name == "Doom Lamp Room" )
			{
				Rectangle2D rect = new Rectangle2D( 342, 168, 16, 16 );

				int x = Utility.Random( rect.X, rect.Width );
				int y = Utility.Random( rect.Y, rect.Height );

				if ( x >= 345 && x <= 352 && y >= 173 && y <= 179 )
				{
					x = 353;
					y = 172;
				}

				m.MoveToWorld( new Point3D( x, y, -1 ), Map.Malas );
			}
		}

		public DoomLampRoom( XmlElement xml, Map map, Region parent )
			: base( xml, map, parent )
		{
		}

		public override bool CanUseStuckMenu( Mobile m )
		{
			return false;
		}

		public override bool OnSkillUse( Mobile from, SkillName skill )
		{
			// at OSI for logout from Lamp Room, account may be banned

			if ( skill == SkillName.Camping )
				return false;

			return true;
		}
	}
}