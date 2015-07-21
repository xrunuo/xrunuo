using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x2DEB, 0x2DEC, 0x2DED, 0x2DEE )]
	public class CozyElvenChair : Item
	{
		[Constructable]
		public CozyElvenChair()
			: base( 0x2DEB )
		{
			Weight = 20.0;
		}

		public CozyElvenChair( Serial serial )
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