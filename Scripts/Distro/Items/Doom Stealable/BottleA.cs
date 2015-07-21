using System;

namespace Server.Items
{
	public class BottleA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 1; } }

		[Constructable]
		public BottleA()
			: base( 0xE28 )
		{
		}

		public BottleA( Serial serial )
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
