using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Server.Configuration
{
	public class LibraryConfig
	{
		private static readonly ILog log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private readonly string m_BaseDirectory;
		private readonly string m_Filename;
		private XmlDocument m_Document;

		private readonly Dictionary<string, Library> m_LibraryConfig = new Dictionary<string, Library>();

		public LibraryConfig( string baseDirectory, string filename )
		{
			m_BaseDirectory = baseDirectory;
			m_Filename = filename;

			Defaults();
			Load();
		}

		public bool Exists => File.Exists( m_Filename );

		public Library GetLibrary( string name )
		{
			return m_LibraryConfig[name];
		}

		public ICollection<Library> Libraries => m_LibraryConfig.Values;

		private void Defaults()
		{
			var coreConfig = new Library( "Core" );
			m_LibraryConfig["Core"] = coreConfig;
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
			var writer = new XmlTextWriter( tempFilename, Encoding.UTF8 );
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
