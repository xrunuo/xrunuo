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

namespace Server
{
	public class TimerProfile
	{
		private int m_Created;
		private int m_Started;
		private int m_Stopped;
		private int m_Ticked;
		private TimeSpan m_TotalProcTime;
		private TimeSpan m_PeakProcTime;

		[CommandProperty( AccessLevel.Administrator )]
		public int Created
		{
			get { return m_Created; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Started
		{
			get { return m_Started; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Stopped
		{
			get { return m_Stopped; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Ticked
		{
			get { return m_Ticked; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan TotalProcTime
		{
			get { return m_TotalProcTime; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan PeakProcTime
		{
			get { return m_PeakProcTime; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan AverageProcTime
		{
			get
			{
				if ( m_Ticked == 0 )
					return TimeSpan.Zero;

				return TimeSpan.FromTicks( m_TotalProcTime.Ticks / m_Ticked );
			}
		}

		public void RegCreation()
		{
			++m_Created;
		}

		public void RegStart()
		{
			++m_Started;
		}

		public void RegStopped()
		{
			++m_Stopped;
		}

		public void RegTicked( TimeSpan procTime )
		{
			++m_Ticked;
			m_TotalProcTime += procTime;

			if ( procTime > m_PeakProcTime )
				m_PeakProcTime = procTime;
		}

		public TimerProfile()
		{
		}
	}
}
