using System;
using Server;

namespace Server.Items
{
	public class CaptainJohnsHat : TricorneHat
	{
		public override int LabelNumber { get { return 1094911; } } // Captain John's Hat [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public CaptainJohnsHat()
		{
			Hue = 0x1;

			Attributes.BonusDex = 8;
			Attributes.NightSight = 1;
			Attributes.AttackChance = 15;

			SkillBonuses.Skill_1_Name = SkillName.Swords;
			SkillBonuses.Skill_1_Value = 20;

			Resistances.Physical = 2;
			Resistances.Fire = 1;
			Resistances.Poison = 2;
			Resistances.Energy = 18;
		}

		public CaptainJohnsHat( Serial serial )
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
				Resistances.Physical = 2;
				Resistances.Fire = 1;
				Resistances.Poison = 2;
				Resistances.Energy = 18;
			}
		}
	}
}
