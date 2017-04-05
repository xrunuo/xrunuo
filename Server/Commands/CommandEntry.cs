using System;

namespace Server.Commands
{
	public class CommandEntry : IComparable
	{
		public string Command { get; }

		public CommandEventHandler Handler { get; }

		public AccessLevel AccessLevel { get; }

		public CommandEntry( string command, CommandEventHandler handler, AccessLevel accessLevel )
		{
			Command = command;
			Handler = handler;
			AccessLevel = accessLevel;
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

			return Command.CompareTo( e.Command );
		}
	}
}
