using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

using Server;
using Server.Web;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class HttpRouter
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private Dictionary<Route, Type> _routingMap;

		public HttpRouter()
		{
			_routingMap = new Dictionary<Route, Type>();
		}

		public void RegisterLocator( Route route, Type locatorType )
		{
			_routingMap[route] = locatorType;
		}

		public void ProcessRequest( HttpListenerContext context )
		{
			try
			{
				// Acquire the resource
				var resource = AcquireResource( context.Request.RawUrl.Split( '?' ).First() );

				if ( resource != null )
				{
					// Call the handler
					resource.AccessCheck( context );
					var response = resource.HandleRequest( context );

					// Serialize the response
					var jsonResponse = JsonSerialize( response );

					// Write the serialized data into the output stream
					context.Response.ContentType = "application/json";
					byte[] outputBuffer = Encoding.ASCII.GetBytes( jsonResponse );
					context.Response.OutputStream.Write( outputBuffer, 0, outputBuffer.Length );
				}
				else
				{
					log.Error( "Rest Api: Not found: {0}", context.Request.RawUrl );
					context.Response.StatusCode = 404; // Not found
				}
			}
			catch ( AccessDenied e )
			{
				log.Error( "Rest Api: Access denied: {0}", e );
				context.Response.StatusCode = 401; // Unauthorized
			}
			catch ( Exception e )
			{
				log.Error( "Rest Api: Unexpected error: {0}", e );
				context.Response.StatusCode = 500;
			}
		}

		private BaseResource AcquireResource( string uri )
		{
			BaseResource resource = null;

			// Select the route that matches the uri
			var route = _routingMap.Keys.FirstOrDefault(
				r => r.IsMatch( uri ) );

			if ( route != null )
			{
				// Get the resource locator
				var locatorType = _routingMap[route];
				var locator = (BaseLocator) Activator.CreateInstance( locatorType );

				if ( locator != null )
				{
					// Get the matched parameters
					var parameters = route.GetMatchedParameters( uri );

					// Acquire the resource
					resource = locator.Locate( parameters );
				}
			}

			return resource;
		}

		private string JsonSerialize( object o )
		{
			var sb = new StringBuilder();
			var json = new JavaScriptSerializer();

			json.Serialize( o, sb );

			return sb.ToString();
		}
	}
}
