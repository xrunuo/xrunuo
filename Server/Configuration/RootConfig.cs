using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Server.Configuration
{
	public class RootConfig
	{
		private string m_Filename;
		private XmlDocument m_Document;
		private string m_ServerName;
		private string m_Website;
		private string m_ServerEmail;
		private TimeSpan m_SaveInterval = TimeSpan.FromMinutes( 15.0 );
		private TimeSpan m_AccountDecay = TimeSpan.FromDays( 90.0 );
		private Features m_Features = new Features();

		private string m_BaseDirectory,
			m_ConfigDirectory,
			m_SaveDirectory,
			m_BackupDirectory,
			m_LogDirectory,
			m_CacheDirectory;

		private List<string> m_DataDirectories;
		private Network m_Network;
		private GameServerList m_GameServers;

		public RootConfig( string baseDirectory, string filename )
		{
			m_BaseDirectory = baseDirectory;
			m_ConfigDirectory = Path.Combine( m_BaseDirectory, "Data" );
			m_SaveDirectory = Path.Combine( m_BaseDirectory, "Saves" );
			m_BackupDirectory = Path.Combine( m_BaseDirectory, "Backups" );

			var baseDir = new DirectoryInfo( m_BaseDirectory );
			m_LogDirectory = baseDir.CreateSubdirectory( "log" ).FullName;
			m_CacheDirectory = baseDir.CreateSubdirectory( "cache" ).FullName;

			m_Filename = filename;

			Load();
		}

		public bool Exists
		{
			get { return File.Exists( m_Filename ); }
		}

		public string ServerName
		{
			get { return m_ServerName; }
		}

		public string Website
		{
			get { return m_Website; }
		}

		public string ServerEmail
		{
			get { return m_ServerEmail; }
		}

		public TimeSpan SaveInterval
		{
			get { return m_SaveInterval; }
		}

		public TimeSpan AccountDecay
		{
			get { return m_AccountDecay; }
		}

		public Features Features
		{
			get { return m_Features; }
		}

		public string BaseDirectory
		{
			get { return m_BaseDirectory; }
		}

		public string ConfigDirectory
		{
			get { return m_ConfigDirectory; }
		}

		public string SaveDirectory
		{
			get { return m_SaveDirectory; }
		}

		public string BackupDirectory
		{
			get { return m_BackupDirectory; }
		}

		public string LogDirectory
		{
			get { return m_LogDirectory; }
		}

		public string CacheDirectory
		{
			get { return m_CacheDirectory; }
		}

		public List<string> DataDirectories
		{
			get { return m_DataDirectories; }
		}

		public Network Network
		{
			get { return m_Network; }
		}

		public Login Login
		{
			get { return GetConfigModule<Login>(); }
		}

		public Email Email
		{
			get { return GetConfigModule<Email>(); }
		}

		public Reports Reports
		{
			get { return GetConfigModule<Reports>(); }
		}

		public GameServerList GameServers
		{
			get { return m_GameServers; }
		}

		public XmlElement GetConfiguration( string path )
		{
			XmlElement element = m_Document.DocumentElement;

			foreach ( string seg in path.Split( '/' ) )
			{
				XmlElement child = (XmlElement) element.SelectSingleNode( seg );

				if ( child == null )
				{
					child = m_Document.CreateElement( seg );
					element.AppendChild( child );
				}

				element = child;
			}

			return element;
		}

		public static void RemoveElement( XmlElement parent, string tag )
		{
			if ( parent == null )
				return;

			var nodeList = parent.GetElementsByTagName( tag );
			var children = new XmlNode[nodeList.Count];

			for ( var i = 0; i < children.Length; i++ )
				children[i] = nodeList.Item( i );

			foreach ( var child in children )
				parent.RemoveChild( child );
		}

		public static void SetElementBool( XmlElement parent, string tag, bool value )
		{
			XmlElement element;

			RemoveElement( parent, tag );

			element = parent.OwnerDocument.CreateElement( tag );
			element.SetAttribute( "value", value ? "on" : "off" );
			parent.AppendChild( element );
		}

		private void Load()
		{
			m_Document = new XmlDocument();

			m_DataDirectories = new List<string>();

			if ( Exists )
			{
				XmlTextReader reader = new XmlTextReader( m_Filename );

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
				m_Document.AppendChild( m_Document.CreateElement( "x-runuo-config" ) );
			}

			// Section "global"
			XmlElement global = GetConfiguration( "global" );

			if ( global != null )
			{
				foreach ( XmlNode node in global.ChildNodes )
				{
					if ( node.NodeType != XmlNodeType.Element )
						continue;

					XmlElement element = (XmlElement) node;

					switch ( node.Name )
					{
						case "server-name":
						{
							m_ServerName = element.GetAttribute( "value" );
							break;
						}
						case "website":
						{
							m_Website = element.GetAttribute( "value" );
							break;
						}
						case "server-email":
						{
							m_ServerEmail = element.GetAttribute( "value" );
							break;
						}
						case "multi-threading":
						{
							m_Features[node.Name] = Parser.ParseBool( element.GetAttribute( "value" ), true );
							break;
						}
						case "feature":
						{
							m_Features[element.GetAttribute( "name" )] = Parser.ParseBool( element.GetAttribute( "value" ), true );
							break;
						}
						case "save-interval":
						{
							double saveInterval = Convert.ToDouble( element.GetAttribute( "value" ) );

							if ( saveInterval < 1.0 )
								Console.WriteLine( "Warning: Invalid value of save-interval, setting it to default" );
							else
								m_SaveInterval = TimeSpan.FromMinutes( saveInterval );

							break;
						}
						case "account-decay":
						{
							m_AccountDecay = TimeSpan.FromDays( Convert.ToDouble( element.GetAttribute( "value" ) ) );

							break;
						}
						default:
						{
							Console.WriteLine( "Warning: Invalid element global/{0}", node.Name );
							break;
						}
					}
				}

				// Setup core features.
				Expansion.HS = Features["high-seas"];

				ObjectPropertyListPacket.Enabled = true;
				Mobile.InsuranceEnabled = true;

				Core.Logging = Features["logging"];

				World.ManualGC = Features["manual-gc"];
				World.DualSave = Features["dual-save"];
			}

			// Section "locations"
			XmlElement locations = GetConfiguration( "locations" );

			foreach ( XmlNode node in locations.ChildNodes )
			{
				XmlElement element = node as XmlElement;

				if ( element != null )
				{
					string path = element.InnerText;

					switch ( element.Name )
					{
						case "config-dir":
						{
							m_ConfigDirectory = path;
							break;
						}
						case "save-dir":
						{
							m_SaveDirectory = path;
							break;
						}
						case "backup-dir":
						{
							m_BackupDirectory = path;
							break;
						}
						case "data-path":
						{
							if ( Directory.Exists( path ) )
								m_DataDirectories.Add( path );

							break;
						}
						case "log-dir":
						{
							m_LogDirectory = path;
							break;
						}
						case "cache-dir":
						{
							m_CacheDirectory = path;
							break;
						}
						default:
						{
							Console.WriteLine( "Warning: Ignoring unknown location tag in {0}: {1}", m_Filename, element.Name );
							break;
						}
					}
				}
			}

			// Section "network"
			XmlElement networkEl = GetConfiguration( "network" );

			m_Network = networkEl == null
				? new Network()
				: new Network( networkEl );

			// Section "server-list"
			XmlElement serverListEl = GetConfiguration( "server-list" );

			if ( serverListEl != null )
				m_GameServers = new GameServerList( serverListEl );

			// Config modules
			foreach ( Type t in GetTypesWithAttribute( Assembly.GetExecutingAssembly(), typeof( ConfigModuleAttribute ) ) )
			{
				m_ConfigModules[t] = DeserializeModule( t );
			}
		}

		private static IEnumerable<Type> GetTypesWithAttribute( Assembly assembly, Type attributeType )
		{
			return assembly.GetTypes()
				.Where( t => t.GetCustomAttributes( attributeType, true ).Length > 0 );
		}

		private static object DeserializeModule( Type moduleType )
		{
			var serializer = new XmlSerializer( moduleType );

			var reader = new StreamReader( Path.Combine( Core.BaseDirectory, "Config", $"{moduleType.Name}.xml" ) );
			return serializer.Deserialize( reader );
		}

		private Dictionary<Type, object> m_ConfigModules = new Dictionary<Type, object>();

		private T GetConfigModule<T>() where T : class
		{
			var t = typeof( T );

			if ( m_ConfigModules.ContainsKey( t ) )
				return m_ConfigModules[t] as T;
			else
				return null;
		}

		public void Save()
		{
			string tempFilename;

			if ( m_Filename.EndsWith( ".xml" ) )
				tempFilename = m_Filename.Substring( 0, m_Filename.Length - 4 ) + ".new";
			else
				tempFilename = m_Filename + ".new";

			// Section "locations"
			XmlElement locations = GetConfiguration( "locations" );
			RemoveElement( locations, "data-path" );

			Hashtable dirHash = new Hashtable();

			foreach ( string path in m_DataDirectories )
			{
				// Check for double path.
				if ( dirHash.ContainsKey( path ) )
					continue;

				dirHash[path] = true;

				XmlElement el = m_Document.CreateElement( "data-path" );
				el.InnerText = path;
				locations.AppendChild( el );
			}

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

	public class GameServer
	{
		private string m_Name;
		private IPEndPoint m_Address;
		private bool m_SendAuthId, m_Query, m_Optional;

		public GameServer( string name, IPEndPoint address, bool sendAuthId, bool query, bool optional )
		{
			m_Name = name;
			m_Address = address;
			m_SendAuthId = sendAuthId;
			m_Query = query;
			m_Optional = optional;
		}

		public string Name
		{
			get { return m_Name; }
		}

		public IPEndPoint Address
		{
			get { return m_Address; }
		}

		public bool SendAuthID
		{
			get { return m_SendAuthId; }
		}

		public bool Query
		{
			get { return m_Query; }
		}

		public bool Optional
		{
			get { return m_Optional; }
		}
	}

	public class GameServerList : IEnumerable
	{
		private List<GameServer> m_Servers = new List<GameServer>();

		public GameServerList()
		{
		}

		public GameServerList( XmlElement element )
		{
			foreach ( XmlElement gameServerElem in element.GetElementsByTagName( "game-server" ) )
			{
				string name = Parser.GetElementString( gameServerElem, "name" );

				if ( name == null )
				{
					Console.WriteLine( "Warning: Game server without name ignored" );
					continue;
				}

				string addressString = Parser.GetElementString( gameServerElem, "address" );

				if ( addressString == null )
				{
					Console.WriteLine( "Warning: Game server without address ignored" );
					continue;
				}

				string[] splitted = addressString.Split( new char[] {':'}, 2 );

				if ( splitted.Length != 2 )
				{
					Console.WriteLine( "Warning: Game server without port ignored" );
					continue;
				}

				IPAddress ip;

				try
				{
					IPHostEntry he = Dns.GetHostEntry( splitted[0] );

					if ( he.AddressList.Length == 0 )
					{
						Console.WriteLine( "Warning: Failed to resolve {0}", splitted[0] );
						continue;
					}

					ip = he.AddressList[he.AddressList.Length - 1];
				}
				catch ( Exception e )
				{
					Console.WriteLine( String.Format( "Warning: Failed to resolve {0}", splitted[0] ), e );
					continue;
				}

				short port;

				try
				{
					port = Int16.Parse( splitted[1] );
				}
				catch
				{
					Console.WriteLine( "Warning: Invalid game server port ignored" );
					continue;
				}

				IPEndPoint address = new IPEndPoint( ip, port );

				bool sendAuthId = Parser.GetElementBool( gameServerElem, "send-auth-id", false );
				bool query = Parser.GetElementBool( gameServerElem, "query", false );
				bool optional = Parser.GetElementBool( gameServerElem, "optional", false );

				m_Servers.Add( new GameServer( name, address, sendAuthId, query, optional ) );
			}
		}

		public IEnumerator GetEnumerator()
		{
			return m_Servers.GetEnumerator();
		}

		public int Count
		{
			get { return m_Servers.Count; }
		}

		public GameServer this[ int index ]
		{
			get { return m_Servers[index]; }
		}

		public GameServer this[ string name ]
		{
			get { return m_Servers.FirstOrDefault( s => s.Name == name ); }
		}
	}
}
