using System;
using Server;

namespace Server.Items
{
	public class PillarOfStrength : LargeStoneShield
	{
		public override int LabelNumber { get { return 1113533; } } // Pillar of Strength

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public PillarOfStrength()
		{
			Hue = 2017;

			Attributes.BonusStr = 10;
			Attributes.BonusHits = 10;
			Attributes.WeaponDamage = 20;
			Resistances.Physical = 10;
		}

		public PillarOfStrength( Serial serial )
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