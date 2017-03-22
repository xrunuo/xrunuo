using System;

namespace Server.Targeting
{
	public static class MobileExtensions
	{
		private class SimpleTarget : Target
		{
			private TargetCallback m_Callback;

			public SimpleTarget( int range, TargetFlags flags, bool allowGround, TargetCallback callback )
				: base( range, allowGround, flags )
			{
				m_Callback = callback;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Callback != null )
					m_Callback( from, targeted );
			}
		}

		private class SimpleStateTarget : Target
		{
			private TargetStateCallback m_Callback;
			private object m_State;

			public SimpleStateTarget( int range, TargetFlags flags, bool allowGround, TargetStateCallback callback, object state )
				: base( range, allowGround, flags )
			{
				m_Callback = callback;
				m_State = state;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Callback != null )
					m_Callback( from, targeted, m_State );
			}
		}

		private class GenericStateTarget<T> : Target
		{
			private TargetStateCallback<T> m_Callback;
			private T m_State;

			public GenericStateTarget( int range, TargetFlags flags, bool allowGround, TargetStateCallback<T> callback, T state )
				: base( range, allowGround, flags )
			{
				m_Callback = callback;
				m_State = state;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Callback != null )
					m_Callback( from, targeted, m_State );
			}
		}

		public static Target BeginTarget( this Mobile m, int range, bool allowGround, TargetFlags flags, TargetCallback callback )
		{
			Target t = new SimpleTarget( range, flags, allowGround, callback );

			m.Target = t;

			return t;
		}

		public static Target BeginTarget( this Mobile m, int range, bool allowGround, TargetFlags flags, TargetStateCallback callback, object state )
		{
			Target t = new SimpleStateTarget( range, flags, allowGround, callback, state );

			m.Target = t;

			return t;
		}

		public static Target BeginTarget<T>( this Mobile m, int range, bool allowGround, TargetFlags flags, TargetStateCallback<T> callback, T state )
		{
			Target t = new GenericStateTarget<T>( range, flags, allowGround, callback, state );

			m.Target = t;

			return t;
		}
	}
}
