using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class DreadHornTaintedMushroom : Item
	{
		public override int LabelNumber { get { return 1075088; } } // Dread Horn Tainted Mushroom

		private static int[] m_IDs = new int[]
		{
			0x222E,
			0x222F,
			0x2230,
			0x2231
		};

		[Constructable]
		public DreadHornTaintedMushroom()
			: base( m_IDs[Utility.Random( 4 )] )
		{
			Weight = 1.0;
		}

		public DreadHornTaintedMushroom( Serial serial )
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