using System;
using System.Diagnostics;
using System.IO;

using Server;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;
using Server.Events;

namespace Server.Misc
{
	public class ClientVerification
	{
		private enum OldClientResponse
		{
			Ignore,
			Warn,
			Annoy,
			LenientKick,
			Kick
		}

		private static bool m_DetectClientRequirement = false;
		private static OldClientResponse m_OldClientResponse = OldClientResponse.Annoy;

		private static ClientVersion m_Required;
		private static bool m_AllowRegular = true, m_AllowEnhanced = true;

		private static TimeSpan m_AgeLeniency = TimeSpan.FromDays( 10.0 );
		private static TimeSpan m_GameTimeLeniency = TimeSpan.FromHours( 25.0 );

		private static TimeSpan m_KickDelay = TimeSpan.FromSeconds( 20.0 );

		public static ClientVersion Required
		{
			get
			{
				return m_Required;
			}
			set
			{
				m_Required = value;
			}
		}

		public static bool AllowClassic
		{
			get
			{
				return m_AllowRegular;
			}
			set
			{
				m_AllowRegular = value;
			}
		}

		public static bool AllowEnhanced
		{
			get
			{
				return m_AllowEnhanced;
			}
			set
			{
				m_AllowEnhanced = value;
			}
		}

		public static TimeSpan KickDelay
		{
			get
			{
				return m_KickDelay;
			}
			set
			{
				m_KickDelay = value;
			}
		}

		public static void Initialize()
		{
			EventSink.Instance.ClientVersionReceived += new ClientVersionReceivedHandler( EventSink_ClientVersionReceived );

			ClientVersion.Required = null;
			Required = new ClientVersion( "7.0.34.6" );

			if ( m_DetectClientRequirement )
			{
				string path = Environment.FindDataFile( "client.exe" );

				if ( File.Exists( path ) )
				{
					FileVersionInfo info = FileVersionInfo.GetVersionInfo( path );

					if ( info.FileMajorPart != 0 || info.FileMinorPart != 0 || info.FileBuildPart != 0 || info.FilePrivatePart != 0 )
					{
						Required = new ClientVersion( info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart );
					}
				}
			}

			if ( Required != null )
			{
				Utility.PushColor( ConsoleColor.White );
				Console.WriteLine( "Restricting client version to {0}. Action to be taken: {1}", Required, m_OldClientResponse );
				Utility.PopColor();
			}
		}

		private static void EventSink_ClientVersionReceived( ClientVersionReceivedArgs e )
		{
			GameClient state = e.State;
			ClientVersion version = e.Version;

			if ( state.Mobile == null || state.Mobile.AccessLevel > AccessLevel.Player )
				return;

			string kickMessage = null;

			if ( Required != null && version < Required && ( m_OldClientResponse == OldClientResponse.Kick || ( m_OldClientResponse == OldClientResponse.LenientKick && ( DateTime.Now - state.Mobile.CreationTime ) > m_AgeLeniency && state.Mobile is PlayerMobile && ( (PlayerMobile) state.Mobile ).GameTime > m_GameTimeLeniency ) ) )
			{
				kickMessage = String.Format( "This server requires your client version be at least {0}.", Required );
			}
			else if ( !AllowClassic || !AllowEnhanced )
			{
				if ( !AllowClassic && version.Type == ClientType.Classic )
					kickMessage = "This server does not allow classic client to connect.";
				else if ( !AllowEnhanced && version.Type == ClientType.Enhanced )
					kickMessage = "This server does not allow enhanced client to connect.";

				if ( !AllowClassic && !AllowEnhanced )
				{
					kickMessage = "This server does not allow any clients to connect.";
				}
				else if ( kickMessage != null )
				{
					if ( AllowClassic && AllowEnhanced )
						kickMessage += " You can use classic or enhanced clients.";
					else if ( AllowClassic )
						kickMessage += " You can use classic client.";
					else if ( AllowEnhanced )
						kickMessage += " You can use enhanced client.";
				}
			}

			if ( kickMessage != null )
			{
				state.Mobile.SendMessage( 0x22, kickMessage );
				state.Mobile.SendMessage( 0x22, "You will be disconnected in {0} seconds.", KickDelay.TotalSeconds );

				Timer.DelayCall( KickDelay,
					delegate
					{
						Console.WriteLine( "Client: {0}: Disconnecting, bad version", state );
						state.Dispose();
					} );
			}
			else if ( Required != null && version < Required )
			{
				switch ( m_OldClientResponse )
				{
					case OldClientResponse.Warn:
						{
							state.Mobile.SendMessage( 0x22, "Your client is out of date. Please update your client.", Required );
							state.Mobile.SendMessage( 0x22, "This server recommends that your client version be at least {0}.", Required );
							break;
						}
					case OldClientResponse.LenientKick:
					case OldClientResponse.Annoy:
						{
							SendAnnoyGump( state.Mobile );
							break;
						}
				}
			}
		}

		private static void SendAnnoyGump( Mobile m )
		{
			if ( m.Client != null && m.Client.Version < Required )
			{
				Gump g = new WarningGump( 1060637, 30720, String.Format( "Your client is out of date. Please update your client.<br>This server recommends that your client version be at least {0}.<br> <br>You are currently using version {1}.<br> <br>To patch, run UOPatch.exe inside your Ultima Online folder.", Required, m.Client.Version ), 0xFFC000, 480, 360,
					delegate( Mobile mob, bool selection, object o )
					{
						m.SendMessage( "You will be reminded of this again." );

						if ( m_OldClientResponse == OldClientResponse.LenientKick )
							m.SendMessage( "Old clients will be kicked after {0} days of character age and {1} hours of play time", m_AgeLeniency, m_GameTimeLeniency );

						Timer.DelayCall( TimeSpan.FromMinutes( Utility.Random( 5, 15 ) ), delegate { SendAnnoyGump( m ); } );
					}, null, false );

				g.Dragable = false;
				g.Closable = false;
				g.Resizable = false;

				m.SendGump( g );
			}
		}
	}
}
