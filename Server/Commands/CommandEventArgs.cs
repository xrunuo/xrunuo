using System;

namespace Server.Commands
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
