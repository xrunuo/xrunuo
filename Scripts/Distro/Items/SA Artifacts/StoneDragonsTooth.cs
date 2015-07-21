using System;
using Server;

namespace Server.Items
{
	public class StoneDragonsTooth : GargishDagger
	{
		public override int LabelNumber { get { return 1113523; } } // Stone Dragon's Tooth

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public StoneDragonsTooth()
		{
			Hue = 875;

			AbsorptionAttributes.PoisonEater = 10;
			WeaponAttributes.HitMagicArrow = 40;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.RegenHits = 3;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 50;
			Resistances.Fire = 10;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = cold = nrgy = 0;
			pois = 100;
		}

		public StoneDragonsTooth( Serial serial )
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