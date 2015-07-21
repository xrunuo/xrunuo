using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Engines.BuffIcons;
using Server.Events;

namespace Server.Spells.Spellweaving
{
	public class GiftOfRenewalSpell : SpellweavingSpell
	{
		public static void Initialize()
		{
			EventSink.Instance.PlayerDeath += new PlayerDeathEventHandler( OnPlayerDeath );
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Gift of Renewal", "Olorisstra",
				-1
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 3.0 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 24; } }

		public static Hashtable m_Table = new Hashtable();
		public static Hashtable m_Table2 = new Hashtable();
		public static Hashtable m_Table3 = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			if ( m_Table.Contains( m ) )
				RemoveEffect( m, true );
		}

		private static void ExpireTime_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_Table2.Remove( m );
		}

		public static void RemoveEffect( Mobile m )
		{
			RemoveEffect( m, false );
		}

		public static void RemoveEffect( Mobile m, bool effects )
		{
			if ( m_Table.Contains( m ) && m_Table3.Contains( m ) )
			{
				m_Table.Remove( m );
				m_Table3.Remove( m );

				if ( effects )
				{
					m.SendLocalizedMessage( 1075071 ); // The Gift of Renewal has faded.
					m.PlaySound( 0xE0 );
				}

				BuffInfo.RemoveBuff( m, BuffIcon.GiftOfRenewal );
			}
		}

		private static void OnPlayerDeath( PlayerDeathEventArgs e )
		{
			RemoveEffect( e.Mobile, true );
		}

		public GiftOfRenewalSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( m_Table.Contains( m ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
			}
			else if ( m_Table2.Contains( Caster ) )
			{
				Caster.SendLocalizedMessage( 501789 ); // You must wait before trying again.
				return;
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				int focuslevel = GetFocusLevel( Caster );

				int seconds = 30 + ( Caster.Skills[SkillName.Spellweaving].Fixed / 240 );
				seconds += focuslevel * 10;

				int toHeal = focuslevel + 8;

				AddEffect( Caster, m, seconds, toHeal );
			}

			FinishSequence();
		}

		public static void AddEffect( Mobile caster, Mobile target, int seconds, int toHeal )
		{
			Timer t = (Timer) m_Table[target];

			if ( t != null )
				t.Stop();

			m_Table[target] = t = Timer.DelayCall( TimeSpan.FromSeconds( seconds ), new TimerStateCallback( Expire_Callback ), target );
			m_Table3[target] = caster;

			t = (Timer) m_Table2[caster];

			if ( t != null )
				t.Stop();

			m_Table2[caster] = t = Timer.DelayCall( TimeSpan.FromSeconds( seconds + 60 ), new TimerStateCallback( ExpireTime_Callback ), caster );

			BuffInfo.AddBuff( caster, new BuffInfo( BuffIcon.GiftOfRenewal, 1075796, TimeSpan.FromSeconds( seconds ), caster, String.Format( "{0}", toHeal ) ) );

			target.FixedParticles( 0x374A, 10, 15, 5021, EffectLayer.Waist );
			target.PlaySound( 0x5C9 );
		}

		public static void TryCure( Mobile m )
		{
			Poison p = m.Poison;

			Mobile caster = m_Table3[m] as Mobile;

			if ( p != null && caster != null )
			{
				int chanceToCure = 10000 + (int) ( caster.Skills[SkillName.Spellweaving].Value * 75 ) - ( ( p.Level + 1 ) * 3000 );

				chanceToCure += GetFocusLevel( caster ) * 1000;

				if ( chanceToCure > 10000 )
					chanceToCure = 10000;

				chanceToCure /= 100;

				if ( chanceToCure > Utility.Random( 100 ) )
				{
					if ( m.CurePoison( caster ) )
						m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
				}
			}

			m.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
			m.PlaySound( 0x1E0 );

			GiftOfRenewalTimer.ToDelete.Add( m );
		}

		public static void Heal( Mobile m )
		{
			Mobile caster = m_Table3[m] as Mobile;

			int toHeal = 8 + GetFocusLevel( caster );

			ArcaneEmpowermentSpell.ApplyHealBonus( caster, ref toHeal );

			m.Heal( toHeal, caster );
			m.FixedParticles( 0x376A, 9, 32, 5005, EffectLayer.Waist );
		}

		public class InternalTarget : Target
		{
			private GiftOfRenewalSpell m_Owner;

			public InternalTarget( GiftOfRenewalSpell owner )
				: base( -1, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile) o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}

	public class GiftOfRenewalTimer : Timer
	{
		public static ArrayList ToDelete;

		public static void Initialize()
		{
			GiftOfRenewalTimer timer = new GiftOfRenewalTimer();
			timer.Start();
		}

		public GiftOfRenewalTimer()
			: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
		{
			ToDelete = new ArrayList();
		}

		protected override void OnTick()
		{
			foreach ( Mobile m in GiftOfRenewalSpell.m_Table.Keys )
			{
				if ( m.Hits < m.HitsMax )
					GiftOfRenewalSpell.Heal( m );

				if ( m.Poisoned )
					GiftOfRenewalSpell.TryCure( m );
			}

			foreach ( Mobile m in ToDelete )
				GiftOfRenewalSpell.RemoveEffect( m, true );

			ToDelete.Clear();
		}
	}
}