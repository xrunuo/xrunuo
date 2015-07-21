using System;
using Server;

namespace Server.Items
{
	public class CrimsonCincture : HalfApron
	{
		public override int LabelNumber { get { return 1075043; } } // Crimson Cincture

		[Constructable]
		public CrimsonCincture()
			: base( 0xE8 )
		{
			Weight = 1.0;

			Attributes.BonusDex = 5;
			Attributes.BonusHits = 10;
			Attributes.RegenHits = 2;
		}

		public CrimsonCincture( Serial serial )
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