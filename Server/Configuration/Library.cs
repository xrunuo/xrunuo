using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Server.Configuration
{
	public class Library
	{
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

		public void Load( XmlElement libConfigEl )
		{
			Depends = LowerStringArray( CollectStringArray( libConfigEl, "depends", "name" ) );

			var disabledString = libConfigEl.GetAttribute( "disabled" );
			Disabled = Parser.ParseBool( disabledString, false );

			var warnString = libConfigEl.GetAttribute( "warn" );

			if ( !string.IsNullOrEmpty( warnString ) )
				WarningLevel = int.Parse( warnString );

			BinaryPath = new FileInfo( Path.Combine( Core.BaseDirectory, "lib", string.Format( "{0}.dll", Name ) ) );
		}

		public string Name { get; }

		public FileInfo BinaryPath { get; private set; }

		public bool Exists => BinaryPath != null && BinaryPath.Exists;

		public bool Disabled { get; set; }

		public int WarningLevel { get; private set; } = -1;

		public string[] Depends { get; private set; }
	}
}
