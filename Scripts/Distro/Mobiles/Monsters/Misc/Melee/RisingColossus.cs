using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class RisingColossus : BaseCreature
	{
		public override double DispelDifficulty { get { return m_DispelDifficulty; } }
		public override double DispelFocus { get { return 45.0; } }

		private int m_DispelDifficulty;

		[Constructable]
		public RisingColossus( int level )
			: base( AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.25, 0.5 )
		{
			int bonus1 = ( ( level * 3 ) / 4 );
			int bonus2 = ( ( level * 9 ) / 8 );
			int bonus3 = level / 2;

			Name = "a rising colossus";
			Body = 829;

			SetHits( 200 + bonus2 );

			SetStr( 600 + bonus1 );
			SetDex( 30 + bonus1 );
			SetInt( 50 + bonus1 );

			SetDamage( level / 12, level / 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 55, 60 );
			SetResistance( ResistanceType.Cold, 55, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 65, 70 );

			SetSkill( SkillName.MagicResist, bonus3 );
			SetSkill( SkillName.Tactics, bonus3 );
			SetSkill( SkillName.Wrestling, bonus3 );
			SetSkill( SkillName.Anatomy, bonus3 );
			SetSkill( SkillName.Mysticism, bonus3 );

			ControlSlots = 5;

			m_DispelDifficulty = 40 + ( level * 2 );
		}

		public override int GetAttackSound()
		{
			return 0x627;
		}

		public override int GetHurtSound()
		{
			return 0x629;
		}

		public override bool BleedImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public RisingColossus( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( (int) m_DispelDifficulty );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				m_DispelDifficulty = reader.ReadInt();
		}
	}
}