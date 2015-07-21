using System;

namespace Server.Items
{
	public class PushmePullyu : StealableArtifact
	{
		public override int ArtifactRarity { get { return 8; } }

		public override int LabelNumber { get { return 1095445; } } // Pushme Pullyu

		[Constructable]
		public PushmePullyu()
			: base( 0x40BD )
		{
			Weight = 10.0;
		}

		public PushmePullyu( Serial serial )
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
