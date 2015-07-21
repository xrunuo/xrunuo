using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public abstract class SETeleporter : DynamicTeleporter
	{
		public override int LabelNumber { get { return 1016273; } } // Teleporter

		public SETeleporter()
		{
			ItemID = 0x51C;
		}

		public override bool OnMoveOver( Mobile m )
		{
			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
			{
				Point3D loc = Point3D.Zero;
				Map map = null;

				if ( GetDestination( pm, ref loc, ref map ) )
				{
					BaseCreature.TeleportPets( pm, loc, map );

					pm.PlaySound( 0x1FE );
					pm.MoveToWorld( loc, map );

					return false;
				}
				else
				{
					pm.SendLocalizedMessage( 1063198 ); // You stand on the strange floor tile but nothing happens.
				}
			}

			return true;
		}

		public SETeleporter( Serial serial )
			: base( serial )
		{
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
