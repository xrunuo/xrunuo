using System;
using System.Collections;

namespace Server.Items
{
	/// <summary>
	/// Archers focuses their will into an arrow of pure force, dazing their enemy. 
	/// Dazed enemies are temporarily easier to hit, and sometimes forget who they are attacking.
	/// </summary>	
	public class ForceArrow : WeaponAbility
	{
		public ForceArrow()
		{
		}

		public override int BaseMana { get { return 20; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( IsUnderForceArrowEffect( defender ) )
				return;

			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			if ( ApplyForceArrowEffect( defender ) )
			{
				attacker.SendLocalizedMessage( 1074381 ); // You fire an arrow of pure force.
				attacker.PlaySound( 0x5BD );
			}
		}

		public static readonly TimeSpan ForceArrowEffectDuration = TimeSpan.FromSeconds( 10.0 ); // TODO: Comprobar

		private static Hashtable m_ForceArrowTable = new Hashtable();

		public static bool IsUnderForceArrowEffect( Mobile m )
		{
			return m_ForceArrowTable.Contains( m );
		}

		public static bool ApplyForceArrowEffect( Mobile m )
		{
			if ( IsUnderForceArrowEffect( m ) )
				return false;

			m_ForceArrowTable[m] = new ForceArrowTimer( m );
			m.SendLocalizedMessage( 1074382 ); // You are struck by a force arrow!
			return true;
		}

		private static void RemoveForceArrowEffect( Mobile m )
		{
			m_ForceArrowTable.Remove( m );
		}

		private class ForceArrowTimer : Timer
		{
			private Mobile m_Player;

			public ForceArrowTimer( Mobile player )
				: base( ForceArrowEffectDuration )
			{
				m_Player = player;

				
				Start();
			}

			protected override void OnTick()
			{
				RemoveForceArrowEffect( m_Player );
			}
		}
	}
}