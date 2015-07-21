using System;
using Server;

namespace Server.Items
{
	public class SmallRoundBasket : WeavableBasket
	{
		public override int NeededShafts { get { return 2; } }
		public override int NeededReeds { get { return 1; } }

		public override int LabelNumber { get { return 1112298; } } // small round basket

		[Constructable]
		public SmallRoundBasket()
			: base( 0x24DD )
		{
			Weight = 1.0;
		}

		public SmallRoundBasket( Serial serial )
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