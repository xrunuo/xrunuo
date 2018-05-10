using System;
using System.Net;
using Server.Accounting;
using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/accounts/{username}/validate" )]
	public class AccountValidateController : BaseController
	{
		public override object HandleRequest( HttpListenerContext context, Parameters parameters )
		{
			if ( context.Request.HttpMethod != "PUT" )
				throw new NotSupportedException();

			var username = parameters["username"];
			var acct = Accounts.GetAccount( username );

			if ( acct == null )
				throw new NotFound( "Account does not exist: " + username );

			var request = GetRequestData<AccountValidateRequest>( context );
			string authCode = request.AuthCode;

//			var result = AccountValidationSystem.EndValidation( m_Account, authCode );
//
//			if ( result == AccountValidationResult.Valid )
//			{
//				Analytics.Track( m_Account, MapBuilder.CreateEvent( "Accounting", "End email validation", "Web" ).Build() );
//			}

			return new
			{
				AccountValidateResponse = new
				{
//					Result = result
				}
			};
		}

		private class AccountValidateRequest
		{
			public string AuthCode { get; set; }
		}
	}
}
