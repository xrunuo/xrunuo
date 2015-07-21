using System;

namespace Server.Items
{
	public class SkinnedDeer : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		[Constructable]
		public SkinnedDeer()
			: base( 0x1E91 )
		{
		}

		public SkinnedDeer( Serial serial )
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
