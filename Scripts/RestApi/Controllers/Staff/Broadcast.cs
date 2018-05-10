using System;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	[Path( "/staff/broadcast" )]
	public class BroadcastController : BaseController
	{
		public override object HandleRequest( Request request )
		{
			if ( request.HttpMethod != "POST" )
				throw new NotSupportedException();

			var dto = request.AsDto<BroadcastRequest>();
			var broadcast = dto.Broadcast;

			Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Player, 0x482, String.Format( "Staff message from {0}:", broadcast.Name ) );
			Server.Scripts.Commands.CommandHandlers.BroadcastMessage( AccessLevel.Player, 0x482, broadcast.Message );

			return dto;
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
