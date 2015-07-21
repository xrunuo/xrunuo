using System;

namespace Server.Items
{
	public class Rock : StealableArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public Rock()
			: base( 0x1363 )
		{
		}

		public Rock( Serial serial )
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
