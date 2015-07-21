using System;
using System.Collections.Generic;
using Server;
using Server.Engines.BuffIcons;

namespace Server.Items
{
	public class HitLower
	{
		public static readonly TimeSpan AttackEffectDuration = TimeSpan.FromSeconds( 10.0 );
		public static readonly TimeSpan DefenseEffectDuration = TimeSpan.FromSeconds( 8.0 );

		private static Dictionary<Mobile, Timer> m_AttackTable = new Dictionary<Mobile, Timer>();

		public static bool IsUnderAttackEffect( Mobile m )
		{
			return m_AttackTable.ContainsKey( m );
		}

		public static void ApplyAttack( Mobile m )
		{
			if ( IsUnderAttackEffect( m ) )
				m_AttackTable[m].Stop();

			TimeSpan duration = AttackEffectDuration;

			if ( m.Weapon is BaseRanged )
				duration -= TimeSpan.FromSeconds( 3.0 );

			BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.HitLowerAttack, 1151315, 1151314, duration, m, "25" ) );

			m_AttackTable[m] = new AttackTimer( m, duration );
			m.SendLocalizedMessage( 1062319 ); // Your attack chance has been reduced!
		}

		private static void RemoveAttack( Mobile m )
		{
			m_AttackTable.Remove( m );
			m.SendLocalizedMessage( 1062320 ); // Your attack chance has returned to normal.
		}

		private class AttackTimer : Timer
		{
			private Mobile m_Player;

			public AttackTimer( Mobile player, TimeSpan duration )
				: base( duration )
			{
				m_Player = player;


				Start();
			}

			protected override void OnTick()
			{
				RemoveAttack( m_Player );
			}
		}

		private static Dictionary<Mobile, Timer> m_DefenseTable = new Dictionary<Mobile, Timer>();

		public static bool IsUnderDefenseEffect( Mobile m )
		{
			return m_DefenseTable.ContainsKey( m );
		}

		public static void ApplyDefense( Mobile m )
		{
			if ( IsUnderDefenseEffect( m ) )
				m_DefenseTable[m].Stop();

			TimeSpan duration = DefenseEffectDuration;

			if ( m.Weapon is BaseRanged )
				duration -= TimeSpan.FromSeconds( 3.0 );

			BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.HitLowerDefense, 1151313, 1151312, duration, m, "25" ) );

			m_DefenseTable[m] = new DefenseTimer( m, duration );
			m.SendLocalizedMessage( 1062318 ); // Your defense chance has been reduced!
		}

		private static void RemoveDefense( Mobile m )
		{
			m_DefenseTable.Remove( m );
			m.SendLocalizedMessage( 1062321 ); // Your defense chance has returned to normal.
		}

		private class DefenseTimer : Timer
		{
			private Mobile m_Player;

			public DefenseTimer( Mobile player, TimeSpan duration )
				: base( duration )
			{
				m_Player = player;


				Start();
			}

			protected override void OnTick()
			{
				RemoveDefense( m_Player );
			}
		}
	}
}