using System;
using Server;

namespace Server.Items
{
	public class AcidProofRobe : Robe
	{
		public override int LabelNumber { get { return 1095236; } } // Acid-Proof Robe [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public AcidProofRobe()
		{
			Hue = 0x0455;
			LootType = LootType.Blessed;

			Resistances.Fire = 4;
		}

		public AcidProofRobe( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				Resistances.Fire = 4;
		}
	}
}
