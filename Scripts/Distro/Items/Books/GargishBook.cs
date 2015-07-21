using System;
using Server;

namespace Server.Items
{
	public class GargishBook : BaseBook
	{
		public override int LabelNumber { get { return 1095951; } } // gargish book

		[Constructable]
		public GargishBook()
			: base( 0x42B8, 200, true )
		{
		}

		public GargishBook( Serial serial )
			: base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}
	}
}