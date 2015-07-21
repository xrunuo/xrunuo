using System;
using Server;

namespace Server.Items
{
	public class GoldBricks : Item
	{
		public override int LabelNumber { get { return 1063489; } } // Gold Bricks

		[Constructable]
		public GoldBricks()
			: base( 0x1BEB )
		{
			Weight = 1.0;
		}

		public GoldBricks( Serial serial )
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

			if ( Weight == 0.0 )
				Weight = 1.0;
		}
	}
}