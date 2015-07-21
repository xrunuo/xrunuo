using System;
using Server;

namespace Server.Items
{
	public class TallRoundBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 4; } }
		public override int NeededReeds { get { return 3; } }

		public override int LabelNumber { get { return 1112297; } } // tall round basket

		[Constructable]
		public TallRoundBasket()
			: base( 0x9AC )
		{
			Weight = 1.0;
		}

		public TallRoundBasket( Serial serial )
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