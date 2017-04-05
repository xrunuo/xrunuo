using System;
using System.Collections;
using System.Collections.Generic;
using Server.Events;

namespace Server.Commands
{
	public delegate void CommandEventHandler( CommandEventArgs e );

	public class CommandSystem
	{
		public static string CommandPrefix { get; set; } = ".";

		public static string[] Split( string value )
		{
			var array = value.ToCharArray();
			var list = new List<string>();

			int start = 0, end = 0;

			while ( start < array.Length )
			{
				var c = array[start];

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

		public static Hashtable Entries { get; }

		static CommandSystem()
		{
			Entries = new Hashtable( StringComparer.OrdinalIgnoreCase );
		}

		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
			Register( new[] { command }, access, handler );
		}

		public static void Register( string[] commands, AccessLevel access, CommandEventHandler handler )
		{
			foreach ( var command in commands )
				Entries[command] = new CommandEntry( command, handler, access );
		}

		public static AccessLevel BadCommandIgnoreLevel { get; set; } = AccessLevel.Player;

		public static bool Handle( Mobile from, string text )
		{
			if ( text.StartsWith( CommandPrefix ) )
			{
				text = text.Substring( CommandPrefix.Length );

				var indexOf = text.IndexOf( ' ' );

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

				var entry = (CommandEntry) Entries[command];

				if ( entry != null )
				{
					if ( from.AccessLevel >= entry.AccessLevel )
					{
						if ( entry.Handler != null )
						{
							var e = new CommandEventArgs( from, command, argString, args );
							entry.Handler( e );
							EventSink.InvokeCommand( e );
						}
					}
					else
					{
						if ( from.AccessLevel <= BadCommandIgnoreLevel )
							return false;

						from.SendMessage( "You do not have access to that command." );
					}
				}
				else
				{
					if ( from.AccessLevel <= BadCommandIgnoreLevel )
						return false;

					from.SendMessage( "That is not a valid command." );
				}

				return true;
			}

			return false;
		}
	}
}
