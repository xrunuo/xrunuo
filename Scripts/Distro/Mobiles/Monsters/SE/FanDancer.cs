using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.Plants;

namespace Server.Mobiles
{
	[CorpseName( "a fan dancer corpse" )]
	public class FanDancer : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 10; }
		}

		private static int m_MinTime = 6;
		private static int m_MaxTime = 12;

		private DateTime m_NextAbilityTime;

		private ExpireTimer m_Timer;

		[Constructable]
		public FanDancer()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a fan dancer";
			Body = 247;

			SetStr( 315, 375 );
			SetDex( 200, 255 );
			SetInt( 20, 25 );

			SetHits( 350, 420 );

			SetDamage( 7, 13 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 70 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Anatomy, 85.1, 95.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.0 );

			Fame = 300;
			Karma = -300;

			Tamable = false;

			PackItem( new Tessen() );

			AddItem( Seed.RandomBonsaiSeed() );

			if ( 0.01 > Utility.RandomDouble() )
				AddItem( new OrigamiPaper() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 2 );
			AddLoot( LootPack.Rich, 1 );
		}
		
		public override int TreasureMapLevel { get { return 3; } }		

		public void LowerFireResist( Mobile target )
		{
			if ( BaseAttackHelperSE.IsUnderEffect( target, BaseAttackHelperSE.m_FanDancerRMT ) ) return;

			TimeSpan duration = TimeSpan.FromSeconds( 121 - (int) ( target.Skills[SkillName.MagicResist].Value ) );

			ResistanceMod[] mod = new ResistanceMod[1]
			{
				new ResistanceMod( ResistanceType.Fire, -25 )
			};

			target.SendLocalizedMessage( 1070833 ); // The creature fans you with fire, reducing your resistance to fire attacks.

			BaseAttackHelperSE.LowerResistanceAttack( this, ref m_Timer, duration, target, mod, BaseAttackHelperSE.m_FanDancerRMT );
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				if ( Utility.RandomBool() )
				{
					ThrowingTessenSE tessen = new ThrowingTessenSE( this );

					tessen.ThrowIt();
				}
				else
				{
					Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, FanDancer.AbilityRange );

					if ( target != null ) LowerFireResist( target );
				}

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}

			base.OnThink();
		}

		public override bool BardImmune { get { return true; } }

		public FanDancer( Serial serial )
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
