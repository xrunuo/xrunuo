using System;
using System.Runtime.CompilerServices;

namespace Server.Targeting
{
	public static class AwaitableTargets
	{
		public static TargetAwaitable PickTarget( this Mobile m, int range, bool allowGround, TargetFlags flags )
		{
			return new TargetAwaitable( m, range, allowGround, flags );
		}
	}

	public class TargetAwaitable
	{
		private readonly Mobile m_Mobile;
		private readonly int m_Range;
		private readonly bool m_AllowGround;
		private readonly TargetFlags m_Flags;

		public TargetAwaitable( Mobile m, int range, bool allowGround, TargetFlags flags )
		{
			m_Mobile = m;
			m_Range = range;
			m_AllowGround = allowGround;
			m_Flags = flags;
		}

		public TargetAwaiter GetAwaiter()
		{
			return new TargetAwaiter( m_Mobile, m_Range, m_AllowGround, m_Flags );
		}
	}

	public class TargetAwaiter : INotifyCompletion
	{
		private readonly Mobile m_Mobile;
		private readonly int m_Range;
		private readonly bool m_AllowGround;
		private readonly TargetFlags m_Flags;

		private object m_Targeted;

		public TargetAwaiter( Mobile m, int range, bool allowGround, TargetFlags flags )
		{
			m_Mobile = m;
			m_Range = range;
			m_AllowGround = allowGround;
			m_Flags = flags;
		}

		public bool IsCompleted => m_Targeted != null;

		public void OnCompleted( Action continuation )
		{
			m_Mobile.BeginTarget( m_Range, m_AllowGround, m_Flags, ( from, targeted ) =>
			{
				m_Targeted = targeted;
				continuation();
			} );
		}

		public object GetResult()
		{
			return m_Targeted;
		}
	}
}
