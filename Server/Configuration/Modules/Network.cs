using System;
using System.Net;
using System.Xml;

namespace Server.Configuration
{
	public class Network
	{
		public Network()
		{
		}

		public Network( XmlElement networkEl )
		{
			var nodeList = networkEl.GetElementsByTagName( "bind" );

			if ( nodeList.Count == 0 )
			{
				DefaultBind();
			}
			else
			{
				Bind = new IPEndPoint[nodeList.Count];

				for ( var i = 0; i < nodeList.Count; ++i )
					Bind[i] = ParseEndPoint( (XmlElement) nodeList[i] );
			}
		}

		private void DefaultBind()
		{
			Bind = new IPEndPoint[1];
			Bind[0] = new IPEndPoint( IPAddress.Any, 2593 );
		}

		private static IPEndPoint ParseEndPoint( XmlElement el )
		{
			var address = IPAddress.Any;
			var port = 2593;

			var addressString = el.GetAttribute( "address" );
			if ( addressString != null && addressString != "" )
				address = IPAddress.Parse( addressString );

			var portString = el.GetAttribute( "port" );
			if ( portString != null && portString != "" )
				port = Int32.Parse( portString );

			return new IPEndPoint( address, port );
		}

		public IPEndPoint[] Bind { get; private set; }
	}
}