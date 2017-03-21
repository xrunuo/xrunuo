using System;
using System.IO;
using Server;
using Server.Events;

namespace Server.Misc
{
	// This fastwalk detection is no longer required
	// As of B36 PlayerMobile implements movement packet throttling which more reliably controls movement speeds
	public class Fastwalk
	{
		private static int MaxSteps = 4; // Maximum number of queued steps until fastwalk is detected
		private static bool Enabled = true; // Is fastwalk detection enabled?
		private static bool UOTDOverride = false; // Should UO:TD clients not be checked for fastwalk?
		private static AccessLevel AccessOverride = AccessLevel.GameMaster; // Anyone with this or higher access level is not checked for fastwalk

		public static void Initialize()
		{
			Mobile.FwdMaxSteps = MaxSteps;
			Mobile.FwdEnabled = Enabled;
			Mobile.FwdUOTDOverride = UOTDOverride;
			Mobile.FwdAccessOverride = AccessOverride;

			if ( Enabled )
				EventSink.FastWalk += new FastWalkEventHandler( OnFastWalk );
		}

		public static void OnFastWalk( FastWalkEventArgs e )
		{
			Console.WriteLine( "Client: {0}: Fast movement detected (name={1})", e.NetState, e.NetState.Mobile.Name );

			if ( e.NetState.Mobile.Map == Map.Felucca )
				Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Counselor, 0x20, String.Format( "Fastwalk: {0}, {1}", DateTime.Now, e.NetState.Mobile.Name ) );

			try
			{
				using ( StreamWriter op = new StreamWriter( Path.Combine( Environment.Config.LogDirectory, "Fastwalk.log" ), true ) )
					op.WriteLine( "{0}\t{1}\t{2}\t{3}", DateTime.Now, e.NetState, e.NetState.Account.ToString(), e.NetState.Mobile.Name );
			}
			catch { }

			e.Blocked = true; //disallow this fastwalk			
		}
	}
}