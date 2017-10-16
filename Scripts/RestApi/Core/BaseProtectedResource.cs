using System;
using System.Net;

using Server;
using Server.Accounting;

namespace Server.Engines.RestApi
{
	public abstract class BaseProtectedResource : BaseResource
	{
		public abstract AccessLevel RequiredAccessLevel { get; }

		public override void AccessCheck( HttpListenerContext context )
		{
			var identity = (HttpListenerBasicIdentity) context.User.Identity;
			var username = identity.Name;
			var password = identity.Password;

			var account = GetAccount( context );

			if ( account == null )
				throw new AccessDenied( "Unexistant account" );

			if ( !account.CheckPassword( password ) )
				throw new AccessDenied( "Invalid credentials" );

			if ( account.AccessLevel < RequiredAccessLevel )
				throw new AccessDenied( "Insufficient permissions" );
		}

		protected Account GetAccount( HttpListenerContext context )
		{
			return Accounts.GetAccount( context.User.Identity.Name );
		}
	}

	public class AccessDenied : Exception
	{
		public AccessDenied( string message )
			: base( message )
		{
		}
	}
}
