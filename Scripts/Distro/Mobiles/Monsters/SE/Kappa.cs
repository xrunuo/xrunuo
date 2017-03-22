using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a kappa corpse" )]
	public class Kappa : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 8; }
		}

		private static int m_MinTime = 5;
		private static int m_MaxTime = 10;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public Kappa()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kappa";
			Body = 240;

			SetStr( 195, 225 );
			SetDex( 50, 70 );
			SetInt( 40, 55 );

			SetHits( 150, 180 );
			SetMana( 30 );
			SetStam( 50, 70 );

			SetDamage( 6, 11 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 50 );
			SetResistance( ResistanceType.Fire, 35, 50 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 35, 50 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.MagicResist, 60.1, 70.0 );
			SetSkill( SkillName.Tactics, 80.1, 90.0 );
			SetSkill( SkillName.Wrestling, 60.1, 65.0 );

			Fame = 500;
			Karma = -500;

			Tamable = false;

			PackGold( 175, 225 );

			if ( Utility.RandomDouble() < .33 )
				PackItem( Engines.Plants.Seed.RandomPeculiarSeed( 4 ) );
		}

		public override int TreasureMapLevel { get { return 2; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override void OnThink()
		{
			if ( !BardPacified )
			{
				if ( DateTime.UtcNow >= m_NextAbilityTime )
				{
					Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, Kappa.AbilityRange );

					if ( target != null )
						BaseAttackHelperSE.LifeforceDrainAttack( this, target );

					m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
				}
			}

			base.OnThink();
		}

		public Kappa( Serial serial )
			: base( serial )
		{
		}

		public void IsHurtBad( bool willKill )
		{
			if ( Hits < 15 || willKill )
				BaseAttackHelperSE.SpillAcid( this, false );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( from != null )
				IsHurtBad( willKill );

			base.OnDamage( amount, from, willKill );
		}

		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster != null )
				IsHurtBad( false );

			base.OnDamagedBySpell( caster );
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