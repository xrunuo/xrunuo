using System;
using System.Collections.Generic;

namespace Server.Items
{
	/// <summary>
	/// Temporarily enchants the attacker's weapon with deadly psychic energy,
	/// allowing it to damage the defender's mind and their ability to inflict
	/// damage with magic.
	/// </summary>
	public class PsychicAttack : WeaponAbility
	{
		public PsychicAttack()
		{
		}

		public override int BaseMana { get { return 30; } }

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			defender.PlaySound( 0x213 );

			attacker.SendLocalizedMessage( 1074383 ); // Your shot sends forth a wave of psychic energy.
			defender.SendLocalizedMessage( 1074384 ); // Your mind is attacked by psychic force!

			int extraDamage = 10 * (int) ( attacker.Int / defender.Int );

			if ( extraDamage < 10 )
				extraDamage = 10;
			if ( extraDamage > 20 )
				extraDamage = 20;

			AOS.Damage( defender, attacker, extraDamage, 100, 0, 0, 0, 0 ); // 100% Physical Damage

			if ( m_EffectTable.ContainsKey( defender ) )
				m_EffectTable[defender].Stop();

			m_EffectTable[defender] = Timer.DelayCall( TimeSpan.FromSeconds( 8.0 ), new TimerStateCallback( Expire_Callback ), defender );
		}

		private static Dictionary<Mobile, Timer> m_EffectTable = new Dictionary<Mobile, Timer>();

		private void Expire_Callback( object state )
		{
			Mobile m = state as Mobile;

			if ( m != null && m_EffectTable.ContainsKey( m ) )
				m_EffectTable.Remove( m );
		}

		public static bool UnderEffect( Mobile m )
		{
			return m_EffectTable.ContainsKey( m );
		}
	}
}