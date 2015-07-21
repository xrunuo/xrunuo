using System;
using Server;
using Server.Mobiles;
using Server.Engines.MLQuests;

namespace Server.Items
{
	public class BedlamTeleport : Item
	{
		private static readonly Point3D BedlamEntrance = new Point3D( 121, 1682, 0 );

		public override int LabelNumber { get { return 1074161; } } // Access to Bedlam by invitation only

		[Constructable]
		public BedlamTeleport()
			: base( 0x124D )
		{
			Weight = 0.0;
			Movable = false;
		}

		public BedlamTeleport( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

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

			if ( pm.Bedlam )
			{
				BaseCreature.TeleportPets( pm, BedlamEntrance, from.Map );
				pm.MoveToWorld( BedlamEntrance, Map );
			}
			else
			{
				pm.SendLocalizedMessage( 1074276 ); // You press and push on the iron maiden, but nothing happens.
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