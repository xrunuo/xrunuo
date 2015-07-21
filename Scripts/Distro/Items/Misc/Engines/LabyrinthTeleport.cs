using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class LabyrinthTeleport : Item
	{
		[Constructable]
		public LabyrinthTeleport()
			: base( 0x2FD4 )
		{
			Weight = 0.0;
			Movable = false;
		}

		public LabyrinthTeleport( Serial serial )
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

			Point3D dest = new Point3D( 1731, 978, -80 );
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