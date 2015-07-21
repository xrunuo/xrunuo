using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Server;
using Server.Accounting;
using Server.Engines.Help;
using Server.Network;
using Server.Events;

namespace Server.Misc
{
	public enum PasswordProtection
	{
		None,
		Crypt,
		NewCrypt
	}

	public class AccountHandler
	{
		private static readonly int MaxAccountsPerIP = Environment.Config.Login.MaxAccountsPerIP;
		private static readonly bool AutoAccountCreation = Environment.Config.Login.AutoCreateAccounts;

		private static readonly bool RestrictDeletion = !TestCenter.Enabled;
		private static readonly TimeSpan DeleteDelay = TimeSpan.FromDays( 7.0 );

		public static readonly PasswordProtection ProtectPasswords = PasswordProtection.NewCrypt;

		private static AccessLevel m_LockdownLevel;

		public static AccessLevel LockdownLevel
		{
			get { return m_LockdownLevel; }
			set { m_LockdownLevel = value; }
		}

		private static CityInfo[] StartingCities = new CityInfo[]
			{
				new CityInfo( "New Haven",	"New Haven Bank",			3498,  2572,  14, Map.Trammel, 1150168 ),
				new CityInfo( "Yew",		"The Empath Abbey",			633,   858,    0, Map.Trammel, 1075072 ),
				new CityInfo( "Minoc",		"The Barnacle",				2476,  413,   15, Map.Trammel, 1075073 ),
				new CityInfo( "Britain",	"The Wayfarer's Inn",		1585,  1591,  20, Map.Trammel, 1075074 ),
				new CityInfo( "Moonglow",	"The Scholars Inn",			4408,  1168,   0, Map.Trammel, 1075075 ),
				new CityInfo( "Trinsic",	"The Traveler's Inn",		1845,  2745,   0, Map.Trammel, 1075076 ),
				new CityInfo( "Jhelom",		"The Mercenary Inn",		1374,  3826,   0, Map.Trammel, 1075078 ),
				new CityInfo( "Skara Brae",	"The Falconer's Inn",		618,   2234,   0, Map.Trammel, 1075079 ),
				new CityInfo( "Royal City",	"Ter Mur Royal City Inn",	738,   3486, -19, Map.TerMur,  1150169 ),
			};

		public static void Initialize()
		{
			EventSink.Instance.DeleteRequest += new DeleteRequestEventHandler( EventSink_DeleteRequest );
			EventSink.Instance.AccountLogin += new AccountLoginEventHandler( EventSink_AccountLogin );
			EventSink.Instance.GameLogin += new GameLoginEventHandler( EventSink_GameLogin );
		}

		private static void EventSink_DeleteRequest( DeleteRequestEventArgs e )
		{
			GameClient state = e.State;
			int index = e.Index;

			Account acct = state.Account as Account;

			if ( acct == null )
			{
				state.Dispose();
			}
			else if ( index < 0 || index >= acct.Length )
			{
				state.Send( new DeleteResult( DeleteResultType.BadRequest ) );
				state.Send( new CharacterListUpdate( acct ) );
			}
			else
			{
				Mobile m = acct[index];

				if ( m == null )
				{
					state.Send( new DeleteResult( DeleteResultType.CharNotExist ) );
					state.Send( new CharacterListUpdate( acct ) );
				}
				else if ( m.Client != null )
				{
					state.Send( new DeleteResult( DeleteResultType.CharBeingPlayed ) );
					state.Send( new CharacterListUpdate( acct ) );
				}
				else if ( RestrictDeletion && DateTime.Now < ( m.CreationTime + DeleteDelay ) )
				{
					state.Send( new DeleteResult( DeleteResultType.CharTooYoung ) );
					state.Send( new CharacterListUpdate( acct ) );
				}
				else
				{
					Console.WriteLine( "Client: {0}: Deleting character {1} (0x{2:X})", state, index, m.Serial.Value );

					acct.Comments.Add( new AccountComment( "System", String.Format( "Character #{0} {1} deleted by {2}", index + 1, m, state ) ) );

					m.Delete();
					state.Send( new CharacterListUpdate( acct ) );
				}
			}
		}

		private static Account CreateAccount( GameClient state, string un, string pw )
		{
			if ( un.Length == 0 || pw.Length == 0 )
				return null;

			bool isSafe = true;

			for ( int i = 0; isSafe && i < un.Length; ++i )
				isSafe = ( un[i] >= 0x20 && un[i] < 0x80 );

			for ( int i = 0; isSafe && i < pw.Length; ++i )
				isSafe = ( pw[i] >= 0x20 && pw[i] < 0x80 );

			if ( !isSafe )
				return null;

			IPAddress ip = state.Address;

			int count = 0;

			foreach ( Account a in Accounts.GetAccounts() )
			{
				if ( a.LoginIPs.Length > 0 && a.LoginIPs[0].Equals( ip ) )
				{
					++count;

					if ( count >= MaxAccountsPerIP )
					{
						Console.WriteLine( "Login: {0}: Account '{1}' not created, ip already has {2} account{3}.", state, un, MaxAccountsPerIP, MaxAccountsPerIP == 1 ? "" : "s" );
						return null;
					}
				}
			}

			Console.WriteLine( "Login: {0}: Creating new account '{1}'", state, un );

			var account = Accounts.CreateAccount( un, pw );
			return account;
		}

		public static void EventSink_AccountLogin( AccountLoginEventArgs e )
		{
			if ( !IPLimiter.SocketBlock && !IPLimiter.Verify( e.State.Address, e.State.Account as Account ) )
			{
				e.Accepted = false;
				e.RejectReason = ALRReason.InUse;

				Console.WriteLine( "Login: {0}: Past IP limit threshold", e.State );

				using ( StreamWriter op = new StreamWriter( "ipLimits.log", true ) )
					op.WriteLine( "{0}\tPast IP limit threshold\t{1}", e.State, DateTime.Now );

				return;
			}

			string un = e.Username;
			string pw = e.Password;

			e.Accepted = false;
			Account acct = Accounts.GetAccount( un );

			if ( acct == null )
			{
				if ( AutoAccountCreation )
				{
					e.State.Account = acct = CreateAccount( e.State, un, pw );
					e.Accepted = acct == null ? false : acct.CheckAccess( e.State );

					if ( !e.Accepted )
						e.RejectReason = ALRReason.BadComm;
				}
				else
				{
					Console.WriteLine( "Login: {0}: Invalid username '{1}'", e.State, un );
					e.RejectReason = ALRReason.Invalid;
				}
			}
			else if ( !acct.HasAccess( e.State ) )
			{
				Console.WriteLine( "Login: {0}: Access denied for '{1}'", e.State, un );
				e.RejectReason = ( m_LockdownLevel > AccessLevel.Player ? ALRReason.BadComm : ALRReason.Invalid );
			}
			else if ( !acct.CheckPassword( pw ) )
			{
				Console.WriteLine( "Login: {0}: Invalid password for '{1}'", e.State, un );
				e.RejectReason = ALRReason.Invalid;
			}
			else if ( acct.Banned )
			{
				Console.WriteLine( "Login: {0}: Banned account '{1}'", e.State, un );
				e.RejectReason = ALRReason.Blocked;
			}
			else
			{
				Console.WriteLine( "Login: {0}: Valid credentials for '{1}'", e.State, un );
				e.State.Account = acct;
				e.Accepted = true;

				acct.LogAccess( e.State );
			}

			if ( !e.Accepted )
				AccountAttackLimiter.RegisterInvalidAccess( e.State );
		}

		public static void EventSink_GameLogin( GameLoginEventArgs e )
		{
			string un = e.Username;
			string pw = e.Password;

			Account acct = Accounts.GetAccount( un );

			if ( TestCenter.Enabled && AutoAccountCreation && acct == null )
				acct = CreateAccount( e.State, un, pw );

			if ( acct == null )
			{
				e.Accepted = false;
			}
			else if ( !IPLimiter.SocketBlock && !IPLimiter.Verify( e.State.Address, acct ) )
			{
				e.Accepted = false;

				Console.WriteLine( "Login: {0}: Past IP limit threshold", e.State );

				using ( StreamWriter op = new StreamWriter( "ipLimits.log", true ) )
					op.WriteLine( "{0}\tPast IP limit threshold\t{1}", e.State, DateTime.Now );
			}
			else if ( !acct.HasAccess( e.State ) )
			{
				Console.WriteLine( "Login: {0}: Access denied for '{1}'", e.State, un );
				e.Accepted = false;
			}
			else if ( !acct.CheckPassword( pw ) )
			{
				Console.WriteLine( "Login: {0}: Invalid password for '{1}'", e.State, un );
				e.Accepted = false;
			}
			else if ( acct.Banned )
			{
				Console.WriteLine( "Login: {0}: Banned account '{1}'", e.State, un );
				e.Accepted = false;
			}
			else
			{
				acct.LogAccess( e.State );

				Console.WriteLine( "Login: {0}: Account '{1}' at character list", e.State, un );
				e.State.Account = acct;
				e.Accepted = true;
				e.CityInfo = StartingCities;
			}

			if ( !e.Accepted )
				AccountAttackLimiter.RegisterInvalidAccess( e.State );
		}

		public static bool CheckAccount( Mobile mobCheck, Mobile accCheck )
		{
			if ( accCheck != null )
			{
				Account a = accCheck.Account as Account;

				if ( a != null )
				{
					for ( int i = 0; i < a.Length; ++i )
					{
						if ( a[i] == mobCheck )
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}