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
using System.Collections.Generic;

namespace Server.Network
{
	public class GenericPacket : Packet
	{
		private static Stack<GenericPacket> m_Stack = new Stack<GenericPacket>();

		public static GenericPacket Instantiate( int id, int len )
		{
			GenericPacket p = null;

			lock ( m_Stack )
				if ( m_Stack.Count > 0 )
					p = m_Stack.Pop();

			if ( p == null )
				p = new GenericPacket();

			p.Initialize( id, len );

			return p;
		}

		public static void Release( GenericPacket p )
		{
			lock ( m_Stack )
				if ( !m_Stack.Contains( p ) )
					m_Stack.Push( p );
		}

		protected override void Free()
		{
			base.Free();

			Release( this );
		}

		public GenericPacket()
		{
		}
	}
}
