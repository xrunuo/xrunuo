using System;
using Server;

namespace Server.Items
{
	public class PicnicBasketW : WeavableBasket
	{
		public override int NeededShafts { get { return 2; } }
		public override int NeededReeds { get { return 1; } }

		public override int LabelNumber { get { return 1112356; } } // picnic basket

		[Constructable]
		public PicnicBasketW()
			: base( 0xE7A )
		{
			Weight = 2.0;
		}

		public PicnicBasketW( Serial serial )
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