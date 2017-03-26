using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Configuration
{
	public class LibraryConfig
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private string m_BaseDirectory;
		private string m_Filename;
		private XmlDocument m_Document;

		private Dictionary<string, Library> m_LibraryConfig = new Dictionary<string, Library>();

		public LibraryConfig( string baseDirectory, string filename )
		{
			m_BaseDirectory = baseDirectory;
			m_Filename = filename;

			Defaults();
			Load();
		}

		public bool Exists
		{
			get { return File.Exists( m_Filename ); }
		}

		public Library GetLibrary( string name )
		{
			return m_LibraryConfig[name];
		}

		public ICollection<Library> Libraries
		{
			get { return m_LibraryConfig.Values; }
		}

		private void Defaults()
		{
			var coreConfig = new Library( "Core" );
			m_LibraryConfig["Core"] = coreConfig;

			var baseDirectory = new DirectoryInfo( m_BaseDirectory );

			// Find binary libraries in ./lib/
			var lib = baseDirectory.CreateSubdirectory( "lib" );

			foreach ( var libFile in lib.GetFiles( "*.dll" ) )
			{
				var fileName = libFile.Name;
				var libName = fileName.Substring( 0, fileName.Length - 4 );

				if ( m_LibraryConfig.ContainsKey( libName ) )
				{
					log.Warning( "Duplicate library '{0}' in '{1}'", libName, libFile );
					continue;
				}

				m_LibraryConfig[libName] = new Library( libName, libFile );
			}

			// Find source libraries in ./Scripts/
			var scriptsDir = baseDirectory.CreateSubdirectory( "Scripts" );

			foreach ( var sub in scriptsDir.GetDirectories() )
			{
				var libName = sub.Name;

				if ( m_LibraryConfig.ContainsKey( libName ) )
				{
					log.Warning( "Duplicate library '{0}' in '{1}'", libName, sub.FullName );
					continue;
				}

				m_LibraryConfig[libName] = new Library( libName, sub );
			}
		}

		private void Load()
		{
			m_Document = new XmlDocument();

			if ( Exists )
			{
				var reader = new XmlTextReader( m_Filename );

				try
				{
					m_Document.Load( reader );
				}
				finally
				{
					reader.Close();
				}
			}
			else
			{
				m_Document.AppendChild( m_Document.CreateElement( "libraries" ) );
			}

			var librariesEl = m_Document.DocumentElement;

			foreach ( XmlElement element in librariesEl.GetElementsByTagName( "library" ) )
			{
				var name = element.GetAttribute( "name" );

				if ( string.IsNullOrEmpty( name ) )
				{
					log.Warning( "library element without name attribute" );
					continue;
				}

				Library libConfig;

				if ( m_LibraryConfig.ContainsKey( name ) )
					libConfig = m_LibraryConfig[name];
				else
					libConfig = m_LibraryConfig[name] = new Library( name );

				libConfig.Load( element );
			}

			if ( !m_LibraryConfig.ContainsKey( "Distro" ) )
				m_LibraryConfig["Distro"] = new Library( "Distro" );
		}

		public void Save()
		{
			string tempFilename;

			if ( m_Filename.EndsWith( ".xml" ) )
				tempFilename = m_Filename.Substring( 0, m_Filename.Length - 4 ) + ".new";
			else
				tempFilename = m_Filename + ".new";

			// Write to file.
			XmlTextWriter writer = new XmlTextWriter( tempFilename, System.Text.Encoding.UTF8 );
			writer.Formatting = Formatting.Indented;

			try
			{
				m_Document.Save( writer );
				writer.Close();
				File.Delete( m_Filename );
				File.Move( tempFilename, m_Filename );
			}
			catch
			{
				writer.Close();
				File.Delete( tempFilename );
				throw;
			}
		}
	}
}
