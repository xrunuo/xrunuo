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
using System.Xml;

namespace Server.Configuration
{
	public class Parser
	{
		public static bool ParseBool( string value, bool defaultValue )
		{
			if ( value == null || value == "" )
				return defaultValue;

			if ( value == "true" || value == "on" || value == "yes" )
				return true;

			if ( value == "false" || value == "off" || value == "no" )
				return false;

			throw new FormatException( "Cannot parse boolean value" );
		}

		public static string GetElementString( XmlElement parent, string tag )
		{
			XmlNodeList nodeList = parent.GetElementsByTagName( tag );

			if ( nodeList.Count == 0 )
				return null;

			return nodeList[0].InnerText;
		}

		public static string GetElementString( XmlElement parent, string tag, string defaultValue )
		{
			if ( parent == null )
				return defaultValue;

			XmlNodeList nodeList = parent.GetElementsByTagName( tag );

			if ( nodeList.Count == 0 )
				return defaultValue;

			return ( (XmlElement) nodeList[0] ).GetAttribute( "value" );
		}

		public static bool GetElementBool( XmlElement parent, string tag, bool defaultValue )
		{
			if ( parent == null )
				return defaultValue;

			XmlNodeList nodeList = parent.GetElementsByTagName( tag );

			if ( nodeList.Count == 0 )
				return defaultValue;

			string value = ( (XmlElement) nodeList[0] ).GetAttribute( "value" );

			return ParseBool( value, true );
		}

		public static int GetElementInt( XmlElement parent, string tag, int defaultValue )
		{
			if ( parent == null )
				return defaultValue;

			XmlNodeList nodeList = parent.GetElementsByTagName( tag );

			if ( nodeList.Count == 0 )
				return defaultValue;

			string value = ( (XmlElement) nodeList[0] ).GetAttribute( "value" );

			try
			{
				return Int32.Parse( value );
			}
			catch
			{
				return defaultValue;
			}
		}
	}
}
