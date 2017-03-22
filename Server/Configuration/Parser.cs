using System;
using System.Xml;

namespace Server.Configuration
{
	public class Parser
	{
		public static bool ParseBool( string value, bool defaultValue )
		{
			if ( string.IsNullOrEmpty( value ) )
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
