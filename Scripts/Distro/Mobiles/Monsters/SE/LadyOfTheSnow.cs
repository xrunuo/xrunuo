using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a lady of the snow corpse" )]
	public class LadyOfTheSnow : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 16; }
		}

		private static double m_HardSnowBallChance = 0.8;

		private static int m_MinTime = 12;
		private static int m_MaxTime = 16;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public LadyOfTheSnow()
			: base( AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a lady of the snow";
			Body = 252;

			SetStr( 275, 305 );
			SetDex( 105, 125 );
			SetInt( 470, 490 );

			SetHits( 595, 625 );
			SetMana( 470, 490 );

			SetDamage( 13, 19 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 40, 55 );
			SetResistance( ResistanceType.Cold, 70, 90 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 70, 85 );

			SetSkill( SkillName.Magery, 95.1, 110.0 );
			SetSkill( SkillName.Meditation, 85.1, 90.0 );
			SetSkill( SkillName.MagicResist, 90.1, 105.0 );
			SetSkill( SkillName.Tactics, 85.1, 100.0 );
			SetSkill( SkillName.Wrestling, 85.1, 100.0 );
			SetSkill( SkillName.Necromancy, 95.1, 110.0 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			Tamable = false;

			PackGold( 600, 800 );

			PackReg( 3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
		}

		public override bool CanRummageCorpses { get { return true; } }

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, LadyOfTheSnow.AbilityRange );

				if ( target != null ) BaseAttackHelperSE.IcyWindAttack( this, target );

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public LadyOfTheSnow( Serial serial )
			: base( serial )
		{
		}

		public void SnowBallAttack( Mobile from, Mobile target )
		{
			ThrowingSnowballSE snowball = new ThrowingSnowballSE( this );

			snowball.ThrowIt();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( from != null && !willKill && this.InRange( from, LadyOfTheSnow.AbilityRange ) )
			{
				if ( m_HardSnowBallChance > Utility.RandomDouble() )
				{
					Item item = from.FindItemOnLayer( Layer.TwoHanded );

					if ( item is BaseRanged )
					{
						SnowBallAttack( this, from );
					}
				}
			}

			base.OnDamage( amount, from, willKill );
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