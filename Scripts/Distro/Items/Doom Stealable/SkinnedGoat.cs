using System;

namespace Server.Items
{
	public class SkinnedGoat : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public SkinnedGoat()
			: base( 0x1E88 )
		{
		}

		public SkinnedGoat( Serial serial )
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
