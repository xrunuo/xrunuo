using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a kaze kemono corpse" )]
	public class KazeKemono : BaseCreature
	{
		public static int AbilityRange
		{
			get { return 12; }
		}

		private static int m_MinTime = 10;
		private static int m_MaxTime = 20;

		private DateTime m_NextAbilityTime;

		private ExpireTimer m_Timer;

		[Constructable]
		public KazeKemono()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kaze kemono";
			Body = 196;
			BaseSoundID = 263;

			SetStr( 205, 275 );
			SetDex( 100, 155 );
			SetInt( 100, 110 );

			SetHits( 250, 310 );
			SetMana( 100, 110 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Fire, 10 );
			SetDamageType( ResistanceType.Cold, 10 );
			SetDamageType( ResistanceType.Poison, 10 );

			SetResistance( ResistanceType.Physical, 50, 70 );
			SetResistance( ResistanceType.Fire, 30, 60 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 60, 80 );

			SetSkill( SkillName.MagicResist, 110.1, 120.0 );
			SetSkill( SkillName.Tactics, 55.1, 65.0 );
			SetSkill( SkillName.Wrestling, 85.1, 95.1 );
			SetSkill( SkillName.Anatomy, 25.1, 35.0 );
			SetSkill( SkillName.Magery, 95.1, 105.0 );

			Fame = 4500;
			Karma = -4500;

			Tamable = false;

			PackGold( 550, 650 );
			PackMagicItems( 1, 5 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 2 );
		}

		public KazeKemono( Serial serial )
			: base( serial )
		{
		}

		public void LowerResist( Mobile target )
		{
			if ( BaseAttackHelperSE.IsUnderEffect( target, BaseAttackHelperSE.m_KazeKemonoRMT ) ) return;

			TimeSpan duration = TimeSpan.FromSeconds( 121 - (int) ( target.Skills[SkillName.MagicResist].Value ) );

			ResistanceMod[] mod = new ResistanceMod[1]
			{
				new ResistanceMod( ResistanceType.Physical, -25 )
			};

			BaseAttackHelperSE.LowerResistanceAttack( this, ref m_Timer, duration, target, mod, BaseAttackHelperSE.m_KazeKemonoRMT );
		}

		public override void OnThink()
		{
			if ( DateTime.Now >= m_NextAbilityTime )
			{
				Mobile target = BaseAttackHelperSE.GetRandomAttacker( this, KazeKemono.AbilityRange );

				if ( target != null ) LowerResist( target );

				m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
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