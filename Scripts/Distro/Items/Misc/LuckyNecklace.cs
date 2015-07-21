using System;
using Server;

namespace Server.Items
{
	public class LuckyNecklace : BaseNecklace
	{
		public override int LabelNumber { get { return 1075239; } } // Lucky Necklace

		[Constructable]
		public LuckyNecklace()
			: base( 0x1088 )
		{
			Hue = 0x481;
			Weight = 0.1;
			Attributes.Luck = 200;
			LootType = LootType.Blessed;
		}

		public LuckyNecklace( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}