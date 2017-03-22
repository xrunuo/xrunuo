using System;
using System.Net;
using System.Xml;

namespace Server.Configuration
{
	public class Network
	{
		private IPEndPoint[] m_Bind;

		public Network()
		{
		}

		public Network( XmlElement networkEl )
		{
			XmlNodeList nodeList = networkEl.GetElementsByTagName( "bind" );

			if ( nodeList.Count == 0 )
			{
				DefaultBind();
			}
			else
			{
				m_Bind = new IPEndPoint[nodeList.Count];

				for ( int i = 0; i < nodeList.Count; ++i )
					m_Bind[i] = ParseEndPoint( (XmlElement) nodeList[i] );
			}
		}

		private void DefaultBind()
		{
			m_Bind = new IPEndPoint[1];
			m_Bind[0] = new IPEndPoint( IPAddress.Any, 2593 );
		}

		private static IPEndPoint ParseEndPoint( XmlElement el )
		{
			IPAddress address = IPAddress.Any;
			int port = 2593;

			string addressString = el.GetAttribute( "address" );
			if ( addressString != null && addressString != "" )
				address = IPAddress.Parse( addressString );

			string portString = el.GetAttribute( "port" );
			if ( portString != null && portString != "" )
				port = Int32.Parse( portString );

			return new IPEndPoint( address, port );
		}

		public IPEndPoint[] Bind
		{
			get { return m_Bind; }
		}
	}
}