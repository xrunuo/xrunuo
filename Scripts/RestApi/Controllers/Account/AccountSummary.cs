using System;
using System.Net;

using Server;
using Server.Accounting;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/accounts/{username}/summary" )]
	public class AccountSummaryController : BaseProtectedController
	{
		public override AccessLevel RequiredAccessLevel => AccessLevel.Player;

		public override object HandleRequest( Request request )
		{
			var username = request["username"];
			var acct = Accounts.GetAccount( username );

			if ( acct == null )
				throw new NotFound( "Account does not exist: " + username );

			var account = request.GetAccount();
			if ( account.AccessLevel <= AccessLevel.Player && account != acct )
				throw new AccessDenied( "Cannot see other player account summary" );

			var email = acct.GetTag( "email" ) ?? "";

			var age = DateTime.Now - acct.Created;

			return new
			{
				AccountSummary = new
				{
					Username = acct.Username,
					Email = email,
					Created = acct.Created.ToUnixTime(),
					Gametime = (int) acct.TotalGameTime.TotalSeconds,
					Age = (int) age.TotalSeconds
				}
			};
		}
	}
}
