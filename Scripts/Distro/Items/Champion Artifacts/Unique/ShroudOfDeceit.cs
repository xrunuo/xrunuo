using System;
using Server;

namespace Server.Items
{
	public class ShroudOfDeciet : BoneChest
	{
		public override int LabelNumber { get { return 1094914; } } // Shroud of Deceit [Replica]

		public override int InitMinHits { get { return 150; } }
		public override int InitMaxHits { get { return 150; } }

		public override bool CanFortify { get { return false; } }

		[Constructable]
		public ShroudOfDeciet()
		{
			Hue = 0x3D5;

			Attributes.RegenHits = 3;

			ArmorAttributes.MageArmor = 1;

			SkillBonuses.Skill_1_Name = SkillName.MagicResist;
			SkillBonuses.Skill_1_Value = 10;

			Resistances.Physical = 8;
			Resistances.Fire = 3;
			Resistances.Cold = 14;
			Resistances.Poison = 13;
			Resistances.Energy = 9;
		}

		public ShroudOfDeciet( Serial serial )
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
				Resistances.Physical = 8;
				Resistances.Fire = 3;
				Resistances.Cold = 14;
				Resistances.Poison = 13;
				Resistances.Energy = 9;
			}
		}
	}
}
