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
