using System;
using Server;

namespace Server.Items
{
	public class BreastplateOfTheBerserker : GargishPlatemailChest
	{
		public override int LabelNumber { get { return 1113539; } } // Breastplate of the Berserker

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BreastplateOfTheBerserker()
		{
			Hue = 0x21;

			Attributes.BonusHits = 5;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 15;
			Attributes.LowerManaCost = 4;

			Resistances.Physical = 10;
			Resistances.Fire = 10;
			Resistances.Poison = 5;
		}

		public BreastplateOfTheBerserker( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
