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
		public static TargetAwaitable PickTarget( this Mobile m )
		{
			return new TargetAwaitable( m );
		}
	}

	public class TargetAwaiter : INotifyCompletion
	{
		private Mobile m_Mobile;
		private object m_Targeted;

		public TargetAwaiter( Mobile m )
		{
			m_Mobile = m;
		}

		public bool IsCompleted { get { return m_Targeted != null; } }

		public void OnCompleted( Action continuation )
		{
			m_Mobile.BeginTarget( -1, true, TargetFlags.None, ( from, targeted ) =>
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

	public class TargetAwaitable
	{
		private readonly Mobile m_Mobile;

		public TargetAwaitable(Mobile m)
		{
			m_Mobile = m;
		}

		public TargetAwaiter GetAwaiter()
		{
			return new TargetAwaiter( m_Mobile );
		}
	}
}
