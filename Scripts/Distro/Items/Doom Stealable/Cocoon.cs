using System;

namespace Server.Items
{
	public class Cocoon : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		[Constructable]
		public Cocoon()
			: base( 0x10DA )
		{
		}

		public Cocoon( Serial serial )
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
