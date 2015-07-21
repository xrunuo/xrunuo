using System;
using Server.Engines.Housing;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Multis;
using Server.Spells;
using Server.Spells.Sixth;

namespace Server.SkillHandlers
{
	public class Hiding
	{
		private static bool m_CombatOverride;

		public static bool CombatOverride
		{
			get { return m_CombatOverride; }
			set { m_CombatOverride = value; }
		}

		public static void Initialize()
		{
			SkillInfo.Table[21].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			if ( m.Spell != null )
			{
				m.SendLocalizedMessage( 501238 ); // You are busy doing something else and cannot hide.
				return TimeSpan.FromSeconds( 1.0 );
			}

			double bonus = 0.0;

			var house = HousingHelper.FindHouseAt( m );

			if ( house != null && house.IsFriend( m ) )
			{
				bonus = 100.0;
			}

			int range = 18 - (int) ( m.Skills[SkillName.Hiding].Value / 10 );

			bool badCombat = ( !m_CombatOverride && m.Combatant != null && m.InRange( m.Combatant.Location, range ) && m.Combatant.InLOS( m ) );
			bool ok = !badCombat;

			if ( ok )
			{
				if ( !m_CombatOverride )
				{
					foreach ( Mobile check in m.GetMobilesInRange( range ) )
					{
						if ( check.InLOS( m ) && check.Combatant == m )
						{
							badCombat = true;
							ok = false;
							break;
						}
					}
				}

				ok = ( !badCombat && m.CheckSkill( SkillName.Hiding, 0.0 - bonus, 100.0 - bonus ) );
			}

			if ( badCombat )
			{
				m.RevealingAction();

				m.LocalOverheadMessage( MessageType.Regular, 0x22, 501237 ); // You can't seem to hide right now.

				return TimeSpan.FromSeconds( 1.0 );
			}
			else
			{
				if ( ok )
				{
					m.Hidden = true;
					m.Warmode = false;
					m.Combatant = null;
					m.Target = null;

					//If you have drank a invis potion, you must stop the timer in order to remain hidden.
					InvisibilitySpell.RemoveTimer( m );

					m.LocalOverheadMessage( MessageType.Regular, 0x1F4, 501240 ); // You have hidden yourself well.
				}
				else
				{
					m.RevealingAction();

					m.LocalOverheadMessage( MessageType.Regular, 0x22, 501241 ); // You can't seem to hide here.
				}

				return TimeSpan.FromSeconds( 10.0 );
			}
		}
	}
}