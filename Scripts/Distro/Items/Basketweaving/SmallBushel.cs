using System;
using Server;

namespace Server.Items
{
	public class SmallBushel : WeavableBasket
	{
		public override int NeededShafts { get { return 2; } }
		public override int NeededReeds { get { return 1; } }

		public override int LabelNumber { get { return 1112337; } } // small bushel

		[Constructable]
		public SmallBushel()
			: base( 0x9B1 )
		{
			Weight = 2.0;
		}

		public SmallBushel( Serial serial )
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