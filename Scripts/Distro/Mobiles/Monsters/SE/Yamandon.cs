using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "an yamandon corpse" )]
	public class Yamandon : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 10; }
		}

		private static int m_MinTime = 10;
		private static int m_MaxTime = 20;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public Yamandon()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a yamandon";
			Body = 249;

			SetStr( 785, 930 );
			SetDex( 250, 365 );
			SetInt( 100, 120 );

			SetHits( 1600, 1800 );

			SetDamage( 19, 31 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 10 );

			SetResistance( ResistanceType.Physical, 65, 85 );
			SetResistance( ResistanceType.Fire, 70, 90 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Anatomy, 115.1, 130.0 );
			SetSkill( SkillName.MagicResist, 115.1, 135.0 );
			SetSkill( SkillName.Poisoning, 120.1, 140.0 );
			SetSkill( SkillName.Tactics, 115.1, 135.0 );
			SetSkill( SkillName.Wrestling, 110.1, 135.0 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			Tamable = false;

			PackGold( 200, 500 );

			AddItem( Seed.RandomBonsaiSeed() );
		}

		public override int GetAngerSound()
		{
			return 0x4EF;
		}

		public override int GetIdleSound()
		{
			return 0x4EE;
		}

		public override int GetAttackSound()
		{
			return 0x4ED;
		}

		public override int GetHurtSound()
		{
			return 0x4F0;
		}

		public override int GetDeathSound()
		{
			return 0x4EB;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.Gems, 6 );
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Regular; } }

		public Yamandon( Serial serial )
			: base( serial )
		{
		}

		public override void OnThink()
		{
			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, Yamandon.AbilityRange );

				if ( target != null )
					BaseAttackHelperSE.SpillAcid( target, true );

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
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