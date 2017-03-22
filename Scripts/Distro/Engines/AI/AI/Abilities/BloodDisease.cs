using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class BloodDisease : SpecialAbility
	{
		private static Dictionary<Mobile, InternalTimer> m_Table = new Dictionary<Mobile, InternalTimer>();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public override void OnGotMeleeAttack( Mobile m, Mobile attacker )
		{
			if ( FontOfFortune.HasBlessing( attacker, FontOfFortune.BlessingType.Protection ) )
				return;

			if ( !m_Table.ContainsKey( attacker ) && m.InRange( attacker, 1 ) )
			{
				// The rotworm has infected you with blood disease!!
				attacker.SendLocalizedMessage( 1111672, "", 0x25 );

				attacker.PlaySound( 0x213 );
				Effects.SendTargetParticles( attacker, 0x373A, 1, 15, 0x26B9, EffectLayer.Head );

				var timer = new InternalTimer( attacker );
				timer.Start();

				m_Table.Add( attacker, timer );
			}
		}

		private class InternalTimer : Timer
		{
			private const int MaxCount = 8;

			private int m_Count;
			private Mobile m_Victim;

			public InternalTimer( Mobile m )
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Victim = m;
			}

			protected override void OnTick()
			{
				if ( m_Count == MaxCount || m_Victim.Deleted || !m_Victim.Alive || m_Victim.IsDeadBondedPet )
				{
					// You no longer feel sick.
					m_Victim.SendLocalizedMessage( 1111673 );

					m_Table.Remove( m_Victim );
					Stop();
				}
				else if ( m_Count > 0 )
				{
					AOS.Damage( m_Victim, Utility.RandomMinMax( 10, 20 ), 0, 0, 0, 100, 0 );
					m_Victim.Combatant = null;
				}

				m_Count++;
			}
		}
	}
}
