using System;
using Server;

namespace Server.Items
{
	public class KneadingBowl : Item
	{
		public override int LabelNumber { get { return 1113637; } } // kneading bowl

		[Constructable]
		public KneadingBowl()
			: base( 0x10E3 )
		{
			Weight = 1.0;
		}

		public KneadingBowl( Serial serial )
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