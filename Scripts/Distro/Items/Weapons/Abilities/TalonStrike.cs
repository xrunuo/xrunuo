using System;
using Server;
using System.Collections;

namespace Server.Items
{
	/// <summary>
	/// Attack with increased damage with additional damage over time. Requires Ninjitsu skill.
	/// </summary>
	public class TalonStrike : SEWeaponAbility
	{
		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			attacker.SendLocalizedMessage( 1063358 ); // You deliver a talon strike!
			defender.SendLocalizedMessage( 1063359 ); // Your attacker delivers a talon strike!			

			defender.FixedParticles( 0x373A, 1, 17, 0x26BC, 0x662, 0, EffectLayer.Waist );

			BeginTalon( defender, attacker, damage );
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool IsTalon( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginTalon( Mobile m, Mobile from, int damage )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			t = new InternalTimer( from, m, damage );
			m_Table[m] = t;

			t.Start();
		}

		public static void DoTalon( Mobile m, Mobile from, int level )
		{
			if ( m.Alive )
			{
				int damage = (int) ( level * 0.2 );

				m.Damage( damage, from );
			}
			else
			{
				EndTalon( m, false );
			}
		}

		public static void EndTalon( Mobile m, bool message )
		{
			Timer t = (Timer) m_Table[m];

			if ( t == null )
				return;

			t.Stop();
			m_Table.Remove( m );
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_Mobile;
			private int m_Count;
			private int damage;

			public InternalTimer( Mobile from, Mobile m, int damages )
				: base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ) )
			{
				m_From = from;
				m_Mobile = m;
				damage = damages;
				}

			protected override void OnTick()
			{
				if ( m_Count == 2 )
					damage /= 2;

				DoTalon( m_Mobile, m_From, damage );

				if ( m_Count < 3 )
					m_Count++;

				if ( m_Count == 3 )
					EndTalon( m_Mobile, true );
			}
		}
	}
}