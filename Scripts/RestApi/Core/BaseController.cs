using System;
using System.Net;

namespace Server.Engines.RestApi
{
	public abstract class BaseController
	{
		/// <summary>
		/// Checks the access to this resource.
		/// </summary>
		/// <param name="context"></param>
		/// <exception cref="AccessDenied">If client is not allowed to access this resource.</exception>
		public virtual void AccessCheck( HttpListenerContext context )
		{
		}

		/// <summary>
		/// Handles a request to this resource.
		/// </summary>
		/// <param name="request"></param>
		/// <returns>The response</returns>
		public abstract object HandleRequest( Request request );
	}
}
