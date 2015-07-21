using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Spells;
using Server.Spells.Bushido;
using Server.Network;

namespace Server.Mobiles
{
	public class SamuraiAI : BaseAI
	{
		private static double defenceChance = 0.6;
		private static double attackChance = 0.5;
		private static double heChance = 0.5;

		public SamuraiAI( BaseCreature m )
			: base( m )
		{
		}

		public override bool DoActionWander()
		{
			m_Mobile.DebugSay( "I have no combatant" );

			if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
				{
					m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );
				}

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;
			}
			else
			{
				if ( m_Mobile.Combatant != null )
				{
					Action = ActionType.Combat;
					return true;
				}

				Mobile target = BaseAttackHelperSE.GetRandomAttacker( m_Mobile, m_Mobile.RangePerception );

				if ( target != null )
				{
					m_Mobile.Combatant = target;

					Action = ActionType.Combat;
				}

				base.DoActionWander();
			}

			return true;
		}

		private double MagnitudeBySkill()
		{
			return ( m_Mobile.Skills[SkillName.Bushido].Value / 1000.0 );
		}

		private bool CanUseAbility( double limit, int mana, double chance )
		{
			if ( m_Mobile.Skills[SkillName.Bushido].Value >= limit && m_Mobile.Mana >= mana )
			{
				if ( ( chance + MagnitudeBySkill() ) >= Utility.RandomDouble() )
				{
					return true;
				}
			}

			return false;
		}

		private void UseAttackAbility()
		{
			ArrayList t = BaseAttackHelperSE.GetAllAttackers( m_Mobile, 2 );

			if ( t.Count > 1 )
			{
				if ( Utility.Random( 3 ) != 1 )
				{
					if ( CanUseAbility( 70.0, 10, 1.0 ) )
					{
						SpecialMove.SetCurrentMove( m_Mobile, SpellRegistry.GetSpecialMove( 405 ) ); // Momentum Strike
						return;
					}
				}
			}

			if ( CanUseAbility( 50.0, 5, 1.0 ) )
				SpecialMove.SetCurrentMove( m_Mobile, SpellRegistry.GetSpecialMove( 404 ) ); // Lightning Strike

			return;
		}

		private void UseDefenceAbility()
		{
			if ( Confidence.IsConfident( m_Mobile ) || Evasion.IsEvading( m_Mobile ) || CounterAttack.IsCountering( m_Mobile ) )
				return;

			switch ( Utility.Random( 3 ) )
			{
				case 2:
					if ( CanUseAbility( 60.0, 10, 1.0 ) )
					{
						new Evasion( m_Mobile, null ).Cast();
						break;
					}
					goto case 1;
				case 1:
					if ( CanUseAbility( 40.0, 5, 1.0 ) )
					{
						new CounterAttack( m_Mobile, null ).Cast();
						break;
					}
					goto case 0;
				case 0:
					if ( CanUseAbility( 25.0, 10, 1.0 ) )
					{
						new Confidence( m_Mobile, null ).Cast();
					}
					break;
			}
		}

		public override bool DoActionCombat()
		{
			Mobile combatant = m_Mobile.Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet )
			{
				m_Mobile.DebugSay( "My combatant is gone, so my guard is up" );

				Action = ActionType.Guard;

				return true;
			}

			if ( MoveTo( combatant, true, m_Mobile.RangeFight ) )
			{
				m_Mobile.Direction = m_Mobile.GetDirectionTo( combatant );
			}
			else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
				{
					m_Mobile.DebugSay( "My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name );
				}

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;

				return true;
			}
			else if ( m_Mobile.GetDistanceToSqrt( combatant ) > m_Mobile.RangePerception + 1 )
			{
				if ( m_Mobile.Debug )
				{
					m_Mobile.DebugSay( "I cannot find {0}, so my guard is up", combatant.Name );
				}

				Action = ActionType.Guard;

				return true;
			}
			else
			{
				if ( m_Mobile.Debug )
				{
					m_Mobile.DebugSay( "I should be closer to {0}", combatant.Name );
				}
			}

			if ( !m_Mobile.Controlled && !m_Mobile.Summoned && m_Mobile.CanFlee )
			{
				if ( m_Mobile.Hits < m_Mobile.HitsMax * 20 / 100 )
				{
					// We are low on health, should we flee?

					bool flee = false;

					if ( m_Mobile.Hits < combatant.Hits )
					{
						// We are more hurt than them

						int diff = combatant.Hits - m_Mobile.Hits;

						flee = ( Utility.Random( 0, 100 ) < ( 10 + diff ) ); // (10 + diff)% chance to flee
					}
					else
					{
						flee = Utility.Random( 0, 100 ) < 10; // 10% chance to flee
					}

					if ( flee )
					{
						if ( m_Mobile.Debug )
						{
							m_Mobile.DebugSay( "I am going to flee from {0}", combatant.Name );
						}

						Action = ActionType.Flee;
					}
				}
			}

			if ( combatant.Hits < ( Utility.Random( 10 ) + 10 ) )
			{
				if ( CanUseAbility( 25.0, 0, heChance ) )
				{
					SpecialMove.SetCurrentMove( m_Mobile, SpellRegistry.GetSpecialMove( 400 ) ); // Honorable Execution
					return true;
				}
			}

			UseSamuraiAbility();

			return true;
		}

		private void UseSamuraiAbility()
		{
			if ( attackChance + MagnitudeBySkill() > Utility.RandomDouble() )
			{
				UseAttackAbility();
			}

			if ( defenceChance + MagnitudeBySkill() > Utility.RandomDouble() )
			{
				UseDefenceAbility();
			}
		}

		public override bool DoActionGuard()
		{
			if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
				{
					m_Mobile.DebugSay( "I have detected {0}, attacking", m_Mobile.FocusMob.Name );
				}

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
			if ( m_Mobile.Hits > m_Mobile.HitsMax / 2 )
			{
				m_Mobile.DebugSay( "I am stronger now, so I will continue fighting" );
				Action = ActionType.Combat;
			}
			else
			{
				m_Mobile.FocusMob = m_Mobile.Combatant;

				if ( m_Mobile.FocusMob == null || m_Mobile.FocusMob.Deleted || m_Mobile.FocusMob.Map != m_Mobile.Map )
				{
					m_Mobile.DebugSay( "I have lost im" );
					Action = ActionType.Guard;
					return true;
				}

				UseSamuraiAbility();

				if ( WalkMobileRange( m_Mobile.FocusMob, 1, false, m_Mobile.RangePerception * 2, m_Mobile.RangePerception * 3 ) )
				{
					m_Mobile.DebugSay( "I Have fled" );
					Action = ActionType.Guard;
					return true;
				}
				else
				{
					m_Mobile.DebugSay( "I am fleeing!" );
				}
			}

			return true;
		}
	}
}