using System;
using Server;

namespace Server.Items
{
	public class SmallSquareBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 2; } }
		public override int NeededReeds { get { return 1; } }

		public override int LabelNumber { get { return 1112296; } } // small square basket

		[Constructable]
		public SmallSquareBasket()
			: base( 0x24D9 )
		{
			Weight = 1.0;
		}

		public SmallSquareBasket( Serial serial )
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