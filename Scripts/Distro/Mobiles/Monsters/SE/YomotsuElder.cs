using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a Wrinkly Yomotsu corpse" )]
	public class YomotsuElder : BaseCreature
	{
		private static int m_MinTime = 5;
		private static int m_MaxTime = 10;

		private DateTime m_NextAbilityTime;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.ParalyzingBlow;
		}

		[Constructable]
		public YomotsuElder()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an yomotsu elder";
			Body = 255;

			SetStr( 695, 825 );
			SetDex( 250, 365 );
			SetInt( 15, 40 );

			SetHits( 820, 895 );
			SetMana( 15, 40 );

			SetDamage( 13, 14 );

			SetResistance( ResistanceType.Physical, 65, 85 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 45, 65 );
			SetResistance( ResistanceType.Poison, 35, 55 );
			SetResistance( ResistanceType.Energy, 25, 50 );

			SetSkill( SkillName.Anatomy, 115.1, 130.0 );
			SetSkill( SkillName.MagicResist, 100.1, 115.0 );
			SetSkill( SkillName.Tactics, 115.1, 130.0 );
			SetSkill( SkillName.Wrestling, 110.1, 130.0 );

			Fame = 10000;
			Karma = -10000;

			Tamable = false;

			PackGold( 1500, 1800 );
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
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				ThrowingDaggerSE dagger = new ThrowingDaggerSE( this );

				dagger.ThrowIt();

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public override bool CanRummageCorpses { get { return true; } }

		public YomotsuElder( Serial serial )
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
