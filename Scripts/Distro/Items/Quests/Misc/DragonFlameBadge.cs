using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class DragonFlameBadge : Item
	{
		public override int LabelNumber { get { return 1073141; } } // A Dragon Flame Sect Badge

		[Constructable]
		public DragonFlameBadge()
			: base( 0x23E )
		{
			Stackable = true;
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public DragonFlameBadge( Serial serial )
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
