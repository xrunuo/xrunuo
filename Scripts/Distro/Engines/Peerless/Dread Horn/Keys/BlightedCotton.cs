using System;
using Server;

namespace Server.Items
{
	public class BlightedCotton : TransientItem
	{
		public override int LabelNumber { get { return 1074331; } } // Blighted Cotton

		[Constructable]
		public BlightedCotton()
			: base( 0x02DB, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 0x1C5;
		}

		public BlightedCotton( Serial serial )
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