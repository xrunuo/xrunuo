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
using System.IO;
using System.Threading;

namespace Server
{
	public class TimerScheduler
	{
		private static TimerScheduler m_Instance;

		public static TimerScheduler Instance
		{
			get { return m_Instance; }
			set { m_Instance = value; }
		}

		private readonly Queue<Timer> m_Queue;
		private readonly Queue<TimerChangeEntry> m_ChangeQueue;

		public TimerScheduler( Queue<Timer> queue )
		{
			m_Queue = queue;
			m_ChangeQueue = new Queue<TimerChangeEntry>();
		}

		private static readonly TimeSpan[] PriorityDelays = new TimeSpan[8]
			{
				TimeSpan.Zero,
				TimeSpan.FromMilliseconds( 10.0 ),
				TimeSpan.FromMilliseconds( 25.0 ),
				TimeSpan.FromMilliseconds( 50.0 ),
				TimeSpan.FromMilliseconds( 250.0 ),
				TimeSpan.FromSeconds( 1.0 ),
				TimeSpan.FromSeconds( 5.0 ),
				TimeSpan.FromMinutes( 1.0 )
			};

		private readonly DateTime[] m_NextPriorities = new DateTime[8];

		private readonly List<Timer>[] m_Timers = new List<Timer>[8]
			{
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>(),
				new List<Timer>()
			};

		public void Slice()
		{
			var now = CurrentTime.Now;

			ProcessChangeQueue( now );

			for ( int i = 0; i < m_Timers.Length; i++ )
			{
				if ( now < m_NextPriorities[i] )
					break;

				m_NextPriorities[i] = now + PriorityDelays[i];

				for ( int j = 0; j < m_Timers[i].Count; j++ )
				{
					Timer timer = m_Timers[i][j];

					if ( !timer.m_Queued && now > timer.m_Next )
					{
						timer.m_Queued = true;

						lock ( m_Queue )
							m_Queue.Enqueue( timer );

						if ( timer.Count != 0 && ( ++timer.m_Index >= timer.Count ) )
						{
							timer.Stop();
						}
						else
						{
							timer.m_Next = now + timer.Interval;
						}
					}
				}
			}
		}

		private void ProcessChangeQueue( DateTime now )
		{
			lock ( m_ChangeQueue )
			{
				while ( m_ChangeQueue.Count > 0 )
				{
					TimerChangeEntry tce = m_ChangeQueue.Dequeue();
					Timer timer = tce.Timer;
					int newIndex = tce.NewIndex;

					if ( timer.m_List != null )
						timer.m_List.Remove( timer );

					if ( tce.IsAdd )
					{
						timer.m_Next = now + timer.Delay;
						timer.m_Index = 0;
					}

					if ( newIndex >= 0 )
					{
						timer.m_List = m_Timers[newIndex];
						timer.m_List.Add( timer );
					}
					else
					{
						timer.m_List = null;
					}

					tce.Free();
				}
			}
		}

		public void AddTimer( Timer timer )
		{
			TimerPriority priority;

			if ( timer.Count == 1 )
				priority = ComputePriority( timer.Delay );
			else
				priority = ComputePriority( timer.Interval );

			Change( timer, (int) priority, true );
		}

		private static TimerPriority ComputePriority( TimeSpan ts )
		{
			if ( ts >= TimeSpan.FromMinutes( 1.0 ) )
				return TimerPriority.FiveSeconds;

			if ( ts >= TimeSpan.FromSeconds( 10.0 ) )
				return TimerPriority.OneSecond;

			if ( ts >= TimeSpan.FromSeconds( 5.0 ) )
				return TimerPriority.TwoFiftyMS;

			if ( ts >= TimeSpan.FromSeconds( 2.5 ) )
				return TimerPriority.FiftyMS;

			if ( ts >= TimeSpan.FromSeconds( 1.0 ) )
				return TimerPriority.TwentyFiveMS;

			if ( ts >= TimeSpan.FromSeconds( 0.5 ) )
				return TimerPriority.TenMS;

			return TimerPriority.EveryTick;
		}

		public void RemoveTimer( Timer timer )
		{
			Change( timer, -1, false );
		}

		private void Change( Timer timer, int newIndex, bool isAdd )
		{
			TimerChangeEntry entry = TimerChangeEntry.GetInstance( timer, newIndex, isAdd );

			lock ( m_ChangeQueue )
				m_ChangeQueue.Enqueue( entry );
		}

		private class TimerChangeEntry
		{
			public Timer Timer { get; private set; }
			public int NewIndex { get; private set; }
			public bool IsAdd { get; private set; }

			private TimerChangeEntry( Timer t, int newIndex, bool isAdd )
			{
				Timer = t;
				NewIndex = newIndex;
				IsAdd = isAdd;
			}

			public void Free()
			{
				Timer = null;

				lock ( m_InstancePool )
					m_InstancePool.Enqueue( this );
			}

			private static Queue<TimerChangeEntry> m_InstancePool = new Queue<TimerChangeEntry>();

			public static TimerChangeEntry GetInstance( Timer t, int newIndex, bool isAdd )
			{
				TimerChangeEntry e;

				lock ( m_InstancePool )
				{
					if ( m_InstancePool.Count > 0 )
					{
						e = m_InstancePool.Dequeue();

						e.Timer = t;
						e.NewIndex = newIndex;
						e.IsAdd = isAdd;

						return e;
					}
				}

				return new TimerChangeEntry( t, newIndex, isAdd );
			}
		}

		public void DumpInfo( TextWriter tw )
		{
			for ( int i = 0; i < m_Timers.Length; ++i )
			{
				tw.WriteLine( "Priority: {0}", (TimerPriority) i );
				tw.WriteLine();

				var hash = new Dictionary<string, List<Timer>>();

				for ( int j = 0; j < m_Timers[i].Count; ++j )
				{
					Timer t = m_Timers[i][j];

					string key = t.ToString();

					List<Timer> list;
					hash.TryGetValue( key, out list );

					if ( list == null )
						hash[key] = list = new List<Timer>();

					list.Add( t );
				}

				foreach ( KeyValuePair<string, List<Timer>> kv in hash )
				{
					string key = kv.Key;
					List<Timer> list = kv.Value;

					tw.WriteLine( "Type: {0}; Count: {1}; Percent: {2}%", key, list.Count, (int) ( 100 * ( list.Count / (double) m_Timers[i].Count ) ) );
				}

				tw.WriteLine();
				tw.WriteLine();
			}
		}
	}
}
