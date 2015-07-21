using System;

namespace Server.Items
{
	public class DyingPlant : StealableArtifact
	{
		public override int ArtifactRarity { get { return 5; } }

		public override int LabelNumber { get { return 1095954; } } // dying plant

		[Constructable]
		public DyingPlant()
			: base( 0x42BA )
		{
			Weight = 10.0;
		}

		public DyingPlant( Serial serial )
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
