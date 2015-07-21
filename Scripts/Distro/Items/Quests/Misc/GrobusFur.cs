using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class GrobusFur : Item
	{
		public override int LabelNumber { get { return 1074676; } } // Grobu's Fur

		[Constructable]
		public GrobusFur()
			: base( 0x11F4 )
		{
			Hue = 1109;
			LootType = LootType.Blessed;
			Weight = 10.0;
		}

		public GrobusFur( Serial serial )
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
