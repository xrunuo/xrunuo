using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server
{
	public class KRUI
	{
		public static void Initialize()
		{
			PacketHandlers.Register( 0xE8, 13, true, new OnPacketReceive( RemoveHighlightKRUIElement ) );
			PacketHandlers.Register( 0xEB, 0, true, new OnPacketReceive( ReportUseKRHotbarIcon ) );
		}

		public static void RemoveHighlightKRUIElement( NetState state, PacketReader pvSrc )
		{
			Mobile m = World.FindMobile( (Serial) pvSrc.ReadInt32() );

			int elementID = pvSrc.ReadInt16();

			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null )
			{
				if ( elementID == 0x791F )
					pm.CheckKRStartingQuestStep( 15 );

				if ( elementID == 0x7919 )
					pm.CheckKRStartingQuestStep( 6 );
			}
		}

		public static void ReportUseKRHotbarIcon( NetState state, PacketReader pvSrc )
		{
			pvSrc.ReadInt32(); // 0x00010006

			/*
			 * Types:
			 * 0x1 - Spell
			 * 0x2 - Weapon Ability
			 * 0x3 - Skill
			 * 0x4 - Item
			 * 0x5 - Scroll
			 * 
			 * As per RUOSI packet guide:
			 * "Sometimes between 2.48.0.3 and 2.59.0.2 they changed it again: now type is always 0x06."
			 */
			int type = pvSrc.ReadByte();

			pvSrc.ReadByte(); // 0x00

			int objectID = pvSrc.ReadByte();
			objectID |= pvSrc.ReadByte() << 8;
			objectID |= pvSrc.ReadByte() << 16;
			objectID |= pvSrc.ReadByte() << 24;

			#region KR Starting Quest
			Item item = World.FindItem( (Serial) objectID );

			if ( type == 6 && item != null && item is MagicArrowScroll )
			{
				PlayerMobile pm = state.Mobile as PlayerMobile;

				if ( pm != null )
					pm.CheckKRStartingQuestStep( 23 );
			}
			#endregion
		}
	}

	public class ToggleHighlightKRUIElement : Packet
	{
		public ToggleHighlightKRUIElement( int serial, int elementID, string description, int commandID )
			: base( 0xE9, 75 )
		{
			m_Stream.Write( (short) elementID );
			m_Stream.Write( serial );
			m_Stream.WriteAsciiFixed( description, 64 );
			m_Stream.Write( commandID );
		}
	}

	public class ContinueHighlightKRUIElement : Packet
	{
		public ContinueHighlightKRUIElement( int serial, int elementID, int destSerial )
			: base( 0xE7, 12 )
		{
			m_Stream.Write( serial );
			m_Stream.Write( (short) elementID );
			m_Stream.Write( destSerial );
			m_Stream.Write( (byte) 1 );
		}
	}

	public class EnableKRHotbar : Packet
	{
		public static readonly Packet Enable = Packet.SetStatic( new EnableKRHotbar( true ) );
		public static readonly Packet Disable = Packet.SetStatic( new EnableKRHotbar( false ) );

		public EnableKRHotbar( bool enable )
			: base( 0xEA, 3 )
		{
			m_Stream.Write( (short) ( enable ? 1 : 0 ) );
		}
	}
}