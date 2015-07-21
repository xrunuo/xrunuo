using System;
using Server;

namespace Server.Items
{
	public class FishOrigami : Item
	{
		public override int LabelNumber { get { return 1030301; } } // a delicate origami fish

		[Constructable]
		public FishOrigami()
			: base( 0x283D )
		{
			LootType = LootType.Blessed;

			Weight = 1.0;
		}

		public FishOrigami( Serial serial )
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
