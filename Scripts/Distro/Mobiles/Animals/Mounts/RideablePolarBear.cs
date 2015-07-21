using System;
using Server.Items;

namespace Server.Mobiles
{
	public class RideablePolarBear : EtherealMount
	{
		public override int LabelNumber { get { return 1076159; } } // Rideable Polar Bear

		[Constructable]
		public RideablePolarBear()
			: base( 0x20E1, 0x3EC5 )
		{
			SolidHue = true;
		}

		public RideablePolarBear( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				SolidHue = true;
		}
	}
}