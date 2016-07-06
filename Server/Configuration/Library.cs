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
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Server.Configuration
{
	public class Library
	{
		private string m_Name;
		private DirectoryInfo m_SourcePath;
		private FileInfo m_BinaryPath;
		private bool m_Disabled = false;
		private string[] m_IgnoreSources;
		private string[] m_IgnoreTypes;
		private string[] m_Depends;
		private int m_WarningLevel = -1;

		private static string[] CollectStringArray( XmlElement parent, string tag, string attr )
		{
			List<string> attributeList = new List<string>();

			foreach ( XmlElement element in parent.GetElementsByTagName( tag ) )
			{
				string value = element.GetAttribute( attr );

				if ( value != null && value != "" )
					attributeList.Add( value );
			}

			return attributeList.Count == 0 ? null : attributeList.ToArray();
		}

		private static string[] LowerStringArray( string[] src )
		{
			if ( src == null )
				return null;

			string[] dst = new string[src.Length];

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

		public void Load( XmlElement libConfigEl )
		{
			string sourcePathString = Parser.GetElementString( libConfigEl, "path" );

			if ( sourcePathString != null )
			{
				if ( sourcePathString.EndsWith( ".dll" ) )
				{
					m_SourcePath = null;
					m_BinaryPath = new FileInfo( sourcePathString );
				}
				else
				{
					m_SourcePath = new DirectoryInfo( sourcePathString );
					m_BinaryPath = null;
				}
			}

			m_IgnoreSources = CollectStringArray( libConfigEl, "ignore-source", "name" );
			m_IgnoreTypes = CollectStringArray( libConfigEl, "ignore-type", "name" );
			m_Depends = LowerStringArray( CollectStringArray( libConfigEl, "depends", "name" ) );

			string disabledString = libConfigEl.GetAttribute( "disabled" );
			m_Disabled = Parser.ParseBool( disabledString, false );

			string warnString = libConfigEl.GetAttribute( "warn" );

			if ( warnString != null && warnString != "" )
				m_WarningLevel = Int32.Parse( warnString );
		}

		public string Name
		{
			get { return m_Name; }
		}

		public DirectoryInfo SourcePath
		{
			get { return m_SourcePath; }
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
			if ( m_IgnoreSources == null )
				return false;

			foreach ( string ign in m_IgnoreSources )
				if ( filename.EndsWith( ign ) )
					return true;

			return false;
		}

		public bool GetIgnoreType( Type type )
		{
			if ( m_IgnoreTypes == null )
				return false;

			foreach ( string ign in m_IgnoreTypes )
				if ( ign == type.FullName )
					return true;

			return false;
		}
	}
}