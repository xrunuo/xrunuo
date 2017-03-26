//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2017 Pedro Pardal
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
using System.IO.Compression;
using System.Net;

namespace Server
{
	public class Dependencies
	{
		public static string Fetch( Configuration.Library library )
		{
			string targetPath;

			Console.Write( "Dependencies: Fetching library " + library.Name + " from " + library.Uri + "..." );

			using ( var client = new WebClient() )
			{
				var zipPath = Path.Combine( Core.BaseDirectory, $"deps/{library.Name}.zip" );

				if ( !File.Exists( zipPath ) )
				{
					client.DownloadFile( library.Uri, zipPath );
				}

				targetPath = $"Scripts/{library.Name}";

				if ( Directory.Exists( targetPath ) )
					Directory.Delete( targetPath, recursive: true );

				ZipFile.ExtractToDirectory( zipPath, targetPath );
			}

			Console.WriteLine( "done!" );

			return targetPath;
		}
	}
}
