using System;
using Server;

namespace Server.Items
{
	public class GiantSteps : GargishStoneLeggings
	{
		public override int LabelNumber { get { return 1113537; } } // Giant Steps

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public GiantSteps()
		{
			Hue = 556;

			Attributes.BonusStr = 5;
			Attributes.BonusDex = 5;
			Attributes.BonusHits = 5;
			Attributes.RegenHits = 2;
			Attributes.WeaponDamage = 10;

			Resistances.Physical = 12;
			Resistances.Fire = 10;
			Resistances.Energy = 6;
		}

		public GiantSteps( Serial serial )
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
