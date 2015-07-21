using System;
using Server;

namespace Server.Items
{
	public class FaerieFire : ElvenCompositeLongBow
	{
		public override int LabelNumber { get { return 1072908; } } // Faerie Fire

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FaerieFire()
		{
			WeaponAttributes.Balanced = 1;
			Hue = 1360;
			Attributes.BonusDex = 3;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 60;
			WeaponAttributes.HitFireball = 25;
		}
		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = nrgy = cold = pois = 0;
			fire = 100;
		}

		public FaerieFire( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}