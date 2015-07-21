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
using System.Diagnostics;
using DiagELog = System.Diagnostics.EventLog;

namespace Server
{
	public class EventLog
	{
		static EventLog()
		{
			if ( !DiagELog.SourceExists( "X-RunUO" ) )
				DiagELog.CreateEventSource( "X-RunUO", "Application" );
		}

		public static void Error( int eventID, string text )
		{
			DiagELog.WriteEntry( "X-RunUO", text, EventLogEntryType.Error, eventID );
		}

		public static void Error( int eventID, string format, params object[] args )
		{
			Error( eventID, String.Format( format, args ) );
		}

		public static void Warning( int eventID, string text )
		{
			DiagELog.WriteEntry( "X-RunUO", text, EventLogEntryType.Warning, eventID );
		}

		public static void Warning( int eventID, string format, params object[] args )
		{
			Warning( eventID, String.Format( format, args ) );
		}

		public static void Inform( int eventID, string text )
		{
			DiagELog.WriteEntry( "X-RunUO", text, EventLogEntryType.Information, eventID );
		}

		public static void Inform( int eventID, string format, params object[] args )
		{
			Inform( eventID, String.Format( format, args ) );
		}
	}
}