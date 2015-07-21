using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class SerpentFangBadge : Item
	{
		public override int LabelNumber { get { return 1073139; } } // A Serpent Fang Sect Badge

		[Constructable]
		public SerpentFangBadge()
			: base( 0x23C )
		{
			Stackable = true;
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public SerpentFangBadge( Serial serial )
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
