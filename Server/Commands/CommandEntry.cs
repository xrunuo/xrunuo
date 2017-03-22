using System;

namespace Server.Commands
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
