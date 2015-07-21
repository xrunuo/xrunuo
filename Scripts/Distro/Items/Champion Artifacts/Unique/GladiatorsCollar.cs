using System;
using Server;

namespace Server.Items
{
	public class GladiatorsCollar : PlateGorget
	{
		public override int LabelNumber { get { return 1094917; } } // Gladiator's Collar [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public GladiatorsCollar()
		{
			Hue = 0x967;

			Attributes.BonusHits = 10;
			Attributes.AttackChance = 10;

			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 13;
			Resistances.Fire = 15;
			Resistances.Cold = 15;
			Resistances.Poison = 15;
			Resistances.Energy = 14;
		}

		public GladiatorsCollar( Serial serial )
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
				Resistances.Physical = 13;
				Resistances.Fire = 15;
				Resistances.Cold = 15;
				Resistances.Poison = 15;
				Resistances.Energy = 14;
			}
		}
	}
}
