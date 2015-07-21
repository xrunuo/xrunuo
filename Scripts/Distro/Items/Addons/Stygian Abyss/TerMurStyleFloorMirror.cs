using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x403A, 0x4046 )]
	public class TerMurStyleFloorMirror : Item
	{
		public override int LabelNumber { get { return 1095326; } } // Ter-Mur style floor mirror

		[Constructable]
		public TerMurStyleFloorMirror()
			: base( 0x403A )
		{
			Weight = 1.0;
		}

		public TerMurStyleFloorMirror( Serial serial )
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
