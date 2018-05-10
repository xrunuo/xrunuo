using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

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
		/// <param name="context"></param>
		/// <returns>The response</returns>
		public abstract object HandleRequest( HttpListenerContext context, Parameters parameters );

		/// <summary>
		/// Reads the request parameters as a json string and deserializes it
		/// into the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected T GetRequestData<T>( HttpListenerContext context )
		{
			var data = "";

			using ( var body = context.Request.InputStream )
			using ( var reader = new StreamReader( body, context.Request.ContentEncoding ) )
				data = reader.ReadToEnd();

			var json = new JavaScriptSerializer();
			var request = json.Deserialize<T>( data );

			return request;
		}

		/// <summary>
		/// Reads the request parameters as a json string and deserializes it
		/// into a dictionary string/string.
		/// </summary>
		protected Dictionary<string, string> GetRequestData( HttpListenerContext context )
		{
			return GetRequestData<Dictionary<string, string>>( context );
		}
	}
}
