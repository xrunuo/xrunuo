using System;
using Server;

namespace Server.Items
{
	[Flipable( 0x403D, 0x403E )]
	public class GargishPainting : Item
	{
		public override int LabelNumber { get { return 1095317; } } // gargish painting

		[Constructable]
		public GargishPainting()
			: base( 0x403D )
		{
			Weight = 10.0;
		}

		public GargishPainting( Serial serial )
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
