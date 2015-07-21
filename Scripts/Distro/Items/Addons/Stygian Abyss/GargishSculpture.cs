using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x403F, 0x4040 )]
	public class GargishSculpture : Item
	{
		public override int LabelNumber { get { return 1095319; } } // gargish sculpture

		[Constructable]
		public GargishSculpture()
			: base( 0x403F )
		{
			Weight = 255.0;
		}

		public GargishSculpture( Serial serial )
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
