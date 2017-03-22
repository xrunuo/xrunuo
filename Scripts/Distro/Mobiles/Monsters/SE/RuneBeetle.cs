using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a rune beetle corpse" )]
	public class RuneBeetle : BaseCreature
	{
		private static int m_MinTime = 15;
		private static int m_MaxTime = 30;

		private DateTime m_NextAbilityTime;

		private ExpireTimer m_Timer;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public RuneBeetle()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a rune beetle";
			Body = 244;

			SetStr( 400, 465 );
			SetDex( 125, 170 );
			SetInt( 375, 450 );

			SetHits( 310, 360 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 10 );
			SetDamageType( ResistanceType.Energy, 70 );

			SetResistance( ResistanceType.Physical, 40, 65 );
			SetResistance( ResistanceType.Fire, 35, 50 );
			SetResistance( ResistanceType.Cold, 35, 50 );
			SetResistance( ResistanceType.Poison, 75, 95 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.EvalInt, 100.1, 125.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.Meditation, 95.1, 110.0 );
			SetSkill( SkillName.Poisoning, 120.1, 140.0 );
			SetSkill( SkillName.MagicResist, 95.1, 110.0 );
			SetSkill( SkillName.Tactics, 80.1, 95.0 );
			SetSkill( SkillName.Wrestling, 70.1, 80.0 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 93.9;

			PackGold( 1000, 1200 );
			PackItem( new Bone( Utility.RandomMinMax( 2, 3 ) ) );
			PackMagicItems( 1, 5 );
			PackItem( BaseLootHelperSE.RandomBodyPart() );
			PackReg( 5, 14 );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x4E9;
		}

		public override int GetIdleSound()
		{
			return 0x4E8;
		}

		public override int GetAttackSound()
		{
			return 0x4E7;
		}

		public override int GetHurtSound()
		{
			return 0x4EA;
		}

		public override int GetDeathSound()
		{
			return 0x4E6;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.MedScrolls, 1 );
		}

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Greater; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

		public RuneBeetle( Serial serial )
			: base( serial )
		{
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				if ( BaseAttackHelperSE.IsUnderEffect( defender, BaseAttackHelperSE.m_RageTable ) ) return;

				BaseAttackHelperSE.CorruptArmorAttack( this, ref m_Timer, defender );

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}
