
using System;

namespace Server.Items
{
	public class ElvenPodium : Item
	{
		public override int LabelNumber { get { return 1031741; } }
		[Constructable]
		public ElvenPodium()
			: base( 0x2DDE )
		{
			Weight = 0.0;
		}

		public ElvenPodium( Serial serial )
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
}

