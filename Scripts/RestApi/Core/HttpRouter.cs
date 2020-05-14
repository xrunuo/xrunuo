using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class HttpRouter
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private Dictionary<Route, BaseController> _controllers;

		public HttpRouter()
		{
			_controllers = new Dictionary<Route, BaseController>();
		}

		public void RegisterController( Route route, Type controllerType )
		{
			_controllers[route] = (BaseController) Activator.CreateInstance( controllerType );

			log.Info( "Registered controller {0} for route {1}", controllerType.Name, route );
		}

		public void ProcessRequest( HttpListenerContext context )
		{
			try
			{
				// Acquire the controller
				var path = context.Request.RawUrl.Split( '?' ).First();

				// Select the route that matches the path
				var route = _controllers.Keys.FirstOrDefault( r => r.IsMatch( path ) );
				if ( route == null )
					throw new NotFound( "No controller found to handle request to " + path );

				var controller = _controllers[route];

				// Call the controller
				controller.AccessCheck( context );
				var request = new Request( context, route.GetUriParameters( path ) );
				var response = controller.HandleRequest( request );

				// Serialize the response
				var jsonResponse = JsonConvert.SerializeObject( response );

				// Write the serialized data into the output stream
				context.Response.ContentType = "application/json";
				byte[] outputBuffer = Encoding.ASCII.GetBytes( jsonResponse );
				context.Response.OutputStream.Write( outputBuffer, 0, outputBuffer.Length );
			}
			catch ( NotFound )
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
	}
}
