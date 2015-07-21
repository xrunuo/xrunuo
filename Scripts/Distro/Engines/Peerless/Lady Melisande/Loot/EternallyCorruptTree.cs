using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EternallyCorruptTree : Item
	{
		public override int LabelNumber { get { return 1072093; } } // Eternally Corrupt Tree

		[Constructable]
		public EternallyCorruptTree()
			: base( 0x20FA )
		{
			Hue = Utility.RandomMinMax( 2201, 2224 );
			Weight = 1.0;

		}
		public EternallyCorruptTree( Serial serial )
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
		}
	}
}
