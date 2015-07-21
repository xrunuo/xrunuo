using System;

namespace Server.Items
{
	public class BookofTruth : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1027187; } } // Book of Truth

		[Constructable]
		public BookofTruth()
			: base( 0x1C13 )
		{
			Weight = 10.0;
		}

		public BookofTruth( Serial serial )
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
