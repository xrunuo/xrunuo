using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Misc
{
	public static class ManaPhase
	{
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 30.0 );

		private static IDictionary<Mobile, BaseTalisman> m_UnderEffect = new Dictionary<Mobile, BaseTalisman>();
		private static ISet<Mobile> m_Cooldown = new HashSet<Mobile>();

		public static void OnUse( Mobile m, BaseTalisman talisman )
		{
			if ( m_Cooldown.Contains( m ) )
			{
				// You must wait a few seconds before attempting to phase mana again.
				m.SendLocalizedMessage( 1116163 );
			}
			else if ( m_UnderEffect.ContainsKey( m ) )
			{
				// You will no longer attempt to draw magical energy from the void.
				m.SendLocalizedMessage( 1116165 );

				m_UnderEffect.Remove( m );
			}
			else
			{
				// Your next use of magical energy will draw its power from the void.
				m.SendLocalizedMessage( 1116164 );

				m_UnderEffect.Add( m, talisman );
			}
		}

		public static bool UnderEffect( Mobile m )
		{
			BaseTalisman talisman;

			if ( !m_UnderEffect.TryGetValue( m, out talisman ) )
				return false;

			return m.Talisman == talisman;
		}

		public static void OnManaConsumed( Mobile m )
		{
			if ( m_UnderEffect.ContainsKey( m ) )
			{
				BaseTalisman talisman = m_UnderEffect[m];
				talisman.Charges--;

				m_Cooldown.Add( m );
				Timer.DelayCall( Cooldown, () => m_Cooldown.Remove( m ) );

				m_UnderEffect.Remove( m );
			}
		}
	}
}
