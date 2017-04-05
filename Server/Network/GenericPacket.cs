using System;
using System.Collections.Generic;

namespace Server.Network
{
	public class GenericPacket : Packet
	{
		private static readonly Stack<GenericPacket> m_Stack = new Stack<GenericPacket>();

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
