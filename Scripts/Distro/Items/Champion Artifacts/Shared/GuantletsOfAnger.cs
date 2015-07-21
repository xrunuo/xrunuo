using System;
using Server;

namespace Server.Items
{
	public class GuantletsOfAnger : PlateGloves
	{
		public override int LabelNumber { get { return 1094902; } } // Gauntlets of Anger [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public GuantletsOfAnger()
		{
			Hue = 0x557;

			Attributes.BonusHits = 8;
			Attributes.RegenHits = 2;
			Attributes.DefendChance = 10;

			Resistances.Physical = -1;
			Resistances.Fire = 1;
			Resistances.Cold = 3;
			Resistances.Poison = 3;
			Resistances.Energy = 3;
		}

		public GuantletsOfAnger( Serial serial )
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
				Resistances.Physical = -1;
				Resistances.Fire = 1;
				Resistances.Cold = 3;
				Resistances.Poison = 3;
				Resistances.Energy = 3;
			}
		}
	}
}
