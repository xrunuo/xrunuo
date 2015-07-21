using System;

namespace Server.Items
{
	public class BlockAndTackle : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		public override int LabelNumber { get { return 1113660; } } // Block And Tackle

		[Constructable]
		public BlockAndTackle()
			: base( 0x1E9A )
		{
			Weight = 10.0;
		}

		public BlockAndTackle( Serial serial )
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
