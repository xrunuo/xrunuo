using System;

namespace Server.Items
{
	public class BrazierA : StealableArtifact
	{
		public override int ArtifactRarity { get { return 2; } }

		[Constructable]
		public BrazierA()
			: base( 0xE31 )
		{
			Light = LightType.Circle150;
		}

		public BrazierA( Serial serial )
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
