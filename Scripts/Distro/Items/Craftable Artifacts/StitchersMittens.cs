using System;
using Server.Items;

namespace Server.Items
{
	public class StitchersMittens : LeafGloves
	{
		public override int LabelNumber { get { return 1072932; } } // Stitcher's Mittens

		[Constructable]
		public StitchersMittens()
		{
			Hue = 0x481;

			SkillBonuses.SetValues( 0, SkillName.Healing, 10.0 );

			Attributes.BonusDex = 5;
			Attributes.LowerRegCost = 30;

			Resistances.Physical = 18;
			Resistances.Cold = 18;
		}

		public StitchersMittens( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			if ( version < 1 )
			{
				Resistances.Physical = 18;
				Resistances.Cold = 18;
			}
		}
	}
}