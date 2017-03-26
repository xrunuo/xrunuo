//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2016 Pedro Pardal
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
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Server
{
	public class SourceCodeFileProvider
	{
		private static string[] m_IgnoreNames = new string[]
			{
				".svn", "_svn", "_darcs", ".git", ".hg", "cvs"
			};

		private Configuration.Library m_LibraryConfig;
		private string m_Type;

		public SourceCodeFileProvider( Configuration.Library libConfig, string type )
		{
			m_LibraryConfig = libConfig;
			m_Type = type;
		}

		public Dictionary<string, DateTime> ProvideSources()
		{
			var list = new Dictionary<string, DateTime>();

			ProvideSourcesRecursive( list, m_LibraryConfig.SourcePath.FullName );

			return list;
		}

		private void ProvideSourcesRecursive( Dictionary<string, DateTime> list, string path )
		{
			foreach ( var dir in Directory.GetDirectories( path ) )
			{
				var baseName = Path.GetFileName( dir ).ToLower();

				if ( m_IgnoreNames.Contains( baseName ) )
					continue;

				ProvideSourcesRecursive( list, dir );
			}

			foreach ( var filename in Directory.GetFiles( path, m_Type ) )
			{
				// Pass relative filename only.
				if ( m_LibraryConfig == null || !m_LibraryConfig.GetIgnoreSource( filename ) )
					list[filename] = File.GetLastWriteTime( filename );
			}
		}
	}
}
