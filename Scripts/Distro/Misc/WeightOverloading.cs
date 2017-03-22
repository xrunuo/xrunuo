using System;
using Server;
using Server.Mobiles;
using Server.Events;

namespace Server.Misc
{
	public enum DFAlgorithm
	{
		Standard,
		PainSpike
	}

	public class WeightOverloading
	{
		public static void Initialize()
		{
			EventSink.Movement += new MovementEventHandler( EventSink_Movement );
		}

		private static DFAlgorithm m_DFA;

		public static DFAlgorithm DFA { get { return m_DFA; } set { m_DFA = value; } }

		public static void FatigueOnDamage( Mobile m, int damage )
		{
			double fatigue = 0.0;

			switch ( m_DFA )
			{
				case DFAlgorithm.Standard:
					{
						fatigue = ( damage * ( 100.0 / m.Hits ) * ( (double) m.Stam / 100 ) ) - 5.0;
						break;
					}
				case DFAlgorithm.PainSpike:
					{
						fatigue = ( damage * ( ( 100.0 / m.Hits ) + ( ( 50.0 + m.Stam ) / 100 ) - 1.0 ) ) - 5.0;
						break;
					}
			}

			if ( fatigue > 0 )
				m.Stam -= (int) fatigue;
		}

		public const int OverloadAllowance = 4; // We can be four stones overweight without getting fatigued

		public static int GetMaxWeight( Mobile m )
		{
			int maxweight = 40 + (int) ( 3.5 * m.Str );

			/*
			 * Racial Abilities: Strong Back (Humans)
			 * Humans have an increased carrying capacity above what is determined by their strength. 
			 */
			if ( m.Player && m.Race == Race.Human )
				maxweight += 60;

			return maxweight;
		}

		public static void EventSink_Movement( MovementEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( !from.Player || !from.Alive || from.AccessLevel >= AccessLevel.GameMaster )
				return;

			bool isRunning = ( e.Direction & Direction.Running ) != 0;
			int maxWeight = GetMaxWeight( from ) + OverloadAllowance;
			int overWeight = ( Mobile.BodyWeight + from.TotalWeight ) - maxWeight;

			if ( overWeight > 0 )
			{
				from.Stam -= GetStamLoss( from, overWeight, isRunning );

				if ( from.Stam == 0 )
				{
					from.SendLocalizedMessage( 500109 ); // You are too fatigued to move, because you are carrying too much weight!
					e.Blocked = true;
					return;
				}
			}

			if ( isRunning && ( ( from.Stam * 100 ) / Math.Max( from.StamMax, 1 ) ) < 10 )
				--from.Stam;

			if ( from.Stam == 0 )
			{
				from.SendLocalizedMessage( 500110 ); // You are too fatigued to move.
				e.Blocked = true;
				return;
			}

			if ( from is PlayerMobile )
			{
				int amt = ( from.Mounted ? 48 : 16 );
				PlayerMobile pm = (PlayerMobile) from;

				if ( ( ++pm.StepsTaken % amt ) == 0 )
					--from.Stam;
			}
		}

		public static int GetStamLoss( Mobile from, int overWeight, bool running )
		{
			int loss = 5 + ( overWeight / 25 );

			if ( from.Mounted || from.Flying )
				loss /= 3;

			if ( running )
				loss *= 2;

			return loss;
		}

		public static bool IsOverloaded( Mobile m )
		{
			if ( !m.Player || !m.Alive || m.AccessLevel >= AccessLevel.GameMaster )
				return false;

			return ( ( Mobile.BodyWeight + m.TotalWeight ) > ( GetMaxWeight( m ) + OverloadAllowance ) );
		}
	}
}