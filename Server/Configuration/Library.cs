using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Server.Configuration
{
	public class Library
	{
		private string[] m_IgnoreSources;
		private string[] m_IgnoreTypes;

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
			Name = name;
		}

		public Library( string name, DirectoryInfo path )
		{
			Name = name;
			SourcePath = path;
		}

		public Library( string name, FileInfo path )
		{
			Name = name;
			BinaryPath = path;
		}

		public Library( string name, Uri uri )
		{
			Name = name;
			Uri = uri;
		}

		public void Load( XmlElement libConfigEl )
		{
			var sourceString = Parser.GetElementString( libConfigEl, "path" );

			if ( sourceString != null )
			{
				if ( sourceString.StartsWith( "http" ) )
				{
					BinaryPath = null;
					Uri = new Uri( sourceString );
				}
				else if ( sourceString.EndsWith( ".dll" ) )
				{
					SourcePath = null;
					BinaryPath = new FileInfo( sourceString );
					Uri = null;
				}
				else
				{
					SourcePath = new DirectoryInfo( sourceString );
					BinaryPath = null;
					Uri = null;
				}
			}

			m_IgnoreSources = CollectStringArray( libConfigEl, "ignore-source", "name" );
			m_IgnoreTypes = CollectStringArray( libConfigEl, "ignore-type", "name" );
			Depends = LowerStringArray( CollectStringArray( libConfigEl, "depends", "name" ) );

			var disabledString = libConfigEl.GetAttribute( "disabled" );
			Disabled = Parser.ParseBool( disabledString, false );

			var warnString = libConfigEl.GetAttribute( "warn" );

			if ( !string.IsNullOrEmpty( warnString ) )
				WarningLevel = int.Parse( warnString );
		}

		public string Name { get; }

		public DirectoryInfo SourcePath { get; set; }

		public FileInfo BinaryPath { get; private set; }

		public bool Exists => ( SourcePath != null && SourcePath.Exists ) ||
		                      ( BinaryPath != null && BinaryPath.Exists );

		public bool Disabled { get; set; }

		public int WarningLevel { get; private set; } = -1;

		public string[] Depends { get; private set; }

		public bool GetIgnoreSource( string filename )
		{
			return m_IgnoreSources != null && m_IgnoreSources.Any( filename.EndsWith );
		}

		public bool GetIgnoreType( Type type )
		{
			return m_IgnoreTypes != null && m_IgnoreTypes.Any( t => t == type.FullName );
		}

		public Uri Uri { get; private set; }

		public bool IsRemote => Uri != null;
	}
}
