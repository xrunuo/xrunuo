using System;
using Server;

namespace Server.Items
{
	public class SashOfMight : BodySash
	{
		public override int LabelNumber { get { return 1075412; } } // Sash of Might

		[Constructable]
		public SashOfMight()
			: base( 39 )
		{
			LootType = LootType.Blessed;
		}

		public SashOfMight( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}