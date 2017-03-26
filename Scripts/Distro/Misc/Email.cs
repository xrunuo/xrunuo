using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server.Misc
{
	public class Email
	{
		public static readonly string SmtpServer = Config.Get( "Email.SmtpServer", null );
		public static readonly int SmtpPort = Config.Get( "Email.SmtpPort", 25 );
		public static readonly bool EnableSsl = Config.Get( "Email.EnableSsl", false );
		public static readonly string SmtpUser = Config.Get( "Email.SmtpUser", null );
		public static readonly string SmtpPassword = Config.Get( "Email.SmtpPassword", null );

		private static Regex _pattern = new Regex( @"^[a-z0-9.+_-]+@([a-z0-9-]+.)+[a-z]+$", RegexOptions.IgnoreCase );

		public static bool IsValid( string address )
		{
			if ( address == null || address.Length > 320 )
				return false;

			return _pattern.IsMatch( address );
		}

		private static SmtpClient m_Client;

		public static void Configure()
		{
			m_Client = new SmtpClient( SmtpServer, SmtpPort );
			m_Client.UseDefaultCredentials = false;
			m_Client.Credentials = new NetworkCredential( SmtpUser, SmtpPassword );
			m_Client.EnableSsl = EnableSsl;
			ServicePointManager.ServerCertificateValidationCallback = ( s, certificate, chain, sslPolicyErrors ) => true;
		}

		public static bool Send( MailMessage message )
		{
			try
			{
				lock ( m_Client )
				{
					m_Client.Send( message );
				}
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
				return false;
			}

			return true;
		}

		public static void AsyncSend( MailMessage message )
		{
			ThreadPool.QueueUserWorkItem( new WaitCallback( SendCallback ), message );
		}

		private static void SendCallback( object state )
		{
			MailMessage message = (MailMessage) state;

			if ( Send( message ) )
				Console.WriteLine( "Sent e-mail '{0}' to '{1}'.", message.Subject, message.To );
			else
				Console.WriteLine( "Failure sending e-mail '{0}' to '{1}'.", message.Subject, message.To );
		}
	}
}
