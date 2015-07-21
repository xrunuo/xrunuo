using System;
using Server.Items;

namespace Server.Mobiles
{
	public class RideableBoura : EtherealMount
	{
		public override int LabelNumber { get { return 1150006; } } // Rideable Boura Statuette

		[Constructable]
		public RideableBoura()
			: base( 0x46F9, 0x3EC6 )
		{
			SolidHue = true;
		}

		public RideableBoura( Serial serial )
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