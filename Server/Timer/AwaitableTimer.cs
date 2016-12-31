//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2017 Pedro Pardal
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
using System.Runtime.CompilerServices;

namespace Server
{
	public static class AwaitableTimers
	{
		public static TimeSpanAwaiter GetAwaiter( this TimeSpan ts )
		{
			return new TimeSpanAwaiter( ts );
		}
	}

	public class TimeSpanAwaiter : INotifyCompletion
	{
		private readonly TimeSpan m_TimeSpan;

		public TimeSpanAwaiter( TimeSpan ts )
		{
			m_TimeSpan = ts;
		}

		public bool IsCompleted { get { return false; } }

		public void OnCompleted( Action continuation )
		{
			Timer.DelayCall( m_TimeSpan, () => { continuation(); } );
		}

		public void GetResult()
		{
		}
	}
}
