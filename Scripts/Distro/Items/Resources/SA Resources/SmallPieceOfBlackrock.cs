using System;
using Server;

namespace Server.Items
{
	public class SmallPieceOfBlackrock : Item
	{
		public override int LabelNumber { get { return 1150016; } } // a small piece of blackrock

		[Constructable]
		public SmallPieceOfBlackrock()
			: base( 0xF26 )
		{
			Hue = 1175;
			Weight = 1.0;
		}

		public SmallPieceOfBlackrock( Serial serial )
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
