using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Web
{
	public class HttpServer : IDisposable
	{
		private readonly HttpListener _listener;

		public HttpServer( string domain = "localhost", int port = 8080 )
		{
			_listener = new HttpListener();
			_listener.Prefixes.Add( String.Format( @"http://{0}:{1}/", domain, port ) );
			_listener.AuthenticationSchemes = AuthenticationSchemes.Basic;
		}

		public IEnumerable<string> GetPrefixes()
		{
			return _listener.Prefixes;
		}

		public void Start()
		{
			_listener.Start();

			Task.Factory.StartNew( () => HandleRequests() );
		}

		public void Dispose()
		{
			Stop();
		}

		public void Stop()
		{
			_listener.Stop();
			_listener.Close();
		}

		private void HandleRequests()
		{
			while ( _listener.IsListening )
			{
				var context = _listener.GetContext();

				Task.Factory.StartNew( () =>
				{
					try
					{
						HandleRequest( context );
					}
					finally
					{
						context.Response.OutputStream.Close();
					}
				} );
			}
		}

		public Action<HttpListenerContext> HandleRequest;
	}
}
