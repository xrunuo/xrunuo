using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Server.Configuration
{
	public class RootConfig
	{
		private static readonly ILog log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private readonly string m_Filename;
		private XmlDocument m_Document;

		public RootConfig( string baseDirectory, string filename )
		{
			BaseDirectory = baseDirectory;
			ConfigDirectory = Path.Combine( BaseDirectory, "Data" );
			SaveDirectory = Path.Combine( BaseDirectory, "Saves" );
			BackupDirectory = Path.Combine( BaseDirectory, "Backups" );

			var baseDir = new DirectoryInfo( BaseDirectory );
			LogDirectory = baseDir.CreateSubdirectory( "log" ).FullName;

			m_Filename = filename;

			Load();
		}

		public bool Exists => File.Exists( m_Filename );

		public string ServerName { get; private set; }
		public string Website { get; private set; }
		public string ServerEmail { get; private set; }
		public TimeSpan SaveInterval { get; private set; } = TimeSpan.FromMinutes( 15.0 );
		public TimeSpan AccountDecay { get; private set; } = TimeSpan.FromDays( 90.0 );
		public Features Features { get; } = new Features();
		public string BaseDirectory { get; }
		public string ConfigDirectory { get; private set; }
		public string SaveDirectory { get; private set; }
		public string BackupDirectory { get; private set; }
		public string LogDirectory { get; private set; }
		public List<string> DataDirectories { get; private set; }
		public Network Network { get; private set; }

		public Login Login => GetConfigModule<Login>();
		public Reports Reports => GetConfigModule<Reports>();
		public GameServerList GameServers { get; private set; }

		public XmlElement GetConfiguration( string path )
		{
			var element = m_Document.DocumentElement;

			foreach ( var seg in path.Split( '/' ) )
			{
				var child = (XmlElement) element.SelectSingleNode( seg );

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

			DataDirectories = new List<string>();

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
				m_Document.AppendChild( m_Document.CreateElement( "x-runuo-config" ) );
			}

			// Section "global"
			var global = GetConfiguration( "global" );

			if ( global != null )
			{
				foreach ( XmlNode node in global.ChildNodes )
				{
					if ( node.NodeType != XmlNodeType.Element )
						continue;

					var element = (XmlElement) node;

					switch ( node.Name )
					{
						case "server-name":
						{
							ServerName = element.GetAttribute( "value" );
							break;
						}
						case "website":
						{
							Website = element.GetAttribute( "value" );
							break;
						}
						case "server-email":
						{
							ServerEmail = element.GetAttribute( "value" );
							break;
						}
						case "multi-threading":
						{
							Features[node.Name] = Parser.ParseBool( element.GetAttribute( "value" ), true );
							break;
						}
						case "feature":
						{
							Features[element.GetAttribute( "name" )] = Parser.ParseBool( element.GetAttribute( "value" ), true );
							break;
						}
						case "save-interval":
						{
							var saveInterval = Convert.ToDouble( element.GetAttribute( "value" ) );

							if ( saveInterval < 1.0 )
								log.Warning( "Invalid value of save-interval, setting it to default" );
							else
								SaveInterval = TimeSpan.FromMinutes( saveInterval );

							break;
						}
						case "account-decay":
						{
							AccountDecay = TimeSpan.FromDays( Convert.ToDouble( element.GetAttribute( "value" ) ) );

							break;
						}
						default:
						{
							log.Warning( "Invalid element global/{0}", node.Name );
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
			var locations = GetConfiguration( "locations" );

			foreach ( XmlNode node in locations.ChildNodes )
			{
				var element = node as XmlElement;

				if ( element != null )
				{
					var path = element.InnerText;

					switch ( element.Name )
					{
						case "config-dir":
						{
							ConfigDirectory = path;
							break;
						}
						case "save-dir":
						{
							SaveDirectory = path;
							break;
						}
						case "backup-dir":
						{
							BackupDirectory = path;
							break;
						}
						case "data-path":
						{
							if ( Directory.Exists( path ) )
								DataDirectories.Add( path );

							break;
						}
						case "log-dir":
						{
							LogDirectory = path;
							break;
						}
						default:
						{
							log.Warning( "Ignoring unknown location tag in {0}: {1}", m_Filename, element.Name );
							break;
						}
					}
				}
			}

			// Section "network"
			var networkEl = GetConfiguration( "network" );

			Network = networkEl == null
				? new Network()
				: new Network( networkEl );

			// Section "server-list"
			var serverListEl = GetConfiguration( "server-list" );

			if ( serverListEl != null )
				GameServers = new GameServerList( serverListEl );

			// Config modules
			foreach ( var t in GetTypesWithAttribute( Assembly.GetExecutingAssembly(), typeof( ConfigModuleAttribute ) ) )
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

		private readonly Dictionary<Type, object> m_ConfigModules = new Dictionary<Type, object>();

		private T GetConfigModule<T>() where T : class
		{
			var t = typeof( T );

			if ( m_ConfigModules.ContainsKey( t ) )
				return m_ConfigModules[t] as T;

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
			var locations = GetConfiguration( "locations" );
			RemoveElement( locations, "data-path" );

			var dirHash = new Hashtable();

			foreach ( var path in DataDirectories )
			{
				// Check for double path.
				if ( dirHash.ContainsKey( path ) )
					continue;

				dirHash[path] = true;

				var el = m_Document.CreateElement( "data-path" );
				el.InnerText = path;
				locations.AppendChild( el );
			}

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

	public class GameServer
	{
		public GameServer( string name, IPEndPoint address, bool sendAuthId, bool query, bool optional )
		{
			Name = name;
			Address = address;
			SendAuthID = sendAuthId;
			Query = query;
			Optional = optional;
		}

		public string Name { get; }

		public IPEndPoint Address { get; }

		public bool SendAuthID { get; }

		public bool Query { get; }

		public bool Optional { get; }
	}

	public class GameServerList : IEnumerable
	{
		private static readonly ILog log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private readonly List<GameServer> m_Servers = new List<GameServer>();

		public GameServerList()
		{
		}

		public GameServerList( XmlElement element )
		{
			foreach ( XmlElement gameServerElem in element.GetElementsByTagName( "game-server" ) )
			{
				var name = Parser.GetElementString( gameServerElem, "name" );

				if ( name == null )
				{
					log.Warning( "Game server without name ignored" );
					continue;
				}

				var addressString = Parser.GetElementString( gameServerElem, "address" );

				if ( addressString == null )
				{
					log.Warning( "Game server without address ignored" );
					continue;
				}

				var splitted = addressString.Split( new[] {':'}, 2 );

				if ( splitted.Length != 2 )
				{
					log.Warning( "Game server without port ignored" );
					continue;
				}

				IPAddress ip;

				try
				{
					var he = Dns.GetHostEntry( splitted[0] );

					if ( he.AddressList.Length == 0 )
					{
						log.Warning( "Failed to resolve {0}", splitted[0] );
						continue;
					}

					ip = he.AddressList[he.AddressList.Length - 1];
				}
				catch ( Exception e )
				{
					log.Warning( "Failed to resolve {0}: {1}", splitted[0], e );
					continue;
				}

				short port;

				try
				{
					port = Int16.Parse( splitted[1] );
				}
				catch
				{
					log.Warning( "Invalid game server port ignored" );
					continue;
				}

				var address = new IPEndPoint( ip, port );

				var sendAuthId = Parser.GetElementBool( gameServerElem, "send-auth-id", false );
				var query = Parser.GetElementBool( gameServerElem, "query", false );
				var optional = Parser.GetElementBool( gameServerElem, "optional", false );

				m_Servers.Add( new GameServer( name, address, sendAuthId, query, optional ) );
			}
		}

		public IEnumerator GetEnumerator()
		{
			return m_Servers.GetEnumerator();
		}

		public int Count => m_Servers.Count;

		public GameServer this[ int index ] => m_Servers[index];

		public GameServer this[ string name ]
		{
			get { return m_Servers.FirstOrDefault( s => s.Name == name ); }
		}
	}
}
