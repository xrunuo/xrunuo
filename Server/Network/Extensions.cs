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
using System.Linq;
using System.IO;

namespace Server.Network
{
	public static class Extensions
	{
		public static void Trace( this PacketReader reader, GameClient client )
		{
			try
			{
				using ( StreamWriter sw = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Packets.log" ), true ) )
				{
					byte[] buffer = reader.Buffer;

					if ( buffer.Length > 0 )
						sw.WriteLine( "Client: {0}: Unhandled packet 0x{1:X2}", client, buffer[0] );

					using ( MemoryStream ms = new MemoryStream( buffer ) )
						Utility.FormatBuffer( sw, ms, buffer.Length );

					sw.WriteLine();
					sw.WriteLine();
				}
			}
			catch
			{
			}
		}
	}
}
