﻿using System;
using System.Runtime.InteropServices;

namespace Server.Profiler
{
	public struct MainProfile
	{
		public enum TimerId : int
		{
			Idle,
			MobileDelta,
			ItemDelta,
			Timers,
			Network,
			Count
		}

		private DateTime m_Start;
		private ulong m_Iterations;

		[MarshalAs( UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I8, SizeConst = (int) TimerId.Count )]
		private TimeSpan[] m_Timers;

		public MainProfile( DateTime start )
		{
			m_Start = start;
			m_Iterations = 0;

			m_Timers = new TimeSpan[(int) TimerId.Count];
			for ( int i = 0; i < (int) TimerId.Count; i++ )
				m_Timers[i] = TimeSpan.Zero;
		}

		public DateTime Start
		{
			get { return m_Start; }
		}

		public ulong Iterations
		{
			get { return m_Iterations; }
		}

		public TimeSpan Timer( TimerId id )
		{
			if ( id < 0 || id >= TimerId.Count )
				return TimeSpan.Zero;

			return m_Timers[(int) id];
		}

		public void Add( TimerId id, TimeSpan diff )
		{
			m_Timers[(int) id] += diff;
		}

		public void Next()
		{
			m_Iterations++;
		}
	}
}
