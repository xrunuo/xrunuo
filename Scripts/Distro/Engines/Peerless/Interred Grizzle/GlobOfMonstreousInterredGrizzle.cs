using System;
using Server;

namespace Server.Items
{
	public class GlobOfMonstrousInterredGrizzle : Item
	{
		public override int LabelNumber { get { return 1072117; } } // Glob of Monstrous Interred Grizzle

		[Constructable]
		public GlobOfMonstrousInterredGrizzle()
			: base( 0x2F3 )
		{
			Weight = 1.0;
		}

		public GlobOfMonstrousInterredGrizzle( Serial serial )
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