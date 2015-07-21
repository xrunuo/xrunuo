using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x4042, 0x4043 )]
	public class GargishVase : Item
	{
		public override int LabelNumber { get { return 1095322; } } // gargish vase

		[Constructable]
		public GargishVase()
			: base( 0x4042 )
		{
			Weight = 10.0;
		}

		public GargishVase( Serial serial )
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
