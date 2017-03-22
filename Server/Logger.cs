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
using System.IO;

namespace Server
{
	public class Logger
	{
		public static void Error( string format, params object[] args )
		{
			string log = String.Format( format, args );

			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Exceptions.log" ), true ) )
				{
					op.WriteLine( "{0}, {1}", DateTime.UtcNow, log );
					op.WriteLine();
				}
			}
			catch
			{
			}

			Console.WriteLine( log );
		}

		public static void Debug( string format, params object[] args )
		{
			string log = String.Format( format, args );

			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Debug.log" ), true ) )
				{
					op.WriteLine( "{0}, {1}", DateTime.UtcNow, log );
				}
			}
			catch
			{
			}

			Console.WriteLine( log );
		}
	}
}
