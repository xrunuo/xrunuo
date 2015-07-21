using System;
using Server;

namespace Server.Items
{
	public class LordBlackthornsExemplar : ChaosShield
	{
		public override int ArtifactRarity { get { return 11; } }

		public override int LabelNumber { get { return 1079793; } } // Lord Blackthorn's Exemplar

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public LordBlackthornsExemplar()
		{
			Hue = 1121;

			Resistances.Physical = 5;
			Resistances.Fire = 10;
			Resistances.Cold = 10;
			Resistances.Poison = 15;
			Resistances.Energy = 10;
		}

		public LordBlackthornsExemplar( Serial serial )
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
			{
				Resistances.Physical = 5;
				Resistances.Fire = 10;
				Resistances.Cold = 10;
				Resistances.Poison = 15;
				Resistances.Energy = 10;
			}
		}
	}
}