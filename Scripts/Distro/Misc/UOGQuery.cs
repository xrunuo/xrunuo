using System;
using Server;
using Server.Network;

namespace Server.Misc
{
	public class UOGQuery
	{
		public static void Configure()
		{
			PacketHandlers.Instance.Register( 0xF1, 0, false, new OnPacketReceive( OnReceive ) );
		}

		public static void OnReceive( GameClient state, PacketReader pvSrc )
		{
			if ( pvSrc.ReadByte() == 0xFF )
				state.Send( new UOGInfo( String.Format( ", Name={0}, Age={1}, Clients={2}, Items={3}, Chars={4}, Mem={5}K", Environment.Config.ServerName, (int) ( DateTime.Now - Server.Items.Clock.ServerStart ).TotalHours, GameServer.Instance.ClientCount, World.Instance.ItemCount, World.Instance.MobileCount, (int) ( System.GC.GetTotalMemory( false ) / 1024 ) ) ) );

			state.Dispose();
		}

		public sealed class UOGInfo : Packet
		{
			public UOGInfo( string str )
				: base( 0x52, str.Length + 6 ) // 'R'
			{
				m_Stream.WriteAsciiFixed( "unUO", 4 );
				m_Stream.WriteAsciiNull( str );
			}
		}
	}
}