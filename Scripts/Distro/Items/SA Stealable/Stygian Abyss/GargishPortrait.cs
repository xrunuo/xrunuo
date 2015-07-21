using System;

namespace Server.Items
{
	public class GargishPortrait : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		public override int LabelNumber { get { return 1095950; } } // Gargish Portrait

		[Constructable]
		public GargishPortrait()
			: base( 0x42B6 )
		{
			Weight = 10.0;
		}

		public GargishPortrait( Serial serial )
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
