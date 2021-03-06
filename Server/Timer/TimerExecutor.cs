﻿using System;
using System.Collections.Generic;

namespace Server
{
	public class TimerExecutor
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

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
				var index = 0;

				while ( index < BreakCount && m_Queue.Count != 0 )
				{
					var timer = m_Queue.Dequeue();

					try
					{
						timer.Tick();
					}
					catch ( Exception ex )
					{
						log.Error( "Exception disarmed in Timer {0}: {1}", timer.GetType().FullName, ex );
					}

					timer.m_Queued = false;

					++index;
				}
			}
		}
	}
}
