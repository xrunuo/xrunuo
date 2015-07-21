using System;
using Server;

namespace Server.Items
{
	public class AlchemistsBandage : Item
	{
		public override int LabelNumber { get { return 1075452; } } // Alchemist's Bandage
		public override int QuestItemHue { get { return PrivateHue; } }

		[Constructable]
		public AlchemistsBandage()
			: base( 0xE21 )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
			Hue = 0x482;
		}

		public AlchemistsBandage( Serial serial )
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