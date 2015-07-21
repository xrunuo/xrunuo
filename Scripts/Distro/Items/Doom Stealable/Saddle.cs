using System;

namespace Server.Items
{
	public class Saddle : StealableArtifact
	{
		public override int ArtifactRarity { get { return 9; } }

		[Constructable]
		public Saddle()
			: base( 0xF38 )
		{
		}

		public Saddle( Serial serial )
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
