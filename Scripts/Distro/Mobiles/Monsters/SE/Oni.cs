using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "an oni corpse" )]
	public class Oni : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 10; }
		}

		private static int m_MinTime = 10;
		private static int m_MaxTime = 30;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public Oni()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an oni";

			Body = 241;

			SetStr( 800, 905 );
			SetDex( 150, 200 );
			SetInt( 170, 200 );

			SetHits( 400, 530 );

			SetDamage( 13, 20 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Fire, 10 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 45, 70 );
			SetResistance( ResistanceType.Energy, 45, 65 );

			SetSkill( SkillName.EvalInt, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 96.1, 106.0 );
			SetSkill( SkillName.Meditation, 27.5, 42.5 );
			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 85.1, 100.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.Wrestling, 90.1, 100.0 );

			Fame = 15000;
			Karma = -15000;

			Tamable = false;

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x4E4;
		}

		public override int GetIdleSound()
		{
			return 0x4E3;
		}

		public override int GetAttackSound()
		{
			return 0x4E2;
		}

		public override int GetHurtSound()
		{
			return 0x4E5;
		}

		public override int GetDeathSound()
		{
			return 0x4E1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override int TreasureMapLevel { get { return 4; } }

		public Oni( Serial serial )
			: base( serial )
		{
		}

		public override void OnThink()
		{
			if ( !BardPacified )
			{
				if ( DateTime.UtcNow >= m_NextAbilityTime )
				{
					Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, Oni.AbilityRange );

					if ( target != null )
						BaseAttackHelperSE.AngryFireAttack( this, target );

					m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
				}
			}

			base.OnThink();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}