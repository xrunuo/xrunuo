using System;
using System.Net;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/v1/staff/broadcast" )]
	public class BroadcastLocator : BaseLocator
	{
		public override BaseResource Locate( Parameters parameters )
		{
			return BroadcastResource.Instance;
		}
	}

	public class BroadcastResource : BaseResource
	{
		private static BroadcastResource _instance;

		public static BroadcastResource Instance
		{
			get
			{
				if ( _instance == null )
					_instance = new BroadcastResource();

				return _instance;
			}
		}

		private BroadcastResource()
		{
		}

		public override object HandleRequest( HttpListenerContext context )
		{
			if ( context.Request.HttpMethod != "POST" )
				throw new NotSupportedException();

			var request = GetRequestData<BroadcastRequest>( context );
			var broadcast = request.Broadcast;

			Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Player, 0x482, String.Format( "Staff message from {0}:", broadcast.Name ) );
			Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Player, 0x482, broadcast.Message );

			return request;
		}

		private class BroadcastRequest
		{
			public Broadcast Broadcast { get; set; }
		}

		private class Broadcast
		{
			public string Name { get; set; }
			public string Message { get; set; }
		}
	}
}
