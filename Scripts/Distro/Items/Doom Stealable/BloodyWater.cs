using System;

namespace Server.Items
{
	public class BloodyWater : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public BloodyWater()
			: base( 0xE23 )
		{
		}

		public BloodyWater( Serial serial )
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
