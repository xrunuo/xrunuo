using System;

namespace Server.Items
{
	public class QuiverOfInfinity : BaseQuiver
	{
		public override int LabelNumber { get { return 1075201; } } // Quiver of Infinity

		public override int WeightReduction { get { return 30; } }

		[Constructable]
		public QuiverOfInfinity()
		{
			LootType = LootType.Blessed;
			ItemID = 0x2B02;
			Attributes.LowerAmmoCost = 20;
			Attributes.DefendChance = 5;
		}

		public QuiverOfInfinity( Serial serial )
			: base( serial )
		{
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

			LootType = LootType.Blessed;
		}
	}
}
