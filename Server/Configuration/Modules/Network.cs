//
//  X-RunUO - Ultima Online Server Emulator
//  Copyright (C) 2015 Pedro Pardal
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

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