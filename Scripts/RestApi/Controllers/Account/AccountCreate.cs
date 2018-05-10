using System;
using System.Net;

using Server;
using Server.Accounting;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/accounts/create" )]
	public class AccountCreateController : BaseController
	{
		public override object HandleRequest( Request request )
		{
			if ( request.HttpMethod != "PUT" )
				throw new NotSupportedException();

			var accountInputData = request.AsDto<AccountCreateRequest>().AccountData;

			string username = accountInputData.Username;
			string password = accountInputData.Password;
			string email = accountInputData.Email;

			Console.Write( "Rest Api: Create account request with user='{0}', pass='{1}', email='{2}'... ", username, password, email );

			Account acct = Accounts.GetAccount( username );
			AccountCreateResult result;

			if ( acct != null )
			{
				result = AccountCreateResult.AlreadyExist;
				Console.WriteLine( "already exists" );
			}
			else if ( !ValidateInputData( accountInputData ) )
			{
				result = AccountCreateResult.InvalidParams;
				Console.WriteLine( "invalid request" );
			}
			else
			{
				acct = Accounts.CreateAccount( username, password, email );

				if ( acct != null )
				{
					//AccountValidationSystem.BeginValidation( acct, email );

					result = AccountCreateResult.Valid;
					Console.WriteLine( "done" );
				}
				else
				{
					result = AccountCreateResult.InternalError;
					Console.WriteLine( "internal error" );
				}
			}

			return new
			{
				AccountCreateResponse = new
				{
					Result = result
				}
			};
		}

		private static bool ValidateInputData( AccountData accountData )
		{
			if ( !IsSafe( accountData.Username ) || !IsSafe( accountData.Password ) )
				return false;

			if ( !ValidateEmailFormat( accountData.Email ) )
				return false;

			return true;
		}

		private static bool IsSafe( string s )
		{
			if ( string.IsNullOrEmpty( s ) )
				return false;

			bool isSafe = true;

			for ( int i = 0; isSafe && i < s.Length; ++i )
				isSafe = ( s[i] >= 0x20 && s[i] < 0x80 );

			return isSafe;
		}

		private static bool ValidateEmailFormat( string email )
		{
			if ( email.IndexOf( "@" ) < 0 || email.IndexOf( "." ) < 0 || email.Length < 4 )
				return false;

			return true;
		}

		private class AccountCreateRequest
		{
			public AccountData AccountData { get; set; }
		}

		private class AccountData
		{
			public string Username { get; set; }
			public string Password { get; set; }
			public string Email { get; set; }
			public string PromoCode { get; set; }
		}

		private enum AccountCreateResult
		{
			Valid = 0,
			AlreadyExist = 1,
			InvalidParams = 2,
			InternalError = 3,
		}
	}
}
