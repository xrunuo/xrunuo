using System;

namespace Server.Items
{
	public class FallenBook : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		[Constructable]
		public FallenBook()
			: base( 0x1E21 )
		{
		}

		public FallenBook( Serial serial )
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
