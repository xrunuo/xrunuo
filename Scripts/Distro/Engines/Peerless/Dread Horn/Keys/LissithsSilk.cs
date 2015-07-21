using System;
using Server;

namespace Server.Items
{
	public class LissithsSilk : TransientItem
	{
		public override int LabelNumber { get { return 1074333; } } // Lissith's Silk

		[Constructable]
		public LissithsSilk()
			: base( 0x2001, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 21;
			LootType = LootType.Blessed;
		}

		public LissithsSilk( Serial serial )
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