//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2017 Pedro Pardal
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
		private Mobile m_Mobile;
		private int m_Range;
		private bool m_AllowGround;
		private TargetFlags m_Flags;
		
		private object m_Targeted;

		public TargetAwaiter( Mobile m, int range, bool allowGround, TargetFlags flags )
		{
			m_Mobile = m;
			m_Range = range;
			m_AllowGround = allowGround;
			m_Flags = flags;
		}

		public bool IsCompleted { get { return m_Targeted != null; } }

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
