using System;
using Server;

namespace Server.Items
{
	public class PixieLeg : Item
	{
		public override int LabelNumber { get { return 1074613; } } // Pixie Leg

		[Constructable]
		public PixieLeg()
			: this( 1 )
		{
		}

		[Constructable]
		public PixieLeg( int amount )
			: base( 0x1608 )
		{
			Weight = 1.0;
			Amount = amount;
			Stackable = true;
			LootType = LootType.Blessed;
			Hue = 450;
		}

		public PixieLeg( Serial serial )
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