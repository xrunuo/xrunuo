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
using System.IO;
using System.Collections.Generic;

namespace Server
{
	public class AggressorInfo
	{
		private Mobile m_Attacker, m_Defender;
		private DateTime m_LastCombatTime;
		private bool m_CanReportMurder;
		private bool m_Reported;
		private bool m_CriminalAggression;

		private bool m_Queued;

		private static Queue<AggressorInfo> m_Pool = new Queue<AggressorInfo>();

		public static AggressorInfo Create( Mobile attacker, Mobile defender, bool criminal )
		{
			AggressorInfo info;

			if ( m_Pool.Count > 0 )
			{
				info = m_Pool.Dequeue();

				info.m_Attacker = attacker;
				info.m_Defender = defender;

				info.m_CanReportMurder = criminal;
				info.m_CriminalAggression = criminal;

				info.m_Queued = false;

				info.Refresh();
			}
			else
			{
				info = new AggressorInfo( attacker, defender, criminal );
			}

			return info;
		}

		public void Free()
		{
			if ( m_Queued )
				return;

			m_Queued = true;
			m_Pool.Enqueue( this );
		}

		private AggressorInfo( Mobile attacker, Mobile defender, bool criminal )
		{
			m_Attacker = attacker;
			m_Defender = defender;

			m_CanReportMurder = criminal;
			m_CriminalAggression = criminal;

			Refresh();
		}

		private static TimeSpan m_ExpireDelay = TimeSpan.FromMinutes( 2.0 );

		public static TimeSpan ExpireDelay
		{
			get { return m_ExpireDelay; }
			set { m_ExpireDelay = value; }
		}

		public static void DumpAccess()
		{
			using ( StreamWriter op = new StreamWriter( "warnings.log", true ) )
			{
				op.WriteLine( "Warning: Access to queued AggressorInfo:" );
				op.WriteLine( new System.Diagnostics.StackTrace() );
				op.WriteLine();
				op.WriteLine();
			}
		}

		public bool Expired
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return ( m_Attacker.Deleted || m_Defender.Deleted || DateTime.Now >= ( m_LastCombatTime + m_ExpireDelay ) );
			}
		}

		public bool CriminalAggression
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_CriminalAggression;
			}
			set
			{
				if ( m_Queued )
					DumpAccess();

				m_CriminalAggression = value;
			}
		}

		public Mobile Attacker
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_Attacker;
			}
		}

		public Mobile Defender
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_Defender;
			}
		}

		public DateTime LastCombatTime
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_LastCombatTime;
			}
		}

		public bool Reported
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_Reported;
			}
			set
			{
				if ( m_Queued )
					DumpAccess();

				m_Reported = value;
			}
		}

		public bool CanReportMurder
		{
			get
			{
				if ( m_Queued )
					DumpAccess();

				return m_CanReportMurder;
			}
			set
			{
				if ( m_Queued )
					DumpAccess();

				m_CanReportMurder = value;
			}
		}

		public void Refresh()
		{
			if ( m_Queued )
				DumpAccess();

			m_LastCombatTime = DateTime.Now;
			//m_Reported = false;
		}
	}
}