using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Server.Configuration
{
	public class Library
	{
		private string m_Name;
		private DirectoryInfo m_SourcePath;
		private FileInfo m_BinaryPath;
		private Uri m_Uri;
		private bool m_Disabled = false;
		private string[] m_IgnoreSources;
		private string[] m_IgnoreTypes;
		private string[] m_Depends;
		private int m_WarningLevel = -1;

		private static string[] CollectStringArray( XmlElement parent, string tag, string attr )
		{
			var attributeList = new List<string>();

			foreach ( XmlElement element in parent.GetElementsByTagName( tag ) )
			{
				var value = element.GetAttribute( attr );

				if ( !string.IsNullOrEmpty( value ) )
					attributeList.Add( value );
			}

			return attributeList.Count == 0 ? null : attributeList.ToArray();
		}

		private static string[] LowerStringArray( string[] src )
		{
			if ( src == null )
				return null;

			var dst = new string[src.Length];

			for ( uint i = 0; i < src.Length; i++ )
				dst[i] = src[i].ToLower();

			return dst;
		}

		public Library( string name )
		{
			m_Name = name;
		}

		public Library( string name, DirectoryInfo path )
		{
			m_Name = name;
			m_SourcePath = path;
		}

		public Library( string name, FileInfo path )
		{
			m_Name = name;
			m_BinaryPath = path;
		}

		public Library( string name, Uri uri )
		{
			m_Name = name;
			m_Uri = uri;
		}

		public void Load( XmlElement libConfigEl )
		{
			var sourceString = Parser.GetElementString( libConfigEl, "path" );

			if ( sourceString != null )
			{
				if ( sourceString.StartsWith( "http" ) )
				{
					m_BinaryPath = null;
					m_Uri = new Uri( sourceString );
				}
				else if ( sourceString.EndsWith( ".dll" ) )
				{
					m_SourcePath = null;
					m_BinaryPath = new FileInfo( sourceString );
					m_Uri = null;
				}
				else
				{
					m_SourcePath = new DirectoryInfo( sourceString );
					m_BinaryPath = null;
					m_Uri = null;
				}
			}

			m_IgnoreSources = CollectStringArray( libConfigEl, "ignore-source", "name" );
			m_IgnoreTypes = CollectStringArray( libConfigEl, "ignore-type", "name" );
			m_Depends = LowerStringArray( CollectStringArray( libConfigEl, "depends", "name" ) );

			var disabledString = libConfigEl.GetAttribute( "disabled" );
			m_Disabled = Parser.ParseBool( disabledString, false );

			var warnString = libConfigEl.GetAttribute( "warn" );

			if ( !string.IsNullOrEmpty( warnString ) )
				m_WarningLevel = int.Parse( warnString );
		}

		public string Name
		{
			get { return m_Name; }
		}

		public DirectoryInfo SourcePath
		{
			get { return m_SourcePath; }
			set { m_SourcePath = value; }
		}

		public FileInfo BinaryPath
		{
			get { return m_BinaryPath; }
		}

		public bool Exists
		{
			get
			{
				return ( m_SourcePath != null && m_SourcePath.Exists ) ||
				       ( m_BinaryPath != null && m_BinaryPath.Exists );
			}
		}

		public bool Disabled
		{
			get { return m_Disabled; }
			set { m_Disabled = value; }
		}

		public int WarningLevel
		{
			get { return m_WarningLevel; }
		}

		public string[] Depends
		{
			get { return m_Depends; }
		}

		public bool GetIgnoreSource( string filename )
		{
			return m_IgnoreSources != null && m_IgnoreSources.Any( filename.EndsWith );
		}

		public bool GetIgnoreType( Type type )
		{
			return m_IgnoreTypes != null && m_IgnoreTypes.Any( t => t == type.FullName );
		}

		public Uri Uri
		{
			get { return m_Uri; }
		}

		public bool IsRemote
		{
			get { return Uri != null; }
		}
	}
}
