using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x4044, 0x4045 )]
	public class TerMurStyleWallMirror : Item
	{
		public override int LabelNumber { get { return 1095324; } } // Ter-Mur style wall mirror

		[Constructable]
		public TerMurStyleWallMirror()
			: base( 0x4044 )
		{
			Weight = 1.0;
		}

		public TerMurStyleWallMirror( Serial serial )
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
