using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
	public class ChampionMeleeAI : BaseAI
	{
		private int blockcounter = 0;

		private FightMode curFightMode;

		private const double CombatantChangeChance = 0.3; // 30% chance to change combatant between Closest and Weaknest

		private const double TeleportChance = 0.9; // 90% chance to teleport to combatant place

		public ChampionMeleeAI( BaseCreature m )
			: base( m )
		{
			curFightMode = m.FightMode;
		}

		public void SelectFightMode()
		{
			if ( Utility.RandomDouble() <= CombatantChangeChance )
			{
				curFightMode = ( curFightMode == FightMode.Closest ) ? FightMode.Weakest : FightMode.Closest;
			}
		}

		public override bool DoActionWander()
		{
			blockcounter = 0;

			m_Mobile.DebugSay( "I have no combatant" );

			SelectFightMode();

			if ( AcquireFocusMob( m_Mobile.RangePerception, curFightMode, true, false, true ) )
			{
				m_Mobile.DebugSay( "I am going to attack {0}", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;

				Action = ActionType.Combat;
			}
			else
			{
				base.DoActionWander();
			}

			return true;
		}

		private void TryTeleport()
		{
			m_Mobile.DebugSay( "I'l try to teleport to combatant" );

			if ( ++blockcounter > 10 )
			{
				blockcounter = 0;

				if ( Utility.RandomDouble() <= TeleportChance )
				{
					m_Mobile.DebugSay( "I'm teleporting to combatant" );

					Point3D to = new Point3D( m_Mobile.Combatant.Location );

					m_Mobile.Location = to;

					m_Mobile.ProcessDelta();
				}
			}
		}

		public override bool DoActionCombat()
		{
			Mobile combatant = m_Mobile.Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map )
			{
				m_Mobile.DebugSay( "My combatant is gone, so my guard is up" );

				Action = ActionType.Guard;

				return true;
			}

			if ( MoveTo( combatant, true, m_Mobile.RangeFight ) )
			{
				if ( blockcounter > 0 )
				{
					blockcounter--;
				}

				m_Mobile.Direction = m_Mobile.GetDirectionTo( combatant );
			}
			else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, true, false, true ) )
			{
				TryTeleport();

				m_Mobile.DebugSay( "My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;

				Action = ActionType.Combat;

				return true;
			}
			else if ( m_Mobile.GetDistanceToSqrt( combatant ) > m_Mobile.RangePerception + 1 )
			{
				m_Mobile.DebugSay( "I cannot find {0}, so my guard is up", combatant.Name );

				Action = ActionType.Guard;

				return true;
			}
			else
			{
				m_Mobile.DebugSay( "I should be closer to {0}", combatant.Name );
			}

			return true;
		}

		public override bool DoActionGuard()
		{
			if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;

				Action = ActionType.Combat;
			}
			else
			{
				base.DoActionGuard();
			}

			return true;
		}

		public override bool DoActionFlee()
		{
			return true;
		}
	}
}