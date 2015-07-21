using System;
using Server;

namespace Server.Items
{
	public class Talisman : BaseTalisman
	{
		private static int[] m_IDs = new int[]
		{
			0x2F5B,
			0x2F5A,
			0x2F59,
			0x2F58
		};

		[Constructable]
		public Talisman()
			: base( m_IDs[Utility.Random( 4 )] )
		{
			Weight = 1.0;
		}

		public Talisman( Serial serial )
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
