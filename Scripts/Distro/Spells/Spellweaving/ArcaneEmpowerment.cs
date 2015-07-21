using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.BuffIcons;

namespace Server.Spells.Spellweaving
{
	public class ArcaneEmpowermentSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Arcane Empowerment",
				"Aslavdra",
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 4.0 ); } }

		public override double RequiredSkill { get { return 24.0; } }
		public override int RequiredMana { get { return 50; } }

		public ArcaneEmpowermentSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( m_Table.ContainsKey( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.PlaySound( 0x5C1 );

				int focuslevel = GetFocusLevel( Caster );
				int skill = Caster.Skills[SkillName.Spellweaving].Fixed;

				int duration = 15 + (int) ( skill / 240 ) + ( focuslevel * 2 );
				int sdiBonus = (int) Math.Floor( skill / 120.0 ) + ( focuslevel * 5 );
				double healBonus = 0.2 + ( focuslevel / 10 );
				int hitsBonus = 10;
				int dispelBonus = (int) Math.Floor( skill / 120.0 ) + focuslevel;

				m_Table[Caster] = new EmpowermentInfo( Caster, sdiBonus, healBonus, hitsBonus, dispelBonus, TimeSpan.FromSeconds( duration ) );

				BuffInfo.AddBuff( Caster, new BuffInfo( BuffIcon.ArcaneEmpowerment, 1031616, 1075808, TimeSpan.FromSeconds( duration ), Caster, new TextDefinition( String.Format( "{0}\t{1}", sdiBonus.ToString(), hitsBonus.ToString() ) ) ) );
			}

			FinishSequence();
		}

		private static Hashtable m_Table = new Hashtable();

		private class EmpowermentInfo
		{
			public Mobile m_Caster;
			public int m_SDIBonus;
			public double m_HealBonus;
			public int m_HitsBonus;
			public int m_DispelBonus;
			public ExpireTimer m_Timer;

			public EmpowermentInfo( Mobile caster, int sdiBonus, double healBonus, int hitsBonus, int dispelBonus, TimeSpan duration )
			{
				m_Caster = caster;
				m_SDIBonus = sdiBonus;
				m_HealBonus = healBonus;
				m_HitsBonus = hitsBonus;
				m_DispelBonus = dispelBonus;

				m_Timer = new ExpireTimer( m_Caster, duration );
				m_Timer.Start();
			}
		}

		public static int GetSDIBonus( Mobile m )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info == null )
				return 0;

			return info.m_SDIBonus;
		}

		public static void ApplyHealBonus( Mobile m, ref int points )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info == null )
				return;

			double scalar = 1.0 + info.m_HealBonus;

			points = (int) Math.Floor( points * scalar );
		}

		public static int GetSummonHitsBonus( Mobile m )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info == null )
				return 0;

			return info.m_HitsBonus;
		}

		public static int GetDispelBonus( Mobile m )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info == null )
				return 0;

			return info.m_DispelBonus;
		}

		public static bool StopBuffing( Mobile m, bool message )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info == null || info.m_Timer == null )
				return false;

			info.m_Timer.DoExpire( message );
			return true;
		}

		public static bool IsBuffed( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void RemoveBuff( Mobile m )
		{
			RemoveBuff( m, false );
		}

		public static void RemoveBuff( Mobile m, bool effects )
		{
			EmpowermentInfo info = m_Table[m] as EmpowermentInfo;

			if ( info != null )
			{
				info.m_Timer.Stop();

				if ( effects )
					m.PlaySound( 0x5C2 );

				m_Table.Remove( m );

				BuffInfo.RemoveBuff( m, BuffIcon.ArcaneEmpowerment );
			}
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;

			public ExpireTimer( Mobile m, TimeSpan delay )
				: base( delay )
			{
				m_Mobile = m;
			}

			protected override void OnTick()
			{
				DoExpire( true );
			}

			public void DoExpire( bool message )
			{
				RemoveBuff( m_Mobile, message );
			}
		}
	}
}