using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using Server;
using Server.Accounting;
using Server.Network;
using Server.Events;

namespace Server.Misc
{
	public class CrashGuard
	{
		private static bool Enabled = true;
		private static bool SaveBackup = true;
		private static bool ShutdownServer = true;
		private static bool RestartServer = false;
		private static bool GenerateReport = true;

		private static string FromAddress = Environment.Config.ServerEmail;
		private static string CrashAddresses = Environment.Config.ServerEmail;

		public static void Initialize()
		{
			if ( Enabled ) // If enabled, register our crash event handler
				EventSink.Instance.Crashed += new CrashedEventHandler( CrashGuard_OnCrash );
		}

		public static void CrashGuard_OnCrash( CrashedEventArgs e )
		{
			if ( GenerateReport )
				GenerateCrashReport( e );

			World.Instance.WaitForWriteCompletion();

			if ( SaveBackup )
				Backup();

			if ( ShutdownServer )
				Shutdown();

			if ( RestartServer )
				Restart( e );
		}

		private static void SendEmail( string filePath )
		{
			Console.Write( "Crash: Sending email..." );

			MailMessage message = new MailMessage( FromAddress, CrashAddresses );

			message.Subject = "Automated X-RunUO Crash Report";

			message.Body = "Automated X-RunUO Crash Report. See attachment for details.";

			message.Attachments.Add( new Attachment( filePath ) );

			if ( Email.Send( message ) )
				Console.WriteLine( "done" );
			else
				Console.WriteLine( "failed" );
		}

		private static string GetRoot()
		{
			try
			{
				return Path.GetDirectoryName( System.Environment.GetCommandLineArgs()[0] );
			}
			catch
			{
				return "";
			}
		}

		private static string Combine( string path1, string path2 )
		{
			if ( path1.Length == 0 )
				return path2;

			return Path.Combine( path1, path2 );
		}

		private static void Shutdown()
		{
			Environment.Process.Kill();
		}

		private static void Restart( CrashedEventArgs e )
		{
			string root = GetRoot();

			Console.Write( "Crash: Restarting..." );

			try
			{
				Process.Start( Environment.ExePath/*, Core.Arguments*/ );
				Console.WriteLine( "done" );

				e.Close = true;
			}
			catch
			{
				Console.WriteLine( "failed" );
			}
		}

		private static void CreateDirectory( string path )
		{
			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );
		}

		private static void CreateDirectory( string path1, string path2 )
		{
			CreateDirectory( Combine( path1, path2 ) );
		}

		private static void CopyFile( string rootOrigin, string rootBackup, string path )
		{
			string originPath = Combine( rootOrigin, path );
			string backupPath = Combine( rootBackup, path );

			try
			{
				if ( File.Exists( originPath ) )
					File.Copy( originPath, backupPath );
			}
			catch
			{
			}
		}

		private static void Backup()
		{
			Console.Write( "Crash: Backing up..." );

			try
			{
				string timeStamp = GetTimeStamp();

				string root = GetRoot();
				string rootBackup = Combine( root, String.Format( "Backups/Crashed/{0}/", timeStamp ) );
				string rootOrigin = Combine( root, String.Format( "Saves/" ) );

				// Create new directories
				CreateDirectory( rootBackup );
				CreateDirectory( rootBackup, "Accounts/" );
				CreateDirectory( rootBackup, "Items/" );
				CreateDirectory( rootBackup, "Mobiles/" );
				CreateDirectory( rootBackup, "Guilds/" );
				CreateDirectory( rootBackup, "Jailings/" );
				CreateDirectory( rootBackup, "Chat/" );

				// Copy files
				CopyFile( rootOrigin, rootBackup, "Accounts/Accounts.xml" );

				CopyFile( rootOrigin, rootBackup, "Items/Items.bin" );
				CopyFile( rootOrigin, rootBackup, "Items/Items.idx" );
				CopyFile( rootOrigin, rootBackup, "Items/Items.tdb" );

				CopyFile( rootOrigin, rootBackup, "Mobiles/Mobiles.bin" );
				CopyFile( rootOrigin, rootBackup, "Mobiles/Mobiles.idx" );
				CopyFile( rootOrigin, rootBackup, "Mobiles/Mobiles.tdb" );

				CopyFile( rootOrigin, rootBackup, "Guilds/Guilds.bin" );
				CopyFile( rootOrigin, rootBackup, "Guilds/Guilds.idx" );

				CopyFile( rootOrigin, rootBackup, "Jailings/eventbans.xml" );
				CopyFile( rootOrigin, rootBackup, "Jailings/jailings.xml" );

				CopyFile( rootOrigin, rootBackup, "Chat/Chat.bin" );

				Console.WriteLine( "done" );
			}
			catch
			{
				Console.WriteLine( "failed" );
			}
		}

		private static void GenerateCrashReport( CrashedEventArgs e )
		{
			Console.Write( "Crash: Generating report..." );

			try
			{
				string timeStamp = GetTimeStamp();
				string fileName = Path.Combine( Environment.Config.LogDirectory, String.Format( "Crash {0}.log", timeStamp ) );

				string root = GetRoot();
				string filePath = Combine( root, fileName );

				using ( StreamWriter op = new StreamWriter( filePath ) )
				{
					Version ver = Environment.Assembly.GetName().Version;

					op.WriteLine( "Server Crash Report" );
					op.WriteLine( "===================" );
					op.WriteLine();
					op.WriteLine( "X-RunUO Version {0}.{1}.{2}, Build {3}", ver.Major, ver.Minor, ver.Build, ver.Revision );
					op.WriteLine( "Operating System: {0}", System.Environment.OSVersion );
					op.WriteLine( ".NET Framework: {0}", System.Environment.Version );
					op.WriteLine( "Time: {0}", DateTime.Now );

					try { op.WriteLine( "Mobiles: {0}", World.Instance.MobileCount ); }
					catch { }

					try { op.WriteLine( "Items: {0}", World.Instance.ItemCount ); }
					catch { }

					op.WriteLine( "Exception:" );
					op.WriteLine( e.Exception );
					op.WriteLine();

					op.WriteLine( "Clients:" );

					try
					{
						var server = GameServer.Instance;

						op.WriteLine( "- Count: {0}", server.ClientCount );

						foreach ( var client in server.Clients )
						{
							op.Write( "+ {0}:", client );

							Account a = client.Account as Account;

							if ( a != null )
								op.Write( " (account = {0})", a.Username );

							Mobile m = client.Mobile;

							if ( m != null )
								op.Write( " (mobile = 0x{0:X} '{1}')", m.Serial.Value, m.Name );

							op.WriteLine();
						}
					}
					catch
					{
						op.WriteLine( "- Failed" );
					}
				}

				Console.WriteLine( "done" );

				if ( FromAddress != null && CrashAddresses != null )
					SendEmail( filePath );
			}
			catch
			{
				Console.WriteLine( "failed" );
			}
		}

		private static string GetTimeStamp()
		{
			DateTime now = DateTime.Now;

			return String.Format( "{0}-{1}-{2}-{3}-{4}-{5}",
					now.Day,
					now.Month,
					now.Year,
					now.Hour,
					now.Minute,
					now.Second
				);
		}
	}
}
