using System;
using Server;

namespace Server.Items
{
	public class PetrifiedSnake : SerpentstoneStaff
	{
		public override int LabelNumber { get { return 1113528; } } // Petrified Snake

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public PetrifiedSnake()
		{
			Hue = 2001;

			Slayer = SlayerName.Reptile;
			AbsorptionAttributes.PoisonEater = 20;
			WeaponAttributes.HitMagicArrow = 30;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 50;
			Resistances.Poison = 10;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = cold = nrgy = 0;
			pois = 100;
		}

		public PetrifiedSnake( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}