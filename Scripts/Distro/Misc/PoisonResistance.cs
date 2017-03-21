using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Events;
using Server.Spells.Bard;

namespace Server.Misc
{
	public static class PoisonResistance
	{
		public static void Initialize()
		{
			EventSink.PoisonCured += new PoisonCuredEventHandler( EventSink_PoisonCured );
		}

		private static void EventSink_PoisonCured( PoisonCuredEventArgs e )
		{
			int newResistLevel = Math.Min( e.Poison.Level + 1, 4 );

			if ( m_Table.ContainsKey( e.Mobile ) )
			{
				var context = m_Table[e.Mobile];

				if ( context.ResistanceLevel < newResistLevel )
				{
					context.ResistanceLevel = newResistLevel;

					var decayTimer = context.DecayTimer;

					if ( decayTimer != null )
						decayTimer.Stop();

					decayTimer = new DecayTimer( e.Mobile );
					decayTimer.Start();

					context.DecayTimer = decayTimer;
				}
			}
			else
			{
				var decayTimer = new DecayTimer( e.Mobile );
				decayTimer.Start();

				var context = new PoisonResistanceContext( e.Mobile, newResistLevel, decayTimer );
				m_Table[e.Mobile] = context;
			}
		}

		private static Dictionary<Mobile, PoisonResistanceContext> m_Table = new Dictionary<Mobile, PoisonResistanceContext>();

		public static double GetNaturalResistChance( Mobile m, Poison poison )
		{
			double poisoning = m.Skills.Poisoning.Value;

			Resilience song = Spellsong.GetEffectSpellsong<Resilience>( m );

			if ( song != null )
				poisoning += song.CurseReduction; // Guessing here.

			double factor = 80.0 / ( poison.Level + 1 );

			double chance = factor * poisoning / 100.0;

			return chance / 100.0;
		}

		public static double GetTemporaryResistChance( Mobile m, Poison poison )
		{
			if ( !m_Table.ContainsKey( m ) )
				return 0.0;

			int resistLevel = GetTemporaryResistLevel( m );
			double factor = 15.0 / ( poison.Level + 1 );

			double chance = factor * resistLevel / 100.0;

			return chance / 100.0;
		}

		private static int GetTemporaryResistLevel( Mobile m )
		{
			if ( !m_Table.ContainsKey( m ) )
				return 0;

			var context = m_Table[m];
			return context.ResistanceLevel;
		}

		public static void RemoveContext( Mobile m )
		{
			if ( m_Table.ContainsKey( m ) )
			{
				var context = m_Table[m];
				context.DecayTimer.Stop();

				m_Table.Remove( m );
			}
		}

		private class PoisonResistanceContext
		{
			public Mobile Owner { get; private set; }
			public int ResistanceLevel { get; set; }
			public Timer DecayTimer { get; set; }

			public PoisonResistanceContext( Mobile owner, int resistanceLevel, Timer decayTimer )
			{
				Owner = owner;
				ResistanceLevel = resistanceLevel;
				DecayTimer = decayTimer;
			}
		}

		private class DecayTimer : Timer
		{
			private Mobile m_Mobile;

			public DecayTimer( Mobile mobile )
				: base( TimeSpan.FromSeconds( 8.0 ), TimeSpan.FromSeconds( 8.0 ) )
			{
				m_Mobile = mobile;
			}

			protected override void OnTick()
			{
				if ( !m_Table.ContainsKey( m_Mobile ) )
				{
					Stop();
					return;
				}
				else
				{
					var context = m_Table[m_Mobile];
					context.ResistanceLevel--;

					if ( context.ResistanceLevel <= 0 )
					{
						m_Table.Remove( m_Mobile );

						Stop();
						return;
					}
				}
			}
		}
	}
}
