using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class ThrashersTail : Item
	{
		public override int LabelNumber { get { return 1074230; } } // Thrasher's Tail

		[Constructable]
		public ThrashersTail()
			: base( 0x1A9D )
		{
			Hue = 1109;
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public ThrashersTail( Serial serial )
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
		}
	}
}
