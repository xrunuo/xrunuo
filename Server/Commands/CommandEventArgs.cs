using System;

namespace Server.Commands
{
	public class CommandEventArgs : EventArgs
	{
		public Mobile Mobile { get; }
		public string Command { get; }
		public string ArgString { get; }
		public string[] Arguments { get; }

		public int Length => Arguments.Length;

		public string GetString( int index )
		{
			if ( index < 0 || index >= Arguments.Length )
				return "";

			return Arguments[index];
		}

		public int GetInt32( int index )
		{
			if ( index < 0 || index >= Arguments.Length )
				return 0;

			return Utility.ToInt32( Arguments[index] );
		}

		public bool GetBoolean( int index )
		{
			if ( index < 0 || index >= Arguments.Length )
				return false;

			return Utility.ToBoolean( Arguments[index] );
		}

		public double GetDouble( int index )
		{
			if ( index < 0 || index >= Arguments.Length )
				return 0.0;

			return Utility.ToDouble( Arguments[index] );
		}

		public TimeSpan GetTimeSpan( int index )
		{
			if ( index < 0 || index >= Arguments.Length )
				return TimeSpan.Zero;

			return Utility.ToTimeSpan( Arguments[index] );
		}

		public CommandEventArgs( Mobile mobile, string command, string argString, string[] arguments )
		{
			Mobile = mobile;
			Command = command;
			ArgString = argString;
			Arguments = arguments;
		}
	}
}
