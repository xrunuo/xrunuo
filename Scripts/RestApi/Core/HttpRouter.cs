using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class HttpRouter
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private Dictionary<Route, Type> _controllerTypes;

		public HttpRouter()
		{
			_controllerTypes = new Dictionary<Route, Type>();
		}

		public void RegisterController( Route route, Type controllerType )
		{
			_controllerTypes[route] = controllerType;
		}

		public void ProcessRequest( HttpListenerContext context )
		{
			try
			{
				// Acquire the controller
				var path = context.Request.RawUrl.Split( '?' ).First();

				// Select the route that matches the path
				var route = _controllerTypes.Keys.FirstOrDefault( r => r.IsMatch( path ) );

				if ( route == null )
					throw new NotFound( "No controller found to handle request to " + path );

				var controllerType = _controllerTypes[route];
				var controller = (BaseController) Activator.CreateInstance( controllerType );

				// Call the handler
				var uriParameters = route.GetUriParameters( path );
				controller.AccessCheck( context );
				var request = new Request( context, uriParameters );
				var response = controller.HandleRequest( request );

				// Serialize the response
				var jsonResponse = JsonSerialize( response );

				// Write the serialized data into the output stream
				context.Response.ContentType = "application/json";
				byte[] outputBuffer = Encoding.ASCII.GetBytes( jsonResponse );
				context.Response.OutputStream.Write( outputBuffer, 0, outputBuffer.Length );
			}
			catch ( NotFound e )
			{
				log.Error( "Rest Api: Not found: {0}", context.Request.RawUrl );
				context.Response.StatusCode = 404; // Not found
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

		private string JsonSerialize( object o )
		{
			var sb = new StringBuilder();
			var json = new JavaScriptSerializer();

			json.Serialize( o, sb );

			return sb.ToString();
		}
	}
}
