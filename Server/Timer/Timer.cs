using System;
using System.Collections.Generic;

namespace Server
{
	public delegate void TimerCallback();
	public delegate void TimerStateCallback( object state );
	public delegate void TimerStateCallback<T>( T state );

	public class Timer
	{
		private readonly TimerScheduler m_Scheduler;

		internal bool m_Queued;
		internal int m_Index;
		internal List<Timer> m_List;

		public DateTime Next { get; internal set; }
		public TimeSpan Delay { get; set; }
		public TimeSpan Interval { get; set; }
		public int Count { get; }
		public bool Running { get; private set; }

		public Timer( TimeSpan delay )
			: this( delay, TimeSpan.Zero, 1 )
		{
		}

		public Timer( TimeSpan delay, TimeSpan interval )
			: this( delay, interval, 0 )
		{
		}

		public Timer( TimeSpan delay, TimeSpan interval, int count )
		{
			m_Scheduler = TimerScheduler.Instance;

			Delay = delay;
			Interval = interval;
			Count = count;

			if ( DefRegCreation )
				RegCreation();
		}

		protected virtual bool DefRegCreation => true;

		private void RegCreation()
		{
			var prof = this.GetProfile();

			prof?.RegCreation();
		}

		public override string ToString()
		{
			return GetType().FullName;
		}

		private static string FormatDelegate( Delegate callback )
		{
			if ( callback == null )
				return "null";

			return $"{callback.Method.DeclaringType.FullName}.{callback.Method.Name}";
		}

		public void Start()
		{
			if ( !Running )
			{
				Running = true;

				m_Scheduler.AddTimer( this );

				var prof = this.GetProfile();

				prof?.RegStart();
			}
		}

		public void Stop()
		{
			if ( Running )
			{
				Running = false;

				m_Scheduler.RemoveTimer( this );

				var prof = this.GetProfile();

				prof?.RegStopped();
			}
		}

		internal void Tick()
		{
			var prof = this.GetProfile();

			var start = DateTime.MinValue;

			if ( prof != null )
				start = DateTime.UtcNow;

			OnTick();

			prof?.RegTicked( DateTime.UtcNow - start );
		}

		protected virtual void OnTick()
		{
		}

		#region DelayCall(..)

		public static Timer DelayCall( TimeSpan delay, TimerCallback callback )
		{
			return DelayCall( delay, TimeSpan.Zero, 1, callback );
		}

		public static Timer DelayCall( TimeSpan delay, TimeSpan interval, TimerCallback callback )
		{
			return DelayCall( delay, interval, 0, callback );
		}

		public static Timer DelayCall( TimeSpan delay, TimeSpan interval, int count, TimerCallback callback )
		{
			Timer t = new DelayCallTimer( delay, interval, count, callback );

			t.Start();

			return t;
		}

		public static Timer DelayCall( TimeSpan delay, TimerStateCallback callback, object state )
		{
			return DelayCall( delay, TimeSpan.Zero, 1, callback, state );
		}

		public static Timer DelayCall( TimeSpan delay, TimeSpan interval, TimerStateCallback callback, object state )
		{
			return DelayCall( delay, interval, 0, callback, state );
		}

		public static Timer DelayCall( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback callback, object state )
		{
			Timer t = new DelayStateCallTimer( delay, interval, count, callback, state );

			t.Start();

			return t;
		}
		#endregion

		#region DelayCall<T>(..)
		public static Timer DelayCall<T>( TimeSpan delay, TimerStateCallback<T> callback, T state )
		{
			return DelayCall( delay, TimeSpan.Zero, 1, callback, state );
		}

		public static Timer DelayCall<T>( TimeSpan delay, TimeSpan interval, TimerStateCallback<T> callback, T state )
		{
			return DelayCall( delay, interval, 0, callback, state );
		}

		public static Timer DelayCall<T>( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback<T> callback, T state )
		{
			Timer t = new DelayStateCallTimer<T>( delay, interval, count, callback, state );

			t.Start();

			return t;
		}
		#endregion

		#region DelayCall Timers
		private class DelayCallTimer : Timer
		{
			private readonly TimerCallback m_Callback;

			protected override bool DefRegCreation => false;

			public DelayCallTimer( TimeSpan delay, TimeSpan interval, int count, TimerCallback callback )
				: base( delay, interval, count )
			{
				m_Callback = callback;
				RegCreation();
			}

			protected override void OnTick()
			{
				if ( m_Callback != null )
					m_Callback();
			}

			public override string ToString()
			{
				return $"DelayCallTimer[{FormatDelegate( m_Callback )}]";
			}
		}

		private class DelayStateCallTimer : Timer
		{
			private readonly TimerStateCallback m_Callback;
			private readonly object m_State;

			protected override bool DefRegCreation => false;

			public DelayStateCallTimer( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback callback, object state )
				: base( delay, interval, count )
			{
				m_Callback = callback;
				m_State = state;

				RegCreation();
			}

			protected override void OnTick()
			{
				m_Callback?.Invoke( m_State );
			}

			public override string ToString()
			{
				return $"DelayStateCall[{FormatDelegate( m_Callback )}]";
			}
		}

		private class DelayStateCallTimer<T> : Timer
		{
			private readonly TimerStateCallback<T> m_Callback;
			private readonly T m_State;

			protected override bool DefRegCreation => false;

			public DelayStateCallTimer( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback<T> callback, T state )
				: base( delay, interval, count )
			{
				m_Callback = callback;
				m_State = state;

				RegCreation();
			}

			protected override void OnTick()
			{
				m_Callback?.Invoke( m_State );
			}

			public override string ToString()
			{
				return $"DelayStateCall[{FormatDelegate( m_Callback )}]";
			}
		}
		#endregion
	}
}
