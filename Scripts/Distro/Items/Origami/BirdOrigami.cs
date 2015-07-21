using System;
using Server;

namespace Server.Items
{
	public class BirdOrigami : Item
	{
		public override int LabelNumber { get { return 1030300; } } // a delicate origami songbird

		[Constructable]
		public BirdOrigami()
			: base( 0x283C )
		{
			LootType = LootType.Blessed;

			Weight = 1.0;
		}

		public BirdOrigami( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();
		}
	}
}
