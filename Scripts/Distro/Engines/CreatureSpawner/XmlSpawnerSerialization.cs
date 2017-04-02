using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using Server.Commands;
using Server.Mobiles;

namespace Server.Misc
{
	public class XmlSpawnerSerialization
	{
		public static void Initialize()
		{
			CommandSystem.Register( "DumpXmlSpawners", AccessLevel.Administrator, new CommandEventHandler( DumpXmlSpawners_OnCommand ) );
		}

		public static void DumpXmlSpawners_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Dumping spawners to xml..." );

			var spawners = World.Items.OfType<CreatureSpawner>();

			foreach ( var spawner in spawners )
			{
				DumpSpawner( spawner );
			}

			DisposeWriters();

			e.Mobile.SendMessage( string.Format( "Successfully dumped {0} spawners.", spawners.Count() ) );
		}

		private static void DisposeWriters()
		{
			foreach ( var xml in m_Writers.Values )
			{
				xml.WriteEndElement();

				xml.Close();
			}

			m_Writers.Clear();
		}

		private static Dictionary<string, XmlTextWriter> m_Writers = new Dictionary<string, XmlTextWriter>();

		private static void DumpSpawner( CreatureSpawner spawner )
		{
			if ( spawner.SpawnName == "(invalid name)" )
				return;

			var region = Region.Find( spawner.Location, spawner.Map );

			var regionName = GetRegionName( region );

			string directory = Path.Combine( Core.BaseDirectory, "Data", "Spawns", region.Map.Name );

			if ( !Directory.Exists( directory ) )
				Directory.CreateDirectory( directory );

			string filePath = Path.Combine( directory, regionName + ".xml" );

			var xml = AcquireWriter( filePath );

			spawner.XmlSerialize( xml );
		}

		private static XmlTextWriter AcquireWriter( string filePath )
		{
			XmlTextWriter xml;

			if ( m_Writers.ContainsKey( filePath ) )
				xml = m_Writers[filePath];
			else
			{
				xml = m_Writers[filePath] = new XmlTextWriter( new StreamWriter( filePath ) );

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument( true );

				xml.WriteStartElement( "Spawners" );
			}

			return xml;
		}

		private static string GetRegionName( Region region )
		{
			var name = region.Name;

			if ( !string.IsNullOrEmpty( name ) )
			{
				var ti = new CultureInfo( "en-US", false ).TextInfo;
				name = ti.ToTitleCase( name )
					.Replace( " ", string.Empty )
					.Replace( "'", string.Empty )
					.Replace( "-", string.Empty );
			}
			else
			{
				name = "World";
			}

			return name;
		}
	}
}
