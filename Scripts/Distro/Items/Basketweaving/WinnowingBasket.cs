using System;
using Server;

namespace Server.Items
{
	public class WinnowingBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 3; } }
		public override int NeededReeds { get { return 2; } }

		public override int LabelNumber { get { return 1112355; } } // winnowing basket

		[Constructable]
		public WinnowingBasket()
			: base( 0x1882 )
		{
			Weight = 1.0;
		}

		public WinnowingBasket( Serial serial )
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