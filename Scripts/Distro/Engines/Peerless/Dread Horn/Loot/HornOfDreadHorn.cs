using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class HornOfDreadHorn : Item
	{
		public override int LabelNumber { get { return 1032636; } } // horn of dread horn

		private static int[] m_IDs = new int[]
		{
			0x315C,
			0x315D
		};

		[Constructable]
		public HornOfDreadHorn()
			: base( m_IDs[Utility.Random( 2 )] )
		{
			Weight = 1.0;
		}

		public HornOfDreadHorn( Serial serial )
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