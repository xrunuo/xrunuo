using System;

namespace Server.Items
{
	public class Sake : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public Sake()
			: base( 0x24E2 )
		{
		}

		public Sake( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}