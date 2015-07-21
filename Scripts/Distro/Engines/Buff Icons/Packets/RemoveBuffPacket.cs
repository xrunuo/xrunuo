using System;
using Server;
using Server.Engines.BuffIcons;

namespace Server.Network
{
	public class RemoveBuffPacket : Packet
	{
		public RemoveBuffPacket( Mobile mob, BuffInfo info )
			: this( mob, info.Id )
		{
		}

		public RemoveBuffPacket( Mobile mob, BuffIcon iconID )
			: base( 0xDF )
		{
			this.EnsureCapacity( 13 );
			m_Stream.Write( (int) mob.Serial );

			m_Stream.Write( (short) iconID );
			m_Stream.Write( (short) 0x0 ); // Type 0 for removal. 1 for add 2 for Data

			m_Stream.Fill( 4 );
		}
	}
}
