using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class PrismOfLightTeleport : Teleporter
	{
		[Constructable]
		public PrismOfLightTeleport()
		{
			MapDest = Map.Felucca;
			PointDest = new Point3D( 6474, 188, 0 );
		}

		public PrismOfLightTeleport( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			Container pack = m.Backpack;
			bool found = false;

			if ( pack != null )
			{
				foreach ( Item item in pack.Items )
				{
					if ( item is PrismOfLightAdmissionTicket )
					{
						found = true;
					}
				}
			}

			if ( found )
			{
				return base.OnMoveOver( m );
			}
			else
			{
				m.SendLocalizedMessage( 1074277 ); // No admission without a ticket.
				return true;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}