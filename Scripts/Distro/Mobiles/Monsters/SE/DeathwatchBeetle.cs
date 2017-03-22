using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a deathwatch beetle corpse" )]
	public class DeathWatchBeetle : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 9; }
		}

		private static int m_MinTime = 9;
		private static int m_MaxTime = 18;

		private DateTime m_NextAbilityTime;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.CrushingBlow;
		}

		[Constructable]
		public DeathWatchBeetle()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a deathwatch beetle";
			Body = 242;

			SetStr( 135, 160 );
			SetDex( 40, 55 );
			SetInt( 30, 40 );

			SetHits( 120, 130 );
			SetMana( 20 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 15, 30 );
			SetResistance( ResistanceType.Cold, 15, 30 );
			SetResistance( ResistanceType.Poison, 50, 80 );
			SetResistance( ResistanceType.Energy, 20, 35 );

			SetSkill( SkillName.Anatomy, 30.1, 35.0 );
			SetSkill( SkillName.MagicResist, 50.1, 60.0 );
			SetSkill( SkillName.Tactics, 65.1, 80.0 );
			SetSkill( SkillName.Wrestling, 50.1, 60.0 );

			Fame = 300;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 41.1;

			PackItem( Loot.RandomArmor() );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x4F4;
		}

		public override int GetIdleSound()
		{
			return 0x4F3;
		}

		public override int GetAttackSound()
		{
			return 0x4F2;
		}

		public override int GetHurtSound()
		{
			return 0x4F5;
		}

		public override int GetDeathSound()
		{
			return 0x4F1;
		}

		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Regular; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

		public DeathWatchBeetle( Serial serial )
			: base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Potions );
			AddLoot( LootPack.LowScrolls );
		}

		public override void OnThink()
		{
			if ( !BardPacified )
			{
				if ( DateTime.UtcNow >= m_NextAbilityTime )
				{
					PoisonSpitSE spit = new PoisonSpitSE( this );

					spit.ThrowIt();

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