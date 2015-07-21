using System;

namespace Server.Items
{
	public class RuinedPaintingA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 12; } }

		[Constructable]
		public RuinedPaintingA()
			: base( 0xC2C )
		{
		}

		public RuinedPaintingA( Serial serial )
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
