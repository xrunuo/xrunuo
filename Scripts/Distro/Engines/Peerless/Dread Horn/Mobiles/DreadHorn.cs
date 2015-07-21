using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	public class DreadHorn : BaseCreature
	{
		private Timer m_Timer;

		[Constructable]
		public DreadHorn()
			: base( AIType.AI_Arcanist, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			Name = "Dread Horn";
			Body = 257;
			BaseSoundID = 0x4BC;

			SetStr( 1281, 1305 );
			SetDex( 600, 900 );
			SetInt( 1226, 1300 );

			SetHits( 30000 );

			SetDamage( 22, 28 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Poison, 60 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 55, 70 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 110.0, 110.0 );
			SetSkill( SkillName.Poisoning, 120.0, 120.0 );
			SetSkill( SkillName.Magery, 110.0, 110.0 );
			SetSkill( SkillName.MagicResist, 110.0, 110.0 );
			SetSkill( SkillName.Tactics, 90.0, 100.0 );
			SetSkill( SkillName.Anatomy, 50.0, 60.0 );
			SetSkill( SkillName.Wrestling, 90.0, 100.0 );
			SetSkill( SkillName.Meditation, 110.0, 110.0 );
			SetSkill( SkillName.Spellweaving, 180.0, 200.0 );

			Fame = 25000;
			Karma = -25000;

			m_Timer = new StatsTimer( this );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.PeerlessIngredients, 8 );
			AddLoot( LootPack.Talismans, Utility.RandomMinMax( 1, 5 ) );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 1500 > Utility.Random( 100000 ) )
				c.DropItem( new CrimsonCincture() );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new DreadsRevenge() );

			if ( 20000 > Utility.Random( 100000 ) )
				c.DropItem( new DreadHornTaintedMushroom() );
			if ( 20000 > Utility.Random( 100000 ) )
				c.DropItem( new HornOfDreadHorn() );
			if ( 20000 > Utility.Random( 100000 ) )
				c.DropItem( new MangledHeadOfDreadHorn() );
			if ( 20000 > Utility.Random( 100000 ) )
				c.DropItem( new PristineDreadHorn() );
			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new DreadFlute() );
			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new HumanFeyLeggings() );

			c.DropItem( new DreadHornMane() );

			for ( int i = 0; i < 2; i++ )
			{
				if ( 5000 > Utility.Random( 100000 ) )
					c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}
		}

		public override bool AutoDispel { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public DreadHorn( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Timer = new StatsTimer( this );
			m_Timer.Start();
		}

		private class StatsTimer : Timer
		{
			private static Hashtable m_UnderEffect = new Hashtable();

			public static void RemoveEffect( object state )
			{
				Mobile m = (Mobile) state;

				m_UnderEffect.Remove( m );
				m.UpdateResistances();
			}

			public static bool UnderEffect( Mobile m )
			{
				return m_UnderEffect.Contains( m );
			}

			private Mobile m_Owner;


			public StatsTimer( Mobile owner )
				: base( TimeSpan.FromSeconds( 10.0 ), TimeSpan.FromSeconds( 10.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.2 > Utility.RandomDouble() )
					return;

				Mobile toCurse = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 6 ) )
				{
					if ( m != m_Owner && m.IsPlayer && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) && m.Region == m_Owner.Region )
					{
						toCurse = m;
						break;
					}
				}

				if ( toCurse != null && !UnderEffect( toCurse ) )
				{
					Timer t = (Timer) m_UnderEffect[toCurse];
					m_UnderEffect[toCurse] = t = Timer.DelayCall( TimeSpan.FromSeconds( 60.0 ), new TimerStateCallback( RemoveEffect ), toCurse );

					SpellHelper.AddStatCurse( m_Owner, toCurse, StatType.Str, 20, TimeSpan.FromSeconds( 50.0 ) );
					SpellHelper.AddStatCurse( m_Owner, toCurse, StatType.Dex, 20, TimeSpan.FromSeconds( 50.0 ) );
					SpellHelper.AddStatCurse( m_Owner, toCurse, StatType.Int, 20, TimeSpan.FromSeconds( 50.0 ) );

					toCurse.SendLocalizedMessage( 1072067 ); // A wave of hopelessness washes over you, suppressing your ability to react in combat.
				}
			}
		}
	}
}
