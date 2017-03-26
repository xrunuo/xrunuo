using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Engines.PartySystem;

namespace Server.Misc
{
	public class ProtocolExtensions
	{
		private static PacketHandler[] m_Handlers = new PacketHandler[0x100];

		public static void Initialize()
		{
			PacketHandlers.Register( 0xF0, 0, true, new OnPacketReceive( DecodeBundledPacket ) );

			Register( 0x00, true, new OnPacketReceive( QueryPartyLocations ) );
		}

		public static void QueryPartyLocations( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Party party = Party.Get( from );

			if ( party != null )
			{
				AckPartyLocations ack = new AckPartyLocations( from, party );

				if ( ack.Stream.Length > 8 )
					state.Send( ack );
			}
		}

		public static void Register( int packetID, bool ingame, OnPacketReceive onReceive )
		{
			m_Handlers[packetID] = new PacketHandler( packetID, 0, ingame, onReceive );
		}

		public static PacketHandler GetHandler( int packetID )
		{
			if ( packetID >= 0 && packetID < m_Handlers.Length )
				return m_Handlers[packetID];

			return null;
		}

		public static void DecodeBundledPacket( NetState state, PacketReader pvSrc )
		{
			int packetID = pvSrc.ReadByte();

			PacketHandler ph = GetHandler( packetID );

			if ( ph != null )
			{
				if ( ph.Ingame && state.Mobile == null )
				{
					Console.WriteLine( "Client: {0}: Sent ingame packet (0xF0x{1:X2}) before having been attached to a mobile", state, packetID );
					state.Dispose();
				}
				else if ( ph.Ingame && state.Mobile.Deleted )
				{
					state.Dispose();
				}
				else
				{
					ph.OnReceive( state, pvSrc );
				}
			}
		}
	}

	public abstract class ProtocolExtension : Packet
	{
		public ProtocolExtension( int packetID, int capacity )
			: base( 0xF0 )
		{
			EnsureCapacity( 4 + capacity );

			m_Stream.Write( (byte) packetID );
		}
	}

	public class AckPartyLocations : ProtocolExtension
	{
		public AckPartyLocations( Mobile from, Party party )
			: base( 0x01, ( ( party.Members.Count - 1 ) * 9 ) + 4 )
		{
			foreach ( PartyMemberInfo pmi in party.Members )
			{
				if ( pmi == null || pmi.Mobile == from )
					continue;

				Mobile m = pmi.Mobile;

				if ( from.InUpdateRange( m ) && from.CanSee( m ) )
					continue;

				m_Stream.Write( (int) m.Serial );
				m_Stream.Write( (short) m.X );
				m_Stream.Write( (short) m.Y );
				m_Stream.Write( (byte) ( m.Map == null ? 0 : m.Map.MapID ) );
			}

			m_Stream.Write( (int) 0 );
		}
	}
}