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
	public delegate void TimerCallback();
	public delegate void TimerStateCallback( object state );
	public delegate void TimerStateCallback<T>( T state );

	public class Timer
	{
		private readonly TimerScheduler m_Scheduler;

		private TimeSpan m_Delay;
		private TimeSpan m_Interval;
		private int m_Count;
		private bool m_Running;

		internal DateTime m_Next;
		internal bool m_Queued;
		internal int m_Index;
		internal List<Timer> m_List;

		public DateTime Next
		{
			get { return m_Next; }
		}

		public TimeSpan Delay
		{
			get { return m_Delay; }
			set { m_Delay = value; }
		}

		public TimeSpan Interval
		{
			get { return m_Interval; }
			set { m_Interval = value; }
		}

		public int Count
		{
			get { return m_Count; }
		}

		public bool Running
		{
			get { return m_Running; }
		}

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

			m_Delay = delay;
			m_Interval = interval;
			m_Count = count;

			if ( DefRegCreation )
				RegCreation();
		}

		protected virtual bool DefRegCreation
		{
			get { return true; }
		}

		private void RegCreation()
		{
			TimerProfile prof = this.GetProfile();

			if ( prof != null )
				prof.RegCreation();
		}

		public override string ToString()
		{
			return GetType().FullName;
		}

		private static string FormatDelegate( Delegate callback )
		{
			if ( callback == null )
				return "null";

			return String.Format( "{0}.{1}", callback.Method.DeclaringType.FullName, callback.Method.Name );
		}

		public void Start()
		{
			if ( !m_Running )
			{
				m_Running = true;

				m_Scheduler.AddTimer( this );

				TimerProfile prof = this.GetProfile();

				if ( prof != null )
					prof.RegStart();
			}
		}

		public void Stop()
		{
			if ( m_Running )
			{
				m_Running = false;

				m_Scheduler.RemoveTimer( this );

				TimerProfile prof = this.GetProfile();

				if ( prof != null )
					prof.RegStopped();
			}
		}

		internal void Tick()
		{
			TimerProfile prof = this.GetProfile();

			DateTime start = DateTime.MinValue;

			if ( prof != null )
				start = DateTime.Now;

			OnTick();

			if ( prof != null )
				prof.RegTicked( DateTime.Now - start );
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

			protected override bool DefRegCreation { get { return false; } }

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
				return String.Format( "DelayCallTimer[{0}]", FormatDelegate( m_Callback ) );
			}
		}

		private class DelayStateCallTimer : Timer
		{
			private readonly TimerStateCallback m_Callback;
			private readonly object m_State;

			protected override bool DefRegCreation { get { return false; } }

			public DelayStateCallTimer( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback callback, object state )
				: base( delay, interval, count )
			{
				m_Callback = callback;
				m_State = state;

				RegCreation();
			}

			protected override void OnTick()
			{
				if ( m_Callback != null )
					m_Callback( m_State );
			}

			public override string ToString()
			{
				return String.Format( "DelayStateCall[{0}]", FormatDelegate( m_Callback ) );
			}
		}

		private class DelayStateCallTimer<T> : Timer
		{
			private readonly TimerStateCallback<T> m_Callback;
			private readonly T m_State;

			public TimerStateCallback<T> Callback { get { return m_Callback; } }

			protected override bool DefRegCreation { get { return false; } }

			public DelayStateCallTimer( TimeSpan delay, TimeSpan interval, int count, TimerStateCallback<T> callback, T state )
				: base( delay, interval, count )
			{
				m_Callback = callback;
				m_State = state;

				RegCreation();
			}

			protected override void OnTick()
			{
				if ( m_Callback != null )
					m_Callback( m_State );
			}

			public override string ToString()
			{
				return String.Format( "DelayStateCall[{0}]", FormatDelegate( m_Callback ) );
			}
		}
		#endregion
	}
}