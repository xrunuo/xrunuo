using System;

namespace Server.Items
{
	public class RockA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public RockA()
			: base( 0x1368 )
		{
			Weight = 10.0;
		}

		public RockA( Serial serial )
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
