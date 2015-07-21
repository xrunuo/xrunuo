using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Mail;

using Server;
using Server.Accounting;

namespace Server.Misc
{
	public static class AccountCleanup
	{
		/// <summary>
		/// How many days before the account gets deleted will the remainder email be sent.
		/// </summary>
		public static readonly int RemainderEmailDays = 5;

		public static void Initialize()
		{
			if ( !TestCenter.Enabled )
				Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerCallback( Run ) );
		}

		private static void Run()
		{
			int accountDecayPeriodDays = (int) Environment.Config.AccountDecay.TotalDays;

			var eligibleAccounts = Accounts.GetAccounts().Where( a => a.CanExpire() ).ToArray();

			foreach ( var account in eligibleAccounts )
			{
				int accountInactivityDays = (int) ( DateTime.Now - account.LastLogin ).TotalDays;
				int accountRemainingDays = accountDecayPeriodDays - accountInactivityDays;

				if ( accountRemainingDays <= 0 )
					account.Freeze();
				else if ( accountRemainingDays == RemainderEmailDays )
					account.SendCleanupRemainderEmail();
			}
		}

		private static bool CanExpire( this Account account )
		{
			return !account.Banned && !account.FeatureEnabled( AccountFeature.NoExpire );
		}

		private static void Freeze( this Account account )
		{
			account.Banned = true;
			account.SetTag( "InactiveSince", DateTime.Now.ToString() );
		}

		private static void SendCleanupRemainderEmail( this Account account )
		{
			if ( account.GetTag( "email" ) == null )
				return;

			MailMessage mail = new MailMessage();

			mail.Subject = String.Format( "{0} Account Management", Environment.Config.ServerName );
			mail.From = new MailAddress( Environment.Config.ServerEmail, Environment.Config.ServerName );
			mail.To.Add( new MailAddress( account.GetTag( "email" ) ) );

			using ( StringWriter writer = new StringWriter() )
			{
				writer.WriteLine( String.Format( "Hello, {0}!", account.Username ) );
				writer.WriteLine();

				writer.WriteLine( String.Format( "We want to inform you with this email that your account on {0} will be cancelled within 5 days by the Automatic Old-Account Purge System.", Environment.Config.ServerName ) );
				writer.WriteLine();

				writer.WriteLine( "If you want to avoid this, you must log into the shard before this period. Otherwise it will be permanently deleted with no possibilities of recovery." );
				writer.WriteLine();

				writer.WriteLine( "Hope to see you soon between us!" );
				writer.WriteLine();

				writer.WriteLine( "Regards!" );
				writer.WriteLine();

				writer.WriteLine( String.Format( "{0} Staff.", Environment.Config.ServerName ) );

				mail.Body = writer.ToString();
			}

			Misc.Email.AsyncSend( mail );
		}
	}
}