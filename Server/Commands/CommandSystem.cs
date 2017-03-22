using System;
using System.Collections;
using System.Collections.Generic;
using Server.Events;

namespace Server.Commands
{
	public delegate void CommandEventHandler( CommandEventArgs e );

	public class CommandSystem
	{
		private static string m_CommandPrefix = ".";

		public static string CommandPrefix
		{
			get { return m_CommandPrefix; }
			set { m_CommandPrefix = value; }
		}

		public static string[] Split( string value )
		{
			char[] array = value.ToCharArray();
			List<string> list = new List<string>();

			int start = 0, end = 0;

			while ( start < array.Length )
			{
				char c = array[start];

				if ( c == '"' )
				{
					++start;
					end = start;

					while ( end < array.Length )
					{
						if ( array[end] != '"' || array[end - 1] == '\\' )
							++end;
						else
							break;
					}

					list.Add( value.Substring( start, end - start ) );

					start = end + 2;
				}
				else if ( c != ' ' )
				{
					end = start;

					while ( end < array.Length )
					{
						if ( array[end] != ' ' )
							++end;
						else
							break;
					}

					list.Add( value.Substring( start, end - start ) );

					start = end + 1;
				}
				else
				{
					++start;
				}
			}

			return list.ToArray();
		}

		private static Hashtable m_Entries;

		public static Hashtable Entries
		{
			get
			{
				return m_Entries;
			}
		}

		static CommandSystem()
		{
			m_Entries = new Hashtable( StringComparer.OrdinalIgnoreCase );
		}

		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
			Register( new string[] { command }, access, handler );
		}

		public static void Register( string[] commands, AccessLevel access, CommandEventHandler handler )
		{
			foreach ( string command in commands )
				m_Entries[command] = new CommandEntry( command, handler, access );
		}

		private static AccessLevel m_BadCommandIngoreLevel = AccessLevel.Player;

		public static AccessLevel BadCommandIgnoreLevel { get { return m_BadCommandIngoreLevel; } set { m_BadCommandIngoreLevel = value; } }

		public static bool Handle( Mobile from, string text )
		{
			if ( text.StartsWith( m_CommandPrefix ) )
			{
				text = text.Substring( m_CommandPrefix.Length );

				int indexOf = text.IndexOf( ' ' );

				string command;
				string[] args;
				string argString;

				if ( indexOf >= 0 )
				{
					argString = text.Substring( indexOf + 1 );

					command = text.Substring( 0, indexOf );
					args = Split( argString );
				}
				else
				{
					argString = "";
					command = text.ToLower();
					args = new string[0];
				}

				CommandEntry entry = (CommandEntry) m_Entries[command];

				if ( entry != null )
				{
					if ( from.AccessLevel >= entry.AccessLevel )
					{
						if ( entry.Handler != null )
						{
							CommandEventArgs e = new CommandEventArgs( from, command, argString, args );
							entry.Handler( e );
							EventSink.InvokeCommand( e );
						}
					}
					else
					{
						if ( from.AccessLevel <= m_BadCommandIngoreLevel )
							return false;

						from.SendMessage( "You do not have access to that command." );
					}
				}
				else
				{
					if ( from.AccessLevel <= m_BadCommandIngoreLevel )
						return false;

					from.SendMessage( "That is not a valid command." );
				}

				return true;
			}

			return false;
		}
	}
}
