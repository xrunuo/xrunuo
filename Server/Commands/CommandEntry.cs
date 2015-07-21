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
	public class CommandEntry : IComparable
	{
		private string m_Command;
		private CommandEventHandler m_Handler;
		private AccessLevel m_AccessLevel;

		public string Command
		{
			get
			{
				return m_Command;
			}
		}

		public CommandEventHandler Handler
		{
			get
			{
				return m_Handler;
			}
		}

		public AccessLevel AccessLevel
		{
			get
			{
				return m_AccessLevel;
			}
		}

		public CommandEntry( string command, CommandEventHandler handler, AccessLevel accessLevel )
		{
			m_Command = command;
			m_Handler = handler;
			m_AccessLevel = accessLevel;
		}

		public int CompareTo( object obj )
		{
			if ( obj == this )
				return 0;
			else if ( obj == null )
				return 1;

			CommandEntry e = obj as CommandEntry;

			if ( e == null )
				throw new ArgumentException();

			return m_Command.CompareTo( e.m_Command );
		}
	}
}
