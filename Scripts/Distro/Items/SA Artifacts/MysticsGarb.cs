using System;
using Server;

namespace Server.Items
{
	public class MysticsGarb : GargishRobe
	{
		public override int LabelNumber { get { return 1113649; } } // Mystic's Garb

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public MysticsGarb()
		{
			Hue = 1250;

			Attributes.BonusMana = 5;
			Attributes.LowerManaCost = 1;
		}

		public MysticsGarb( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}