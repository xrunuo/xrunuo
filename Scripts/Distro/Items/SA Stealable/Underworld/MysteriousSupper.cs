using System;

namespace Server.Items
{
	public class MysteriousSupper : StealableArtifact
	{
		public override int ArtifactRarity { get { return 3; } }

		public override int LabelNumber { get { return 1113663; } } // mysterious supper

		[Constructable]
		public MysteriousSupper()
			: base( 0x09AF )
		{
			Weight = 10.0;
		}

		public MysteriousSupper( Serial serial )
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
