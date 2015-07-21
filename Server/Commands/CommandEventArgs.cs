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

namespace Server
{
	public class CommandEventArgs : EventArgs
	{
		private Mobile m_Mobile;
		private string m_Command, m_ArgString;
		private string[] m_Arguments;

		public Mobile Mobile
		{
			get
			{
				return m_Mobile;
			}
		}

		public string Command
		{
			get
			{
				return m_Command;
			}
		}

		public string ArgString
		{
			get
			{
				return m_ArgString;
			}
		}

		public string[] Arguments
		{
			get
			{
				return m_Arguments;
			}
		}

		public int Length
		{
			get
			{
				return m_Arguments.Length;
			}
		}

		public string GetString( int index )
		{
			if ( index < 0 || index >= m_Arguments.Length )
				return "";

			return m_Arguments[index];
		}

		public int GetInt32( int index )
		{
			if ( index < 0 || index >= m_Arguments.Length )
				return 0;

			return Utility.ToInt32( m_Arguments[index] );
		}

		public bool GetBoolean( int index )
		{
			if ( index < 0 || index >= m_Arguments.Length )
				return false;

			return Utility.ToBoolean( m_Arguments[index] );
		}

		public double GetDouble( int index )
		{
			if ( index < 0 || index >= m_Arguments.Length )
				return 0.0;

			return Utility.ToDouble( m_Arguments[index] );
		}

		public TimeSpan GetTimeSpan( int index )
		{
			if ( index < 0 || index >= m_Arguments.Length )
				return TimeSpan.Zero;

			return Utility.ToTimeSpan( m_Arguments[index] );
		}

		public CommandEventArgs( Mobile mobile, string command, string argString, string[] arguments )
		{
			m_Mobile = mobile;
			m_Command = command;
			m_ArgString = argString;
			m_Arguments = arguments;
		}
	}
}
