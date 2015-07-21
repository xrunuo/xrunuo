using System;

namespace Server.Items
{
	public class RottedOars : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		public override int LabelNumber { get { return 1113665; } } // Rotted Oars

		[Constructable]
		public RottedOars()
			: base( 0x1E2B )
		{
			Weight = 10.0;
		}

		public RottedOars( Serial serial )
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
