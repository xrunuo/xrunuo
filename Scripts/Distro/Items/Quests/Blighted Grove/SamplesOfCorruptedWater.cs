using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class SamplesOfCorruptedWater : Item
	{
		public override int LabelNumber { get { return 1074999; } } // samples of corrupted water

		[Constructable]
		public SamplesOfCorruptedWater()
			: base( 0x0EFE )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public SamplesOfCorruptedWater( Serial serial )
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