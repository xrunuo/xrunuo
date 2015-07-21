using System;

namespace Server.Items
{
	public class PricelessTreasure : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		public override int LabelNumber { get { return 1113680; } } // priceless treasure

		[Constructable]
		public PricelessTreasure()
			: base( 0x1B54 )
		{
			Weight = 10.0;
		}

		public PricelessTreasure( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
