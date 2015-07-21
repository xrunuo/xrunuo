using System;
using System.Collections.Generic;
using Server.Engines.Housing.Multis;
using Server.Multis;

namespace Server.Engines.Housing
{
	public static class DynamicDecay
	{
		private static Dictionary<DecayLevel, DecayStageInfo> m_Stages;

		static DynamicDecay()
		{
			m_Stages = new Dictionary<DecayLevel, DecayStageInfo>();

			Register( DecayLevel.LikeNew, TimeSpan.FromHours( 1.0 ), TimeSpan.FromHours( 1.0 ) );
			Register( DecayLevel.Slightly, TimeSpan.FromDays( 1.0 ), TimeSpan.FromDays( 2.0 ) );
			Register( DecayLevel.Somewhat, TimeSpan.FromDays( 1.0 ), TimeSpan.FromDays( 2.0 ) );
			Register( DecayLevel.Fairly, TimeSpan.FromDays( 1.0 ), TimeSpan.FromDays( 2.0 ) );
			Register( DecayLevel.Greatly, TimeSpan.FromDays( 1.0 ), TimeSpan.FromDays( 2.0 ) );
			Register( DecayLevel.IDOC, TimeSpan.FromHours( 12.0 ), TimeSpan.FromHours( 24.0 ) );
		}

		private static void Register( DecayLevel level, TimeSpan min, TimeSpan max )
		{
			DecayStageInfo info = new DecayStageInfo( min, max );

			if ( m_Stages.ContainsKey( level ) )
				m_Stages[level] = info;
			else
				m_Stages.Add( level, info );
		}

		public static bool Decays( DecayLevel level )
		{
			return m_Stages.ContainsKey( level );
		}

		public static TimeSpan GetRandomDuration( DecayLevel level )
		{
			if ( !m_Stages.ContainsKey( level ) )
				return TimeSpan.Zero;

			DecayStageInfo info = m_Stages[level];

			long min = info.MinDuration.Ticks;
			long max = info.MaxDuration.Ticks;

			return TimeSpan.FromTicks( min + (long) ( Utility.RandomDouble() * ( max - min ) ) );
		}
	}
}

namespace Server.Multis
{
	public class DecayStageInfo
	{
		public TimeSpan MinDuration { get; private set; }
		public TimeSpan MaxDuration { get; private set; }

		public DecayStageInfo( TimeSpan min, TimeSpan max )
		{
			MinDuration = min;
			MaxDuration = max;
		}
	}
}
