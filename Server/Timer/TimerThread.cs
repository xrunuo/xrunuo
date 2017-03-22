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
