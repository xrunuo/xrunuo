using System;
using Server;

namespace Server.Items
{
	public class KirinBrains : Item
	{
		public override int LabelNumber { get { return 1074612; } } // Ki-rin Brains

		[Constructable]
		public KirinBrains()
			: base( 0x1CF0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 215;
		}

		public KirinBrains( Serial serial )
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