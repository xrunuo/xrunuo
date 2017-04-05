using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Misc;
using Server.Engines.BuffIcons;

namespace Server
{
	public class PoisonImpl : Poison
	{
		[CallPriority( 10 )]
		public static void Configure()
		{
			Register( new PoisonImpl( "Lesser", 0, 4, 16, 7.5, 3.0, 2.25, 10, 4 ) );
			Register( new PoisonImpl( "Regular", 1, 8, 18, 10.0, 3.0, 3.25, 10, 3 ) );
			Register( new PoisonImpl( "Greater", 2, 12, 20, 15.0, 3.0, 4.25, 10, 2 ) );
			Register( new PoisonImpl( "Deadly", 3, 16, 30, 30.0, 3.0, 5.25, 15, 2 ) );
			Register( new PoisonImpl( "Lethal", 4, 20, 50, 35.0, 3.0, 5.25, 20, 2 ) );

			Register( new PoisonImpl( "Darkglow", 2, 12, 20, 15.0, 3.0, 4.25, 10, 2 ) );
			Register( new PoisonImpl( "Parasitic", 3, 16, 30, 30.0, 3.0, 5.25, 15, 2 ) );
		}

		public static Poison IncreaseLevel( Poison oldPoison )
		{
			Poison newPoison = ( oldPoison == null ? null : GetPoison( oldPoison.Level + 1 ) );

			return ( newPoison == null ? oldPoison : newPoison );
		}

		// Info
		private string m_Name;
		private int m_Level;

		// Damage
		private int m_Minimum, m_Maximum;
		private double m_Scalar;

		// Timers
		private TimeSpan m_Delay;
		private TimeSpan m_Interval;
		private int m_Count, m_MessageInterval;

		public PoisonImpl( string name, int level, int min, int max, double percent, double delay, double interval, int count, int messageInterval )
		{
			m_Name = name;
			m_Level = level;
			m_Minimum = min;
			m_Maximum = max;
			m_Scalar = percent * 0.01;
			m_Delay = TimeSpan.FromSeconds( delay );
			m_Interval = TimeSpan.FromSeconds( interval );
			m_Count = count;
			m_MessageInterval = messageInterval;
		}

		public override string Name => m_Name;
		public override int Level => m_Level;

		private class PoisonTimer : Timer
		{
			private PoisonImpl m_Poison;
			private Mobile m_Mobile;
			private int m_Index;

			public PoisonTimer( Mobile m, PoisonImpl p )
				: base( p.m_Delay, p.m_Interval )
			{
				m_Mobile = m;
				m_Poison = p;

				int damage = 1 + (int) ( m.Hits * p.m_Scalar );

				BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.Poison, 1017383, 1075633, TimeSpan.FromSeconds( (int) ( ( p.m_Count + 1 ) * p.m_Interval.TotalSeconds ) ), m, $"{damage}\t{(int)p.m_Interval.TotalSeconds}" ) );
			}

			protected override void OnTick()
			{
				if ( CheckResistPoison() )
				{
					// Curing this way cause we don't want to trigger the PoisonCured event,
					// so that the resistance timer is not refreshed.
					m_Mobile.Poison = null;
					BuffInfo.RemoveBuff( m_Mobile, BuffIcon.Poison );

					m_Mobile.LocalOverheadMessage( MessageType.Emote, 0x3F, true, "* You feel yourself resisting the effects of the poison *" );

					m_Mobile.NonlocalOverheadMessage( MessageType.Emote, 0x3F, true, $"* {m_Mobile.Name} seems resistant to the poison *" );

					Stop();
					return;
				}

				if ( m_Index++ == m_Poison.m_Count )
				{
					m_Mobile.SendLocalizedMessage( 502136 ); // The poison seems to have worn off.
					m_Mobile.Poison = null;

					Stop();
					return;
				}

				int damage = 1 + (int) ( m_Mobile.Hits * m_Poison.m_Scalar );

				if ( damage < m_Poison.m_Minimum )
					damage = m_Poison.m_Minimum;
				else if ( damage > m_Poison.m_Maximum )
					damage = m_Poison.m_Maximum;

				#region Darkglow
				var poisoner = DarkglowPotion.GetPoisoner( m_Mobile );

				if ( poisoner != null )
				{
					int distance = (int) m_Mobile.GetDistanceToSqrt( poisoner.Location );

					if ( distance > 1 )
					{
						if ( distance > 20 )
							distance = 20;

						damage += AOS.Scale( damage, distance * 5 );
						poisoner.SendLocalizedMessage( 1072850 ); // Darkglow poison increases your damage!
					}
				}
				#endregion

				var honorTarget = m_Mobile as IHonorTarget;

				if ( honorTarget != null && honorTarget.ReceivedHonorContext != null )
					honorTarget.ReceivedHonorContext.OnTargetPoisoned();

				AOS.Damage( m_Mobile, damage, 0, 0, 0, 100, 0 );

				if ( ( m_Index % m_Poison.m_MessageInterval ) == 0 )
					m_Mobile.OnPoisoned( m_Mobile, m_Poison, m_Poison );

				#region Parasitic
				poisoner = ParasiticPotion.GetPoisoner( m_Mobile );

				if ( poisoner != null && m_Mobile.InRange( poisoner.Location, 1 ) )
				{
					int toHeal = Math.Min( damage, poisoner.HitsMax - poisoner.Hits );

					if ( toHeal > 0 )
					{
						poisoner.SendLocalizedMessage( 1060203, toHeal.ToString() ); // You have had ~1_HEALED_AMOUNT~ hit points of damage healed.
						poisoner.Hits += toHeal;
					}
				}
				#endregion
			}

			private bool CheckResistPoison()
			{
				double naturalResistChance = PoisonResistance.GetNaturalResistChance( m_Mobile, m_Poison );
				double temporaryResistChance = PoisonResistance.GetTemporaryResistChance( m_Mobile, m_Poison );

				if ( naturalResistChance + temporaryResistChance > Utility.RandomDouble() )
					return true;

				if ( m_Poison.Level < 4 && TransformationSpell.UnderTransformation( m_Mobile, typeof( VampiricEmbraceSpell ) ) )
					return true;

				if ( m_Poison.Level < 3 && OrangePetals.UnderEffect( m_Mobile ) )
					return true;

				return false;
			}
		}

		public override Timer ConstructTimer( Mobile m )
		{
			return new PoisonTimer( m, this );
		}
	}
}
