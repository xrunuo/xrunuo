using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class HydraScale : Item
	{
		public override int LabelNumber { get { return 1074760; } } // A hydra scale.

		[Constructable]
		public HydraScale()
			: base( 0x26B6 )
		{
			Hue = 700;
			LootType = LootType.Blessed;
			Weight = 1.0;
			Stackable = true;
		}

		public HydraScale( Serial serial )
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
