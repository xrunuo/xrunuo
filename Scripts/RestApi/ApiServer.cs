using System;
using System.Linq;

using Server.Events;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class ApiServer
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static readonly bool Enabled = Config.Get( "RestApi.Enabled", true );
		public static readonly string Domain = Config.Get( "RestApi.Domain", "127.0.0.1" );
		public static readonly int Port = Config.Get( "RestApi.Port", 8080 );

		public static void Initialize()
		{
			EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
		}

		private static void EventSink_ServerStarted()
		{
			if ( !Enabled )
				return;

			var httpServer = new HttpServer( Domain, Port );
			var httpRouter = new HttpRouter();
			Weave( httpRouter );

			var apiServer = new ApiServer( httpServer, httpRouter );
			apiServer.Start();
		}

		private static void Weave( HttpRouter router )
		{
			var types = ScriptCompiler.Libraries.SelectMany( library => library.Types );

			foreach ( var type in types )
			{
				var attr = type.GetCustomAttributes<PathAttribute>( false ).FirstOrDefault();

				if ( attr != null )
					router.RegisterLocator( new Route( attr.Path ), type );
			}
		}

		private readonly HttpServer m_Server;
		private readonly HttpRouter m_Router;

		public ApiServer( HttpServer server, HttpRouter router )
		{
			m_Server = server;
			m_Router = router;

			// Bind router to the server
			m_Server.HandleRequest = m_Router.ProcessRequest;
		}

		public void Start()
		{
			try
			{
				m_Server.Start();

				foreach ( string prefix in m_Server.GetPrefixes() )
					log.Info( "Listening on {0}", prefix );
			}
			catch ( Exception e )
			{
				log.Warning( "Couldn't start: {0}", e );
			}
		}

		public void Dispose()
		{
			Stop();
		}

		public void Stop()
		{
			m_Server.Stop();

			log.Info( "Stopped" );
		}
	}
}
