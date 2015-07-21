using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x2DF5, 0x2DF6 )]
	public class ReadingChair : Item
	{
		public override int LabelNumber { get { return 1072160; } } // Reading Chair 

		[Constructable]
		public ReadingChair()
			: base( 0x2DF5 )
		{
		}

		public ReadingChair( Serial serial )
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