using System;
using Server;

namespace Server.Items
{
	public class RoundBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 3; } }
		public override int NeededReeds { get { return 2; } }

		public override int LabelNumber { get { return 1112293; } } // round basket

		[Constructable]
		public RoundBasket()
			: base( 0x990 )
		{
			Weight = 1.0;
		}

		public RoundBasket( Serial serial )
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