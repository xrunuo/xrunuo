using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x2DE3, 0x2DE4, 0x2DE5, 0x2DE6 )]
	public class OrnateElvenChair : Item
	{
		[Constructable]
		public OrnateElvenChair()
			: base( 0x2DE3 )
		{
			Weight = 20.0;
		}

		public OrnateElvenChair( Serial serial )
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