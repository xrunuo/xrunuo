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
using System.Threading;

namespace Server
{
	public class TimerThread
	{
		private readonly TimerScheduler m_Scheduler;
		private readonly Thread m_Thread;

		public TimerThread( TimerScheduler scheduler )
		{
			m_Scheduler = scheduler;

			m_Thread = new Thread( ThreadMain );
			m_Thread.Name = "Timer Scheduler Thread";
		}

		public void Start()
		{
			m_Thread.Start();
		}

		public void Stop()
		{
			if ( m_Thread != null && m_Thread.IsAlive )
				m_Thread.Abort();
		}

		private void ThreadMain()
		{
			while ( !Core.Closing )
			{
				m_Scheduler.Slice();

				Thread.Sleep( 10 );
			}
		}
	}
}
