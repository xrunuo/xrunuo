using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class LabyrinthDoor : Item
	{
		[Constructable]
		public LabyrinthDoor()
			: base( 0x248B )
		{
			Weight = 0.0;
			Movable = false;
		}

		public LabyrinthDoor( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.InLOS( this.GetWorldLocation() ) )
			{
				from.SendLocalizedMessage( 502800 ); // You can't see that.
				return;
			}

			if ( from.GetDistanceToSqrt( this.GetWorldLocation() ) > 2 )
			{
				from.SendLocalizedMessage( 500446 ); // That is too far away.
				return;
			}

			Point3D dest = new Point3D( 330, 1973, 0 );
			Map map = Map.Malas;

			BaseCreature.TeleportPets( from, dest, map );
			from.MoveToWorld( dest, map );
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