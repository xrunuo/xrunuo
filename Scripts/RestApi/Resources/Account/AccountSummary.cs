using System;
using System.Net;

using Server;
using Server.Accounting;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/accounts/{username}/summary" )]
	public class AccountSummaryLocator : BaseLocator
	{
		public override BaseResource Locate( Parameters parameters )
		{
			BaseResource resource = null;

			try
			{
				var username = parameters["username"];
				var acct = Accounts.GetAccount( username );

				if ( acct != null )
					resource = new AccountSummaryResource( acct );
			}
			catch
			{
			}

			return resource;
		}
	}

	public class AccountSummaryResource : BaseProtectedResource
	{
		public override AccessLevel RequiredAccessLevel { get { return AccessLevel.Player; } }

		private Account m_Account;

		public AccountSummaryResource( Account account )
		{
			m_Account = account;
		}

		public override void AccessCheck( HttpListenerContext context )
		{
			base.AccessCheck( context );

			var account = GetAccount( context );
			if ( account.AccessLevel <= AccessLevel.Player )
			{
				if ( account != m_Account )
					throw new AccessDenied( "Cannot see other player account summary" );
			}
		}

		public override object HandleRequest( HttpListenerContext context )
		{
			string email = m_Account.GetTag( "email" );
			if ( email == null )
				email = "";

			var age = DateTime.Now - m_Account.Created;

			return new
			{
				AccountSummary = new
				{
					Username = m_Account.Username,
					Email = email,
					Created = m_Account.Created.ToUnixTime(),
					Gametime = (int) m_Account.TotalGameTime.TotalSeconds,
					Age = (int) age.TotalSeconds
				}
			};
		}
	}
}
