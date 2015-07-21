using System;
using Server;

namespace Server.Items
{
	public interface ICrystal
	{
	}

	public class ShimmeringCrystals : Item, ICrystal
	{
		public override int LabelNumber { get { return 1075095; } } // Shimmering Crystals

		[Constructable]
		public ShimmeringCrystals()
			: base( 0x2206 )
		{
			int crystals = Utility.RandomMinMax( 1, 4 );
			Weight = 1.0;

			switch ( crystals )
			{
				case 1:
					ItemID = ( Utility.RandomMinMax( 0x2206, 0x220E ) );
					break;
				case 2:
					ItemID = ( Utility.RandomMinMax( 0x2210, 0x2218 ) );
					break;
				case 3:
					ItemID = ( Utility.RandomMinMax( 0x221A, 0x2222 ) );
					break;
				case 4:
					ItemID = ( Utility.RandomMinMax( 0x2224, 0x222C ) );
					break;
			}
		}

		public ShimmeringCrystals( Serial serial )
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
