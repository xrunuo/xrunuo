using System;
using System.IO;
using System.Xml;
using Server.Events;

namespace Server.Accounting
{
	public static class AccountPersistence
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler( Load );
			EventSink.WorldSave += new WorldSaveEventHandler( Save );
		}

		private static void Load()
		{
			string filePath = Path.Combine( "Saves/Accounts", "accounts.xml" );

			if ( !File.Exists( filePath ) )
				return;

			XmlDocument doc = new XmlDocument();
			doc.Load( filePath );

			XmlElement root = doc["accounts"];

			foreach ( XmlElement accountXmlElement in root.GetElementsByTagName( "account" ) )
			{
				try
				{
					Account acct = new Account( accountXmlElement );

					Accounts.AddAccount( acct );
				}
				catch
				{
					log.Warning( "Account instance load failed" );
				}
			}
		}

		private static void Save( WorldSaveEventArgs e )
		{
			if ( !Directory.Exists( "Saves/Accounts" ) )
				Directory.CreateDirectory( "Saves/Accounts" );

			string filePath = Path.Combine( "Saves/Accounts", "accounts.xml" );

			using ( StreamWriter op = new StreamWriter( filePath ) )
			{
				XmlTextWriter xml = new XmlTextWriter( op );

				xml.Formatting = Formatting.Indented;
				xml.IndentChar = '\t';
				xml.Indentation = 1;

				xml.WriteStartDocument( true );

				xml.WriteStartElement( "accounts" );

				xml.WriteAttributeString( "count", Accounts.Count.ToString() );

				foreach ( Account account in Accounts.GetAccounts() )
					account.Save( xml );

				xml.WriteEndElement();

				xml.Close();
			}
		}
	}
}
