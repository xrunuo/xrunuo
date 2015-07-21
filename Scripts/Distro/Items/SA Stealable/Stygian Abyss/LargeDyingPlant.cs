using System;

namespace Server.Items
{
	public class LargeDyingPlant : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1095953; } } // Large Dying Plant

		[Constructable]
		public LargeDyingPlant()
			: base( 0x42B9 )
		{
			Weight = 10.0;
		}

		public LargeDyingPlant( Serial serial )
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
