using System;

namespace Server.Items
{
	public class Cups : StealableArtifact
	{
		public override int ArtifactRarity { get { return 4; } }

		[Constructable]
		public Cups()
			: base( 0x24E1 )
		{
		}

		public Cups( Serial serial )
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
