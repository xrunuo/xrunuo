using System;
using Server;

namespace Server.Items
{
	public class MonksRobe : Item
	{
		public override int LabelNumber { get { return 1076584; } } // A Monk's Robe

		[Constructable]
		public MonksRobe()
			: base( 0x2684 )
		{
			Weight = 1.0;
			Layer = Layer.OuterTorso;
			Hue = 542;
		}

		public MonksRobe( Serial serial )
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