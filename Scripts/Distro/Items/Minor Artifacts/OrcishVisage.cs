using System;
using Server;

namespace Server.Items
{
	public class OrcishVisage : OrcHelm
	{
		public override int LabelNumber { get { return 1070691; } } // Orcish Visage

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public OrcishVisage()
		{
			Hue = 0x592;
			ArmorAttributes.SelfRepair = 3;
			Attributes.BonusStr = 10;
			Attributes.BonusStam = 5;

			Resistances.Physical = 5;
			Resistances.Fire = 4;
		}

		public OrcishVisage( Serial serial )
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
				Resistances.Fire = 4;
			}
		}
	}
}