using System;
using Server;

namespace Server.Items
{
	public class TallBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 3; } }
		public override int NeededReeds { get { return 2; } }

		public override int LabelNumber { get { return 1112299; } } // tall basket

		[Constructable]
		public TallBasket()
			: base( 0x24DB )
		{
			Weight = 1.0;
		}

		public TallBasket( Serial serial )
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