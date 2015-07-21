using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a Yomotsu corpse" )]
	public class YomotsuWarrior : BaseCreature
	{
		private static int m_MinTime = 5;
		private static int m_MaxTime = 10;

		private DateTime m_NextAbilityTime;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ParalyzingBlow;
		}

		[Constructable]
		public YomotsuWarrior()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an yomotsu warrior";
			Body = 245;

			SetStr( 480, 530 );
			SetDex( 150, 165 );
			SetInt( 15, 35 );

			SetHits( 480, 530 );

			SetDamage( 13, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 85 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 40, 55 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 80.1, 90.0 );
			SetSkill( SkillName.Tactics, 95.1, 105.0 );
			SetSkill( SkillName.Wrestling, 98.1, 108.0 );

			Fame = 5000;
			Karma = -5000;

			Tamable = false;

			PackGold( 600, 700 );
			PackItem( new ExecutionersAxe() );
			PackItem( BaseLootHelperSE.RandomFootWears() );
			PackItem( new ShortPants() );
			PackMagicItems( 1, 5 );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x453;
		}

		public override int GetIdleSound()
		{
			return 0x452;
		}

		public override int GetAttackSound()
		{
			return 0x454;
		}

		public override int GetHurtSound()
		{
			return 0x455;
		}

		public override int GetDeathSound()
		{
			return 0x456;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 2 );
		}

		public override void OnThink()
		{
			if ( !BardPacified )
			{
				if ( DateTime.Now >= m_NextAbilityTime )
				{
					ThrowingDaggerSE dagger = new ThrowingDaggerSE( this );

					dagger.ThrowIt();

					m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
				}
			}

			base.OnThink();
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 1; } }

		public YomotsuWarrior( Serial serial )
			: base( serial )
		{
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
