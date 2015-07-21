using System;

namespace Server.Items
{
	public class GruesomeStandard : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		[Constructable]
		public GruesomeStandard()
			: base( 0x428 )
		{
		}

		public GruesomeStandard( Serial serial )
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
