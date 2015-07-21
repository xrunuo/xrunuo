using System;
using Server;

namespace Server.Items
{
	public class DreadPirateHat : TricorneHat
	{
		public override int LabelNumber { get { return 1063467; } } // Dread Pirate Hat

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DreadPirateHat()
		{
			Hue = 0x497;

			Resistances.Cold = 5;
			Resistances.Poison = 5;

			SkillBonuses.SetValues( 0, Utility.RandomCombatSkill(), 10.0 );

			Attributes.BonusDex = 8;
			Attributes.AttackChance = 10;
			Attributes.NightSight = 1;
		}

		public DreadPirateHat( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 2 )
			{
				Resistances.Cold = 5;
				Resistances.Poison = 5;
			}

			if ( version < 1 )
			{
				Attributes.Luck = 0;
				Attributes.AttackChance = 10;
				Attributes.NightSight = 1;
				SkillBonuses.SetValues( 0, Utility.RandomCombatSkill(), 10.0 );
				SkillBonuses.SetBonus( 1, 0 );
			}
		}
	}
}
