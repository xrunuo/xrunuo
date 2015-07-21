using System;
using Server;

namespace Server.Items
{
	public class PiecesOfCrystal : TransientItem
	{
		public override int LabelNumber { get { return 1074263; } } // crushed crystal pieces

		[Constructable]
		public PiecesOfCrystal()
			: base( 0x2245, TimeSpan.FromSeconds( 21600.0 ) )
		{
			LootType = LootType.Blessed;
			Weight = 1;
			Hue = 0x2B2;
		}

		public PiecesOfCrystal( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}

