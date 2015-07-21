using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class GamanHorns : Item
	{
		public override int LabelNumber { get { return 1074557; } } // Gaman Horns

		[Constructable]
		public GamanHorns()
			: this( 1 )
		{
		}

		[Constructable]
		public GamanHorns( int amount )
			: base( 0x1084 )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
			Hue = 917;
			Stackable = true;
			Amount = amount;
		}

		public GamanHorns( Serial serial )
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
