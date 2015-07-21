using System;
using Server;

namespace Server.Items
{
	public class GemmedCirclet : Circlet
	{
		public override int LabelNumber { get { return 1031120; } } // Gemmed Circlet

		[Constructable]
		public GemmedCirclet()
		{
			ItemID = 0x2B70;
		}

		public GemmedCirclet( Serial serial )
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