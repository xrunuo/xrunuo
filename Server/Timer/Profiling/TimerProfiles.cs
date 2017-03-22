//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using System.IO;

namespace Server
{
	public static class TimerProfiles
	{
		private static Hashtable m_Profiles = new Hashtable();

		public static TimerProfile GetProfile( this Timer timer )
		{
			if ( !Environment.Profiling )
				return null;

			string name = timer.ToString();

			TimerProfile prof = (TimerProfile) m_Profiles[name];

			if ( prof == null )
				m_Profiles[name] = prof = new TimerProfile();

			return prof;
		}

		public static void DumpInfo( StreamWriter sw )
		{
			sw.WriteLine( "# Dump on {0:f}", DateTime.UtcNow );
			sw.WriteLine( "# Core profiling for " + Environment.ProfileTime );
			sw.WriteLine();

			foreach ( DictionaryEntry de in m_Profiles )
			{
				string name = (string) de.Key;
				TimerProfile prof = (TimerProfile) de.Value;

				sw.WriteLine( "{6,-100}{0,-12}{1,12} {2,-12}{3,12} {4,-12:F5}{5:F5} {7:F5}", prof.Created, prof.Started, prof.Stopped, prof.Ticked, prof.TotalProcTime.TotalSeconds, prof.AverageProcTime.TotalSeconds, name, prof.PeakProcTime.TotalSeconds );
			}

			sw.WriteLine();
			sw.WriteLine();
		}
	}
}
