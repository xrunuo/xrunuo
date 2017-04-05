using System;
using System.Collections;
using System.IO;

namespace Server
{
	public static class TimerProfiles
	{
		private static readonly Hashtable m_Profiles = new Hashtable();

		public static TimerProfile GetProfile( this Timer timer )
		{
			if ( !Core.Profiling )
				return null;

			var name = timer.ToString();

			var prof = (TimerProfile) m_Profiles[name];

			if ( prof == null )
				m_Profiles[name] = prof = new TimerProfile();

			return prof;
		}

		public static void DumpInfo( StreamWriter sw )
		{
			sw.WriteLine( "# Dump on {0:f}", DateTime.UtcNow );
			sw.WriteLine( "# Core profiling for " + Core.ProfileTime );
			sw.WriteLine();

			foreach ( DictionaryEntry de in m_Profiles )
			{
				var name = (string) de.Key;
				var prof = (TimerProfile) de.Value;

				sw.WriteLine( "{6,-100}{0,-12}{1,12} {2,-12}{3,12} {4,-12:F5}{5:F5} {7:F5}", prof.Created, prof.Started, prof.Stopped, prof.Ticked, prof.TotalProcTime.TotalSeconds, prof.AverageProcTime.TotalSeconds, name, prof.PeakProcTime.TotalSeconds );
			}

			sw.WriteLine();
			sw.WriteLine();
		}
	}
}
