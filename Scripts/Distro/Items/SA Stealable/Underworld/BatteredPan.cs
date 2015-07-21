using System;

namespace Server.Items
{
	public class BatteredPan : StealableArtifact
	{
		public override int ArtifactRarity { get { return 6; } }

		public override int LabelNumber { get { return 1113669; } } // Battered Pan

		[Constructable]
		public BatteredPan()
			: base( 0x09DE )
		{
			Weight = 10.0;
		}

		public BatteredPan( Serial serial )
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
