using System;
using Server;
using System.Collections;

namespace Server.Items
{
	/// <summary>
	/// Attack faster as you swing with both weapons. Requires Ninjitsu skill.
	/// </summary>
	public class DualWield : SEWeaponAbility
	{
		private static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		private static void Expire_Callback( object state )
		{
			Mobile m = (Mobile) state;

			m_Table.Remove( m );
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
			if ( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

			Timer t = (Timer) m_Table[attacker];

			if ( t != null )
				t.Stop();

			Skill ninjitsu = attacker.Skills[SkillName.Ninjitsu];

			int delay = (int) ( ninjitsu.Value / 12.0 );

			attacker.SendLocalizedMessage( 1063362 ); // You dually wield for increased speed!

			attacker.FixedParticles( 0x3779, 1, 15, 0x7F6, 0x3E8, 0x3, EffectLayer.LeftHand );

			m_Table[attacker] = t = Timer.DelayCall( TimeSpan.FromSeconds( delay ), new TimerStateCallback( Expire_Callback ), attacker );
		}
	}
}