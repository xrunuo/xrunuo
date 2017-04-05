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

		public bool IsCompleted => false;

		public void OnCompleted( Action continuation )
		{
			Timer.DelayCall( m_TimeSpan, () => { continuation(); } );
		}

		public void GetResult()
		{
		}
	}
}
