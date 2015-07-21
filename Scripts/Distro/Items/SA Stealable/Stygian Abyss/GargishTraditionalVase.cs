using System;

namespace Server.Items
{
	public class GargishTraditionalVase : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1095946; } } // Gargish Traditional Vase

		[Constructable]
		public GargishTraditionalVase()
			: base( 0x42B2 )
		{
			Weight = 10.0;
		}

		public GargishTraditionalVase( Serial serial )
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
