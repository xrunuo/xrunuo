using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a hiryu corpse" )]
	public class Hiryu : BaseMount
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		public override bool StatLossAfterTame { get { return true; } }

		private static int m_MinTime = 4;
		private static int m_MaxTime = 8;

		private DateTime m_NextAbilityTime;

		private ExpireTimer m_Timer;

		[Constructable]
		public Hiryu()
			: this( "a hiryu" )
		{
		}

		[Constructable]
		public Hiryu( string name )
			: base( name, 243, 0x3E94, AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = Utility.RandomList( 0x0, 0x8032, 0x8037, 0x803C, 0x8294, 0x847F, 0x8487, 0x848D, 0x8490, 0x8495, 0x88A0, 0x889F );

			SetStr( 1200, 1400 );
			SetDex( 170, 270 );
			SetInt( 300, 325 );

			SetHits( 900, 1100 );
			SetMana( 60, 65 );
			SetStam( 170, 270 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 70 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 75.0, 80.0 );
			SetSkill( SkillName.MagicResist, 85.0, 100.0 );
			SetSkill( SkillName.Tactics, 100.0, 110.0 );
			SetSkill( SkillName.Wrestling, 100.0, 120.0 );

			this.Skills[SkillName.Wrestling].Cap = 120;
			this.Skills[SkillName.Tactics].Cap = 110;

			Fame = 25000;
			Karma = -25000;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 98.7;

			PackGold( 100, 150 );

			AddItem( Seed.RandomBonsaiSeed() );

			if ( Utility.RandomDouble() < .33 )
				PackItem( Engines.Plants.Seed.RandomPeculiarSeed( 3 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
			AddLoot( LootPack.Gems, Utility.Random( 1, 5 ) );
		}

		public override int GetAngerSound()
		{
			return 0x4FF;
		}

		public override int GetIdleSound()
		{
			return 0x4FE;
		}

		public override int GetAttackSound()
		{
			return 0x4FD;
		}

		public override int GetHurtSound()
		{
			return 0x500;
		}

		public override int GetDeathSound()
		{
			return 0x4FC;
		}

		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 16; } }
		public override int Hides { get { return 60; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }

		public Hiryu( Serial serial )
			: base( serial )
		{
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime && 0.1 > Utility.RandomDouble() )
			{
				BaseAttackHelperSE.HiryuAbilitiesAttack( this, ref m_Timer );

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( FightMode == FightMode.Aggressor )
				FightMode = FightMode.Closest;

			this.Skills[SkillName.Wrestling].Cap = 120;

			if ( version < 1 )
				SetStam( 170, 270 );
		}
	}
}