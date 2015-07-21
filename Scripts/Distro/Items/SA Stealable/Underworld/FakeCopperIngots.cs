using System;

namespace Server.Items
{
	public class FakeCopperIngots : StealableArtifact
	{
		public override int ArtifactRarity { get { return 7; } }

		public override int LabelNumber { get { return 1113679; } } // fake copper ingots

		[Constructable]
		public FakeCopperIngots()
			: base( 0x1BE5 )
		{
			Weight = 10.0;
		}

		public FakeCopperIngots( Serial serial )
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
