using System;

namespace Server
{
	public class TimerProfile
	{
		[CommandProperty( AccessLevel.Administrator )]
		public int Created { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public int Started { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public int Stopped { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public int Ticked { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan TotalProcTime { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan PeakProcTime { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan AverageProcTime
		{
			get
			{
				if ( Ticked == 0 )
					return TimeSpan.Zero;

				return TimeSpan.FromTicks( TotalProcTime.Ticks / Ticked );
			}
		}

		public void RegCreation()
		{
			++Created;
		}

		public void RegStart()
		{
			++Started;
		}

		public void RegStopped()
		{
			++Stopped;
		}

		public void RegTicked( TimeSpan procTime )
		{
			++Ticked;
			TotalProcTime += procTime;

			if ( procTime > PeakProcTime )
				PeakProcTime = procTime;
		}

		public TimerProfile()
		{
		}
	}
}
