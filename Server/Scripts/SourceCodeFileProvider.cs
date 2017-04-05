using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Server
{
	public class SourceCodeFileProvider
	{
		private static readonly string[] m_IgnoreNames = {
				".svn", "_svn", "_darcs", ".git", ".hg", "cvs"
			};

		private readonly Configuration.Library m_LibraryConfig;
		private readonly string m_Type;

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
