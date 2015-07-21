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
using System.Collections.Generic;

namespace Server
{
	public class TimerExecutor
	{
		/// <summary>
		/// Max number of timers that we process in a single slice.
		/// </summary>
		private const int BreakCount = 20000;

		private readonly Queue<Timer> m_Queue;

		public TimerExecutor( Queue<Timer> queue )
		{
			m_Queue = queue;

		}

		public void Slice()
		{
			lock ( m_Queue )
			{
				int index = 0;

				while ( index < BreakCount && m_Queue.Count != 0 )
				{
					Timer timer = m_Queue.Dequeue();

					try
					{
						timer.Tick();
					}
					catch ( Exception ex )
					{
						Logger.Error( "Exception disarmed in Timer {0}: {1}", timer.GetType().FullName, ex );
					}

					timer.m_Queued = false;

					++index;
				}
			}
		}
	}
}
