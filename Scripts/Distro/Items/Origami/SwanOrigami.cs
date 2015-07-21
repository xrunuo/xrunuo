using System;
using Server;

namespace Server.Items
{
	public class SwanOrigami : Item
	{
		public override int LabelNumber { get { return 1030297; } } // a delicate origami swan

		[Constructable]
		public SwanOrigami()
			: base( 0x2839 )
		{
			LootType = LootType.Blessed;

			Weight = 1.0;
		}

		public SwanOrigami( Serial serial )
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
