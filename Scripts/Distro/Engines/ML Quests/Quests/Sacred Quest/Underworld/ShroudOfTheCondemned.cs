using System;
using Server;

namespace Server.Items
{
	public class ShroudOfTheCondemned : Robe
	{
		public override int LabelNumber { get { return 1113703; } } // Shroud of the Condemned

		[Constructable]
		public ShroudOfTheCondemned()
		{
			Hue = 0x81B;
			Attributes.BonusInt = 5;
			Attributes.BonusHits = 3;
		}

		public ShroudOfTheCondemned( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}