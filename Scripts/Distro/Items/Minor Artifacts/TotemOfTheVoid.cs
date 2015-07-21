using System;
using Server;

namespace Server.Items
{
	public class TotemOfTheVoid : BaseTalisman
	{
		public override int LabelNumber { get { return 1075035; } } // Totem Of The Void

		[Constructable]
		public TotemOfTheVoid()
			: base( 0x2F5B )
		{
			Weight = 1.0;
			Hue = 0x2D0;
			Attributes.LowerManaCost = 10;
			Attributes.RegenHits = 2;
			ProtectionTalis = ProtectionKillerEntry.GetRandom();
			ProtectionValue = 1 + Utility.Random( 59 );
			TalismanType = TalismanType.SummonRandom;
			Charges = -1;
		}

		public TotemOfTheVoid( Serial serial )
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
