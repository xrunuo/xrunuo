using System;
using Server;
using System.Collections;
using Server.Spells;
using Server.Engines.PartySystem;

namespace Server.Items
{
	/// <summary>
	/// A quick attack to all enemies in range of your weapon that causes damage over time. Requires Bushido or Ninjitsu skill.
	/// </summary>
	public class FrenziedWhirlwind : SEWeaponAbility
	{
		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			Map map = attacker.Map;

			if ( map == null )
				return;

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if ( weapon == null )
				return;

			attacker.FixedEffect( 0x3728, 10, 15 );
			attacker.PlaySound( 0x2A1 );

			ArrayList list = new ArrayList();

			foreach ( Mobile m in attacker.GetMobilesInRange( 1 ) )
			{
				list.Add( m );
			}

			Party p = Party.Get( attacker );

			for ( int i = 0; i < list.Count; ++i )
			{
				Mobile m = (Mobile) list[i];

				if ( m != defender && m != attacker && SpellHelper.ValidIndirectTarget( attacker, m ) && ( p == null || !p.Contains( m ) ) )
				{
					if ( m == null || m.Deleted || attacker.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.Alive || !attacker.CanSee( m ) )
						continue;

					if ( !attacker.InRange( m, weapon.MaxRange ) )
						continue;

					if ( attacker.InLOS( m ) )
					{
						attacker.RevealingAction();

						BeginFrenzied( m, attacker, damage );

						weapon.OnHit( attacker, m );
					}
				}
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool IsFrenzied( Mobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginFrenzied( Mobile m, Mobile from, int damage )
		{
			Timer t = (Timer) m_Table[m];

			if ( t != null )
				t.Stop();

			t = new InternalTimer( from, m, damage );
			m_Table[m] = t;

			t.Start();
		}

		public static void DoFrenzied( Mobile m, Mobile from, int level )
		{
			if ( m.Alive )
			{
				int damage = (int) ( level * 0.7 ); // is this correct?

				m.Damage( damage, from );
			}
			else
			{
				EndFrenzied( m, false );
			}
		}

		public static void EndFrenzied( Mobile m, bool message )
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
			private int damage;

			public InternalTimer( Mobile from, Mobile m, int damages )
				: base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_From = from;
				m_Mobile = m;
				damage = damages;
				}

			protected override void OnTick()
			{
				DoFrenzied( m_Mobile, m_From, damage );

				EndFrenzied( m_Mobile, true );
			}
		}
	}
}