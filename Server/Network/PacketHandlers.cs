using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using Server;
using Server.Accounting;
using Server.ContextMenus;
using Server.Events;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;
using Server.Targeting;
using CV = Server.ClientVersion;
using CT = Server.ClientType;

namespace Server.Network
{
	public enum MessageType
	{
		Regular = 0x00,
		System = 0x01,
		Emote = 0x02,
		Label = 0x06,
		Focus = 0x07,
		Whisper = 0x08,
		Yell = 0x09,
		Spell = 0x0A,

		Guild = 0x0D,
		Alliance = 0x0E,
		GM = 0x0F,

		Encoded = 0xC0
	}

	public class PacketHandlers
	{
		private static PacketHandlers m_Instance;

		public static PacketHandlers Instance
		{
			get { return m_Instance; }
			set { m_Instance = value; }
		}

		private PacketHandler[] m_Handlers;

		private PacketHandler[] m_ExtendedHandlersLow;
		private Dictionary<int, PacketHandler> m_ExtendedHandlersHigh;

		private EncodedPacketHandler[] m_EncodedHandlersLow;
		private Dictionary<int, EncodedPacketHandler> m_EncodedHandlersHigh;

		public PacketHandler[] Handlers
		{
			get { return m_Handlers; }
		}

		public PacketHandlers()
		{
			m_Handlers = new PacketHandler[0x100];

			m_ExtendedHandlersLow = new PacketHandler[0x100];
			m_ExtendedHandlersHigh = new Dictionary<int, PacketHandler>();

			m_EncodedHandlersLow = new EncodedPacketHandler[0x100];
			m_EncodedHandlersHigh = new Dictionary<int, EncodedPacketHandler>();

			Register( 0x00, 104, false, new OnPacketReceive( CreateCharacter ) );
			Register( 0x01, 5, false, new OnPacketReceive( Disconnect ) );
			Register( 0x02, 7, true, new OnPacketReceive( MovementReq ) );
			Register( 0x03, 0, true, new OnPacketReceive( AsciiSpeech ) );
			Register( 0x04, 2, true, new OnPacketReceive( GodModeRequest ) );
			Register( 0x05, 5, true, new OnPacketReceive( AttackReq ) );
			Register( 0x06, 5, true, new OnPacketReceive( UseReq ) );
			Register( 0x07, 7, true, new OnPacketReceive( LiftReq ) );
			Register( 0x08, 15, true, new OnPacketReceive( DropReq ) );
			Register( 0x09, 5, true, new OnPacketReceive( LookReq ) );
			Register( 0x0A, 11, true, new OnPacketReceive( Edit ) );
			Register( 0x12, 0, true, new OnPacketReceive( TextCommand ) );
			Register( 0x13, 10, true, new OnPacketReceive( EquipReq ) );
			Register( 0x14, 6, true, new OnPacketReceive( ChangeZ ) );
			Register( 0x22, 3, true, new OnPacketReceive( Resynchronize ) );
			Register( 0x2C, 2, true, new OnPacketReceive( DeathStatusResponse ) );
			Register( 0x34, 10, true, new OnPacketReceive( MobileQuery ) );
			Register( 0x3A, 0, true, new OnPacketReceive( ChangeSkillLock ) );
			Register( 0x3B, 0, true, new OnPacketReceive( VendorBuyReply ) );
			Register( 0x47, 11, true, new OnPacketReceive( NewTerrain ) );
			Register( 0x48, 73, true, new OnPacketReceive( NewAnimData ) );
			Register( 0x58, 106, true, new OnPacketReceive( NewRegion ) );
			Register( 0x5D, 73, false, new OnPacketReceive( PlayCharacter ) );
			Register( 0x61, 9, true, new OnPacketReceive( DeleteStatic ) );
			Register( 0x6C, 19, true, new OnPacketReceive( TargetResponse ) );
			Register( 0x6F, 0, true, new OnPacketReceive( SecureTrade ) );
			Register( 0x72, 5, true, new OnPacketReceive( SetWarMode ) );
			Register( 0x73, 2, false, new OnPacketReceive( PingReq ) );
			Register( 0x75, 35, true, new OnPacketReceive( RenameRequest ) );
			Register( 0x79, 9, true, new OnPacketReceive( ResourceQuery ) );
			Register( 0x7E, 2, true, new OnPacketReceive( GodviewQuery ) );
			Register( 0x7D, 13, true, new OnPacketReceive( MenuResponse ) );
			Register( 0x80, 62, false, new OnPacketReceive( AccountLogin ) );
			Register( 0x83, 39, false, new OnPacketReceive( DeleteCharacter ) );
			Register( 0x8D, 0, false, new OnPacketReceive( CreateCharacterEnhanced ) );
			Register( 0x91, 65, false, new OnPacketReceive( GameLogin ) );
			Register( 0x95, 9, true, new OnPacketReceive( HuePickerResponse ) );
			Register( 0x96, 0, true, new OnPacketReceive( GameCentralMoniter ) );
			Register( 0x98, 0, true, new OnPacketReceive( MobileNameRequest ) );
			Register( 0x9A, 0, true, new OnPacketReceive( AsciiPromptResponse ) );
			Register( 0x9B, 258, true, new OnPacketReceive( HelpRequest ) );
			Register( 0x9D, 51, true, new OnPacketReceive( GMSingle ) );
			Register( 0x9F, 0, true, new OnPacketReceive( VendorSellReply ) );
			Register( 0xA0, 3, false, new OnPacketReceive( PlayServer ) );
			Register( 0xA4, 149, false, new OnPacketReceive( SystemInfo ) );
			Register( 0xA7, 4, true, new OnPacketReceive( RequestScrollWindow ) );
			Register( 0xAD, 0, true, new OnPacketReceive( UnicodeSpeech ) );
			Register( 0xB1, 0, true, new OnPacketReceive( DisplayGumpResponse ) );
			Register( 0xB5, 64, true, new OnPacketReceive( ChatRequest ) );
			Register( 0xB6, 9, true, new OnPacketReceive( ObjectHelpRequest ) );
			Register( 0xB8, 0, true, new OnPacketReceive( ProfileReq ) );
			Register( 0xBB, 9, false, new OnPacketReceive( AccountID ) );
			Register( 0xBD, 0, false, new OnPacketReceive( ClientVersion ) );
			Register( 0xBE, 0, true, new OnPacketReceive( AssistVersion ) );
			Register( 0xBF, 0, true, new OnPacketReceive( ExtendedCommand ) );
			Register( 0xC2, 0, true, new OnPacketReceive( UnicodePromptResponse ) );
			Register( 0xC8, 2, true, new OnPacketReceive( SetUpdateRange ) );
			Register( 0xC9, 6, true, new OnPacketReceive( TripTime ) );
			Register( 0xCA, 6, true, new OnPacketReceive( UTripTime ) );
			Register( 0xCF, 0, false, new OnPacketReceive( AccountLogin ) );
			Register( 0xD0, 0, true, new OnPacketReceive( ConfigurationFile ) );
			Register( 0xD1, 2, true, new OnPacketReceive( LogoutReq ) );
			Register( 0xD6, 0, true, new OnPacketReceive( BatchQueryProperties ) );
			Register( 0xD7, 0, true, new OnPacketReceive( EncodedCommand ) );
			Register( 0xE1, 0, false, new OnPacketReceive( ClientType ) );
			Register( 0xEC, 0, false, new OnPacketReceive( EquipMacro ) );
			Register( 0xED, 0, false, new OnPacketReceive( UnequipMacro ) );
			Register( 0xEF, 21, false, new OnPacketReceive( LoginServerSeed ) );
			Register( 0xF8, 106, false, new OnPacketReceive( CreateCharacterNew ) );

			RegisterExtended( 0x05, false, new OnPacketReceive( ScreenSize ) );
			RegisterExtended( 0x06, true, new OnPacketReceive( PartyMessage ) );
			RegisterExtended( 0x07, true, new OnPacketReceive( QuestArrow ) );
			RegisterExtended( 0x09, true, new OnPacketReceive( DisarmRequest ) );
			RegisterExtended( 0x0A, true, new OnPacketReceive( StunRequest ) );
			RegisterExtended( 0x0B, false, new OnPacketReceive( Language ) );
			RegisterExtended( 0x0C, true, new OnPacketReceive( CloseStatus ) );
			RegisterExtended( 0x0E, true, new OnPacketReceive( Animate ) );
			RegisterExtended( 0x0F, false, new OnPacketReceive( Empty ) ); // What's this?
			RegisterExtended( 0x10, true, new OnPacketReceive( QueryProperties ) );
			RegisterExtended( 0x13, true, new OnPacketReceive( ContextMenuRequest ) );
			RegisterExtended( 0x15, true, new OnPacketReceive( ContextMenuResponse ) );
			RegisterExtended( 0x1A, true, new OnPacketReceive( StatLockChange ) );
			RegisterExtended( 0x1C, true, new OnPacketReceive( CastSpell ) );
			RegisterExtended( 0x24, false, new OnPacketReceive( UnhandledBF ) );
			RegisterExtended( 0x2C, true, new OnPacketReceive( TargetedItemUse ) );
			RegisterExtended( 0x2D, true, new OnPacketReceive( TargetedCastSpell ) );
			RegisterExtended( 0x2E, true, new OnPacketReceive( TargetedSkillUse ) );
			RegisterExtended( 0x30, true, new OnPacketReceive( TargetByResourceMacro ) );
			RegisterExtended( 0x32, true, new OnPacketReceive( RacialAbility ) );
			RegisterExtended( 0x33, true, new OnPacketReceive( MouseBoatMovement ) );

			RegisterEncoded( 0x19, true, new OnEncodedPacketReceive( SetAbility ) );
			RegisterEncoded( 0x28, true, new OnEncodedPacketReceive( GuildGumpRequest ) );

			RegisterEncoded( 0x32, true, new OnEncodedPacketReceive( QuestGumpRequest ) );
			RegisterEncoded( 0x1E, true, new OnEncodedPacketReceive( EquipLastWeaponMacro ) );
		}

		public void Register( int packetId, int length, bool ingame, OnPacketReceive onReceive )
		{
			m_Handlers[packetId] = new PacketHandler( packetId, length, ingame, onReceive );
		}

		public PacketHandler GetHandler( int packetID )
		{
			return m_Handlers[packetID];
		}

		public void RegisterExtended( int packetID, bool ingame, OnPacketReceive onReceive )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				m_ExtendedHandlersLow[packetID] = new PacketHandler( packetID, 0, ingame, onReceive );
			else
				m_ExtendedHandlersHigh[packetID] = new PacketHandler( packetID, 0, ingame, onReceive );
		}

		public PacketHandler GetExtendedHandler( int packetID )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				return m_ExtendedHandlersLow[packetID];
			else
				return m_ExtendedHandlersHigh[packetID];
		}

		public void RemoveExtendedHandler( int packetID )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				m_ExtendedHandlersLow[packetID] = null;
			else
				m_ExtendedHandlersHigh.Remove( packetID );
		}

		public void RegisterEncoded( int packetID, bool ingame, OnEncodedPacketReceive onReceive )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				m_EncodedHandlersLow[packetID] = new EncodedPacketHandler( packetID, ingame, onReceive );
			else
				m_EncodedHandlersHigh[packetID] = new EncodedPacketHandler( packetID, ingame, onReceive );
		}

		public EncodedPacketHandler GetEncodedHandler( int packetID )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				return m_EncodedHandlersLow[packetID];
			else
				return m_EncodedHandlersHigh[packetID];
		}

		public void RemoveEncodedHandler( int packetID )
		{
			if ( packetID >= 0 && packetID < 0x100 )
				m_EncodedHandlersLow[packetID] = null;
			else
				m_EncodedHandlersHigh.Remove( packetID );
		}

		private void UnhandledBF( NetState state, PacketReader pvSrc )
		{
		}

		public void Empty( NetState state, PacketReader pvSrc )
		{
		}

		public void SetAbility( NetState state, IEntity e, EncodedReader reader )
		{
			EventSink.InvokeSetAbility( new SetAbilityEventArgs( state.Mobile, reader.ReadInt32() ) );
		}

		public void EquipLastWeaponMacro( NetState state, IEntity e, EncodedReader reader )
		{
			EventSink.InvokeEquipLastWeaponMacroUsed( new EquipLastWeaponMacroEventArgs( state.Mobile ) );
		}

		public void GuildGumpRequest( NetState state, IEntity e, EncodedReader reader )
		{
			EventSink.InvokeGuildGumpRequest( new GuildGumpRequestArgs( state.Mobile ) );
		}

		public void QuestGumpRequest( NetState state, IEntity e, EncodedReader reader )
		{
			EventSink.InvokeQuestGumpRequest( new QuestGumpRequestArgs( state.Mobile ) );
		}

		public void EncodedCommand( NetState state, PacketReader pvSrc )
		{
			IEntity e = World.FindEntity( pvSrc.ReadInt32() );
			int packetID = pvSrc.ReadUInt16();

			EncodedPacketHandler ph = GetEncodedHandler( packetID );

			if ( ph != null )
			{
				if ( ph.Ingame && state.Mobile == null )
				{
					Console.WriteLine( "Client: {0}: Sent ingame packet (0xD7x{1:X2}) before having been attached to a mobile", state, packetID );
					state.Dispose();
				}
				else if ( ph.Ingame && state.Mobile.Deleted )
				{
					state.Dispose();
				}
				else
				{
					ph.OnReceive( state, e, new EncodedReader( pvSrc ) );
				}
			}
			else
			{
				pvSrc.Trace( state );
			}
		}

		public void RenameRequest( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Mobile targ = World.FindMobile( pvSrc.ReadInt32() );

			if ( targ != null )
				EventSink.InvokeRenameRequest( new RenameRequestEventArgs( from, targ, pvSrc.ReadStringSafe() ) );
		}

		public void ChatRequest( NetState state, PacketReader pvSrc )
		{
			EventSink.InvokeChatRequest( new ChatRequestEventArgs( state.Mobile ) );
		}

		public void SecureTrade( NetState state, PacketReader pvSrc )
		{
			switch ( pvSrc.ReadByte() )
			{
				case 1: // Cancel
					{
						Serial serial = pvSrc.ReadInt32();

						SecureTradeContainer cont = World.FindItem( serial ) as SecureTradeContainer;

						if ( cont != null && cont.Trade != null && ( cont.Trade.From.Mobile == state.Mobile || cont.Trade.To.Mobile == state.Mobile ) )
							cont.Trade.Cancel();

						break;
					}
				case 2: // Check
					{
						Serial serial = pvSrc.ReadInt32();

						SecureTradeContainer cont = World.FindItem( serial ) as SecureTradeContainer;

						if ( cont != null )
						{
							SecureTrade trade = cont.Trade;

							bool value = ( pvSrc.ReadInt32() != 0 );

							if ( trade != null && trade.From.Mobile == state.Mobile )
							{
								trade.From.Accepted = value;
								trade.Update();
							}
							else if ( trade != null && trade.To.Mobile == state.Mobile )
							{
								trade.To.Accepted = value;
								trade.Update();
							}
						}

						break;
					}
			}
		}

		public void VendorBuyReply( NetState state, PacketReader pvSrc )
		{
			pvSrc.Seek( 1, SeekOrigin.Begin );

			int msgSize = pvSrc.ReadUInt16();
			Mobile vendor = World.FindMobile( pvSrc.ReadInt32() );
			byte flag = pvSrc.ReadByte();

			if ( vendor == null )
			{
				return;
			}
			else if ( vendor.Deleted || !Utility.RangeCheck( vendor.Location, state.Mobile.Location, 10 ) )
			{
				state.Send( new EndVendorBuy( vendor ) );
				return;
			}

			if ( flag == 0x02 )
			{
				msgSize -= 1 + 2 + 4 + 1;

				if ( ( msgSize / 7 ) > 100 )
					return;

				List<BuyItemResponse> buyList = new List<BuyItemResponse>( msgSize / 7 );
				for ( ; msgSize > 0; msgSize -= 7 )
				{
					/*byte layer = */
					pvSrc.ReadByte();
					Serial serial = pvSrc.ReadInt32();
					int amount = pvSrc.ReadInt16();

					buyList.Add( new BuyItemResponse( serial, amount ) );
				}

				if ( buyList.Count > 0 )
				{
					IVendor v = vendor as IVendor;

					if ( v != null && v.OnBuyItems( state.Mobile, buyList ) )
						state.Send( new EndVendorBuy( vendor ) );
				}
			}
			else
			{
				state.Send( new EndVendorBuy( vendor ) );
			}
		}

		public void VendorSellReply( NetState state, PacketReader pvSrc )
		{
			Serial serial = pvSrc.ReadInt32();
			Mobile vendor = World.FindMobile( serial );

			if ( vendor == null )
			{
				return;
			}
			else if ( vendor.Deleted || !Utility.RangeCheck( vendor.Location, state.Mobile.Location, 10 ) )
			{
				state.Send( new EndVendorSell( vendor ) );
				return;
			}

			int count = pvSrc.ReadUInt16();
			if ( count < 100 && pvSrc.Size == ( 1 + 2 + 4 + 2 + ( count * 6 ) ) )
			{
				List<SellItemResponse> sellList = new List<SellItemResponse>( count );

				for ( int i = 0; i < count; i++ )
				{
					Item item = World.FindItem( pvSrc.ReadInt32() );
					int Amount = pvSrc.ReadInt16();

					if ( item != null && Amount > 0 )
						sellList.Add( new SellItemResponse( item, Amount ) );
				}

				if ( sellList.Count > 0 )
				{
					IVendor v = vendor as IVendor;

					if ( v != null && v.OnSellItems( state.Mobile, sellList ) )
						state.Send( new EndVendorSell( vendor ) );
				}
			}
		}

		public void DeleteCharacter( NetState state, PacketReader pvSrc )
		{
			pvSrc.Seek( 30, SeekOrigin.Current );
			int index = pvSrc.ReadInt32();

			EventSink.InvokeDeleteRequest( new DeleteRequestEventArgs( state, index ) );
		}

		public void ResourceQuery( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
			}
		}

		public void GameCentralMoniter( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				int type = pvSrc.ReadByte();
				int num1 = pvSrc.ReadInt32();

				Console.WriteLine( "God Client: {0}: Game central moniter", state );
				Console.WriteLine( " - Type: {0}", type );
				Console.WriteLine( " - Number: {0}", num1 );

				pvSrc.Trace( state );
			}
		}

		public void GodviewQuery( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				Console.WriteLine( "God Client: {0}: Godview query 0x{1:X}", state, pvSrc.ReadByte() );
			}
		}

		public void GMSingle( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
				pvSrc.Trace( state );
		}

		public void DeathStatusResponse( NetState state, PacketReader pvSrc )
		{
			// Ignored
		}

		public void ObjectHelpRequest( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			Serial serial = pvSrc.ReadInt32();
			/*int unk = */
			pvSrc.ReadByte();
			/*string lang = */
			pvSrc.ReadString( 3 );

			if ( serial.IsItem )
			{
				Item item = World.FindItem( serial );

				if ( item != null && from.Map == item.Map && item.GetWorldLocation().InUpdateRange( from.Location ) && from.CanSee( item ) )
					item.OnHelpRequest( from );
			}
			else if ( serial.IsMobile )
			{
				Mobile m = World.FindMobile( serial );

				if ( m != null && from.Map == m.Map && m.Location.InUpdateRange( from.Location ) && from.CanSee( m ) )
					m.OnHelpRequest( m );
			}
		}

		public void MobileNameRequest( NetState state, PacketReader pvSrc )
		{
			Mobile m = World.FindMobile( pvSrc.ReadInt32() );

			if ( m != null && state.Mobile.InUpdateRange( m ) && state.Mobile.CanSee( m ) )
				state.Send( new MobileName( m ) );
		}

		public void RequestScrollWindow( NetState state, PacketReader pvSrc )
		{
			/*int lastTip = */
			pvSrc.ReadInt16();
			/*int type = */
			pvSrc.ReadByte();
		}

		public void AttackReq( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Mobile m = World.FindMobile( pvSrc.ReadInt32() );

			if ( m != null )
				from.Attack( m );
		}

		public void HuePickerResponse( NetState state, PacketReader pvSrc )
		{
			int serial = pvSrc.ReadInt32();
			/*int value = */
			pvSrc.ReadInt16();
			int hue = pvSrc.ReadInt16() & 0x3FFF;
			hue = Utility.ClipDyedHue( hue );

			var picker = state.HuePickers.Where( p => p.Serial == serial ).FirstOrDefault();

			if ( picker != null )
			{
				state.RemoveHuePicker( picker );
				picker.OnResponse( hue );
			}
		}

		public void TripTime( NetState state, PacketReader pvSrc )
		{
			int unk1 = pvSrc.ReadByte();
			/*int unk2 = */
			pvSrc.ReadInt32();

			state.Send( new TripTimeResponse( unk1 ) );
		}

		public void UTripTime( NetState state, PacketReader pvSrc )
		{
			int unk1 = pvSrc.ReadByte();
			/*int unk2 = */
			pvSrc.ReadInt32();

			state.Send( new UTripTimeResponse( unk1 ) );
		}

		public void ChangeZ( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				int x = pvSrc.ReadInt16();
				int y = pvSrc.ReadInt16();
				int z = pvSrc.ReadSByte();

				Console.WriteLine( "God Client: {0}: Change Z ({1}, {2}, {3})", state, x, y, z );
			}
		}

		public void SystemInfo( NetState state, PacketReader pvSrc )
		{
			/*int v1 = */
			pvSrc.ReadByte();
			/*int v2 = */
			pvSrc.ReadUInt16();
			/*int v3 = */
			pvSrc.ReadByte();
			/*string s1 = */
			pvSrc.ReadString( 32 );
			/*string s2 = */
			pvSrc.ReadString( 32 );
			/*string s3 = */
			pvSrc.ReadString( 32 );
			/*string s4 = */
			pvSrc.ReadString( 32 );
			/*int v4 = */
			pvSrc.ReadUInt16();
			/*int v5 = */
			pvSrc.ReadUInt16();
			/*int v6 = */
			pvSrc.ReadInt32();
			/*int v7 = */
			pvSrc.ReadInt32();
			/*int v8 = */
			pvSrc.ReadInt32();
		}

		public void Edit( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				int type = pvSrc.ReadByte(); // 10 = static, 7 = npc, 4 = dynamic
				int x = pvSrc.ReadInt16();
				int y = pvSrc.ReadInt16();
				int id = pvSrc.ReadInt16();
				int z = pvSrc.ReadSByte();
				int hue = pvSrc.ReadUInt16();

				Console.WriteLine( "God Client: {0}: Edit {6} ({1}, {2}, {3}) 0x{4:X} (0x{5:X})", state, x, y, z, id, hue, type );
			}
		}

		public void DeleteStatic( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				int x = pvSrc.ReadInt16();
				int y = pvSrc.ReadInt16();
				int z = pvSrc.ReadInt16();
				int id = pvSrc.ReadUInt16();

				Console.WriteLine( "God Client: {0}: Delete Static ({1}, {2}, {3}) 0x{4:X}", state, x, y, z, id );
			}
		}

		public void NewAnimData( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				Console.WriteLine( "God Client: {0}: New tile animation", state );

				pvSrc.Trace( state );
			}
		}

		public void NewTerrain( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				int x = pvSrc.ReadInt16();
				int y = pvSrc.ReadInt16();
				int id = pvSrc.ReadUInt16();
				int width = pvSrc.ReadInt16();
				int height = pvSrc.ReadInt16();

				Console.WriteLine( "God Client: {0}: New Terrain ({1}, {2})+({3}, {4}) 0x{5:X4}", state, x, y, width, height, id );
			}
		}

		public void NewRegion( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
			{
				string name = pvSrc.ReadString( 40 );
				/*int unk = */
				pvSrc.ReadInt32();
				/*int x = */
				pvSrc.ReadInt16();
				/*int y = */
				pvSrc.ReadInt16();
				/*int width = */
				pvSrc.ReadInt16();
				/*int height = */
				pvSrc.ReadInt16();
				/*int zStart = */
				pvSrc.ReadInt16();
				/*int zEnd = */
				pvSrc.ReadInt16();
				string desc = pvSrc.ReadString( 40 );
				/*int soundFX = */
				pvSrc.ReadInt16();
				/*int music = */
				pvSrc.ReadInt16();
				/*int nightFX = */
				pvSrc.ReadInt16();
				/*int dungeon = */
				pvSrc.ReadByte();
				/*int light = */
				pvSrc.ReadInt16();

				Console.WriteLine( "God Client: {0}: New Region '{1}' ('{2}')", state, name, desc );
			}
		}

		public void AccountID( NetState state, PacketReader pvSrc )
		{
		}

		public bool VerifyGC( NetState state )
		{
			if ( state.Mobile == null || state.Mobile.AccessLevel <= AccessLevel.Counselor )
			{
				Console.WriteLine( "Warning: {0}: Player using godclient, disconnecting", state );
				state.Dispose();
				return false;
			}
			else
			{
				return true;
			}
		}

		public void TextCommand( NetState state, PacketReader pvSrc )
		{
			int type = pvSrc.ReadByte();
			string command = pvSrc.ReadString();

			Mobile m = state.Mobile;

			switch ( type )
			{
				case 0x00: // Go
					{
						if ( VerifyGC( state ) )
						{
							try
							{
								string[] split = command.Split( ' ' );

								int x = Utility.ToInt32( split[0] );
								int y = Utility.ToInt32( split[1] );

								int z;

								if ( split.Length >= 3 )
									z = Utility.ToInt32( split[2] );
								else if ( m.Map != null )
									z = m.Map.GetAverageZ( x, y );
								else
									z = 0;

								m.Location = new Point3D( x, y, z );
							}
							catch
							{
							}
						}

						break;
					}
				case 0xC7: // Animate
					{
						EventSink.InvokeAnimateRequest( new AnimateRequestEventArgs( m, command ) );

						break;
					}
				case 0x24: // Use skill
					{
						int skillIndex;

						try { skillIndex = Convert.ToInt32( command.Split( ' ' )[0] ); }
						catch { break; }

						try
						{
							m.UseSkill( skillIndex );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in UseSkill {0} > {1}: {2}", state.Mobile, skillIndex, e );
						}

						break;
					}
				case 0x43: // Open spellbook
					{
						int booktype;

						try { booktype = Convert.ToInt32( command ); }
						catch { booktype = 1; }

						EventSink.InvokeOpenSpellbookRequest( new OpenSpellbookRequestEventArgs( m, booktype ) );

						break;
					}
				case 0x27: // Cast spell from book
					{
						string[] split = command.Split( ' ' );

						if ( split.Length > 0 )
						{
							int spellId = Utility.ToInt32( split[0] ) - 1;
							int serial = split.Length > 1 ? Utility.ToInt32( split[1] ) : -1;

							try
							{
								m.CastSpell( spellId, book: World.FindItem( serial ) );
							}
							catch ( Exception e )
							{
								Logger.Error( "Exception disarmed in CastSpell I {0}, spell {1}: {2}", state.Mobile, spellId, e );
							}
						}

						break;
					}
				case 0x58: // Open door
					{
						try
						{
							EventSink.InvokeOpenDoorMacroUsed( new OpenDoorMacroEventArgs( m ) );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in OpenDoor {0}: {1}", state.Mobile, e );
						}

						break;
					}
				case 0x56: // Cast spell from macro
					{
						int spellId = Utility.ToInt32( command ) - 1;

						try
						{
							m.CastSpell( spellId );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in CastSpell II {0}, spell {1}: {2}", state.Mobile, spellId, e );
						}

						break;
					}
				case 0xF4: // Invoke virtues from macro
					{
						int virtueID = Utility.ToInt32( command );

						try
						{
							EventSink.InvokeVirtueMacroUsed( new VirtueMacroEventArgs( m, virtueID ) );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in VirtueMacroUsed {0}, virtueid: {1}, {2}", state.Mobile, virtueID, e );
						}

						break;
					}
				default:
					{
						Console.WriteLine( "Client: {0}: Unknown text-command type 0x{1:X2}: {2}", state, type, command );
						break;
					}
			}
		}

		public void GodModeRequest( NetState state, PacketReader pvSrc )
		{
			if ( VerifyGC( state ) )
				state.Send( new GodModeReply( pvSrc.ReadBoolean() ) );
		}

		public void AsciiPromptResponse( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Prompt p = from.Prompt;

			int senderSerial = pvSrc.ReadInt32();
			int promptId = pvSrc.ReadInt32();
			int type = pvSrc.ReadInt32();
			string text = pvSrc.ReadStringSafe();

			if ( text.Length > 128 )
				return;

			if ( p != null && p.Sender.Serial == senderSerial && p.TypeId == promptId )
			{
				from.Prompt = null;

				try
				{
					if ( type == 0 )
						p.OnCancel( from );
					else
						p.OnResponse( from, text );
				}
				catch ( Exception e )
				{
					Logger.Error( "Exception disarmed in AsciiPrompt response {0}, type {1}: {2}", state.Mobile, type, e );
				}
			}
		}

		public void UnicodePromptResponse( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Prompt p = from.Prompt;

			int senderSerial = pvSrc.ReadInt32();
			int promptId = pvSrc.ReadInt32();
			int type = pvSrc.ReadInt32();
			/*string lang = */
			pvSrc.ReadString( 4 );
			string text = pvSrc.ReadUnicodeStringLESafe();

			if ( text.Length > 128 )
				return;

			int promptSerial = ( p != null && p.Sender != null ) ? p.Sender.Serial.Value : from.Serial.Value;

			if ( p != null && promptSerial == senderSerial && p.TypeId == promptId )
			{
				from.Prompt = null;

				try
				{
					if ( type == 0 )
						p.OnCancel( from );
					else
						p.OnResponse( from, text );
				}
				catch ( Exception e )
				{
					Logger.Error( "Exception disarmed in UnicodePrompt response {0}, type {1}: {2}", state.Mobile, type, e );
				}
			}
		}

		public void MenuResponse( NetState state, PacketReader pvSrc )
		{
			int serial = pvSrc.ReadInt32();
			/*int menuID = */
			pvSrc.ReadInt16(); // unused in our implementation
			int index = pvSrc.ReadInt16();
			/*int itemID = */
			pvSrc.ReadInt16();
			/*int hue = */
			pvSrc.ReadInt16();

			var menu = state.Menus.Where( m => m.Serial == serial ).FirstOrDefault();

			if ( menu != null )
			{
				try
				{
					if ( index > 0 && index <= menu.EntryLength )
						menu.OnResponse( state, index - 1 );
					else
						menu.OnCancel( state );
				}
				catch ( Exception e )
				{
					Logger.Error( "Exception disarmed in menu response {0} > {1}[index]: {2}", state.Mobile, menu, e );
				}

				state.RemoveMenu( menu );
			}
		}

		public void ProfileReq( NetState state, PacketReader pvSrc )
		{
			int type = pvSrc.ReadByte();
			Serial serial = pvSrc.ReadInt32();

			Mobile beholder = state.Mobile;
			Mobile beheld = World.FindMobile( serial );

			if ( beheld == null )
				return;

			switch ( type )
			{
				case 0x00: // display request
					{
						EventSink.InvokeProfileRequest( new ProfileRequestEventArgs( beholder, beheld ) );

						break;
					}
				case 0x01: // edit request
					{
						pvSrc.ReadInt16(); // Skip
						int length = pvSrc.ReadUInt16();

						if ( length > 511 )
							return;

						string text = pvSrc.ReadUnicodeString( length );

						EventSink.InvokeChangeProfileRequest( new ChangeProfileRequestEventArgs( beholder, beheld, text ) );

						break;
					}
			}
		}

		public void Disconnect( NetState state, PacketReader pvSrc )
		{
			/*int minusOne = */
			pvSrc.ReadInt32();
		}

		public void LiftReq( NetState state, PacketReader pvSrc )
		{
			Serial serial = pvSrc.ReadInt32();
			int amount = pvSrc.ReadUInt16();
			Item item = World.FindItem( serial );

			bool rejected;
			LRReason reject;

			try
			{
				state.Mobile.Lift( item, amount, out rejected, out reject );
			}
			catch ( Exception e )
			{
				Logger.Error( "Exception disarmed in lift {0}, {1} x {2}: {3}", state.Mobile, item, amount, e );
			}
		}

		public void EquipReq( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			Item item = from.Holding;

			if ( item == null )
				return;

			from.Holding = null;

			pvSrc.Seek( 5, SeekOrigin.Current );
			Mobile to = World.FindMobile( pvSrc.ReadInt32() );

			if ( to == null )
				to = from;

			bool success = false;

			try
			{
				if ( to.AllowEquipFrom( from ) )
					success = to.EquipItem( item );
			}
			catch ( Exception e )
			{
				Logger.Error( "Exception disarmed in equip {0} < {1}: {2}", to, item, e );
			}

			if ( !success )
			{
				item.Bounce( from );
				from.Send( new InvalidDrop( item.Serial ) );
			}
			else
			{
				from.Send( ConfirmDrop.Instance );
			}

			item.ClearBounce();
		}

		public void DropReq( NetState state, PacketReader pvSrc )
		{
			Serial serial = (Serial) pvSrc.ReadInt32();

			int x = pvSrc.ReadInt16();
			int y = pvSrc.ReadInt16();
			int z = pvSrc.ReadSByte();

			byte gridloc = pvSrc.ReadByte(); // grid location

			Serial dest = pvSrc.ReadInt32();

			Point3D loc = new Point3D( x, y, z );

			Mobile from = state.Mobile;

			if ( dest.IsMobile )
			{
				Mobile m = World.FindMobile( dest );

				try
				{
					if ( m != null )
						if ( from.Drop( m, loc ) )
							state.Send( ConfirmDrop.Instance );
						else
							state.Send( new InvalidDrop( serial ) );
				}
				catch ( Exception e )
				{
					Logger.Error( "Exception disarmed in drop {0} > {1}: {2}", from, m, e );
				}
			}
			else if ( dest.IsItem )
			{
				Item i = World.FindItem( dest );

				try
				{
					if ( i != null )
						if ( from.Drop( i, loc, gridloc ) )
							state.Send( ConfirmDrop.Instance );
						else
							state.Send( new InvalidDrop( serial ) );
				}
				catch ( Exception e )
				{
					Logger.Error( "Exception disarmed in drop {0} > {1}: {2}", from, i, e );
				}
			}
			else
			{
				if ( from.Drop( loc ) )
					state.Send( ConfirmDrop.Instance );
				else
					state.Send( new InvalidDrop( serial ) );
			}
		}

		public void ConfigurationFile( NetState state, PacketReader pvSrc )
		{
		}

		public void LogoutReq( NetState state, PacketReader pvSrc )
		{
			state.Send( LogoutAck.Instance );
		}

		public void ChangeSkillLock( NetState state, PacketReader pvSrc )
		{
			Skill s = state.Mobile.Skills[pvSrc.ReadInt16()];

			if ( s != null )
				s.SetLockNoRelay( (SkillLock) pvSrc.ReadByte() );
		}

		public void HelpRequest( NetState state, PacketReader pvSrc )
		{
			EventSink.InvokeHelpRequest( new HelpRequestEventArgs( state.Mobile ) );
		}

		public void TargetResponse( NetState state, PacketReader pvSrc )
		{
			int type = pvSrc.ReadByte();
			/*int targetID = */
			pvSrc.ReadInt32();
			/*int flags = */
			pvSrc.ReadByte();
			Serial serial = pvSrc.ReadInt32();
			int x = pvSrc.ReadInt16(), y = pvSrc.ReadInt16(), z = pvSrc.ReadInt16();
			int graphic = pvSrc.ReadInt16();

			Mobile from = state.Mobile;

			Target t = from.Target;

			if ( t == null )
			{
				state.NullTargets++;

				if ( state.NullTargets > NetState.MaxNullTargets )
				{
					state.Dispose();
					return;
				}
			}
			else
			{
				state.NullTargets = 0;

				if ( x == -1 && y == -1 && !serial.IsValid )
				{
					// User pressed escape
					t.Cancel( from, TargetCancelType.Canceled );
				}
				else
				{
					object toTarget;

					if ( type == 1 )
					{
						if ( graphic == 0 )
						{
							toTarget = new LandTarget( new Point3D( x, y, z ), from.Map );
						}
						else
						{
							Map map = from.Map;

							if ( map == null || map == Map.Internal )
							{
								t.Cancel( from, TargetCancelType.Canceled );
								return;
							}
							else
							{
								Tile[] tiles = map.Tiles.GetStaticTiles( x, y, !t.DisallowMultis );

								bool valid = false;

								for ( int i = 0; !valid && i < tiles.Length; ++i )
								{
									bool surface = ( TileData.ItemTable[tiles[i].ID & TileData.MaxItemValue].Flags & TileFlag.Surface ) != 0;
									int zOffset = surface ? tiles[i].Height : 0;

									if ( ( tiles[i].Z + zOffset ) == z && ( tiles[i].ID & TileData.MaxItemValue ) == ( graphic & TileData.MaxItemValue ) )
									{
										valid = true;
										z -= zOffset;
									}
								}

								if ( !valid )
								{
									t.Cancel( from, TargetCancelType.Canceled );
									return;
								}
								else
								{
									toTarget = new StaticTarget( new Point3D( x, y, z ), graphic );
								}
							}
						}
					}
					else if ( serial.IsMobile )
					{
						toTarget = World.FindMobile( serial );
					}
					else if ( serial.IsItem )
					{
						toTarget = World.FindItem( serial );
					}
					else
					{
						t.Cancel( from, TargetCancelType.Canceled );
						return;
					}

					try
					{
						t.Invoke( from, toTarget );
					}
					catch ( Exception e )
					{
						Logger.Error( "Exception disarmed in target {0} > {1} > {2}: {3}", from, t, toTarget, e );
					}
				}
			}
		}

		public void DisplayGumpResponse( NetState state, PacketReader pvSrc )
		{
			int serial = pvSrc.ReadInt32();
			int typeID = pvSrc.ReadInt32();
			int buttonID = pvSrc.ReadInt32();

			foreach ( Gump gump in state.Gumps )
			{
				if ( gump.Serial == serial && gump.TypeID == typeID )
				{
					int switchCount = pvSrc.ReadInt32();

					if ( switchCount < 0 || switchCount > gump.m_Switches )
					{
						Console.WriteLine( "Client: {0}: Invalid gump response, disconnecting...", state );
						state.Dispose();
						return;
					}

					int[] switches = new int[switchCount];

					for ( int j = 0; j < switches.Length; ++j )
						switches[j] = pvSrc.ReadInt32();

					int textCount = pvSrc.ReadInt32();

					if ( textCount < 0 || textCount > gump.m_TextEntries )
					{
						Console.WriteLine( "Client: {0}: Invalid gump response, disconnecting...", state );
						state.Dispose();
						return;
					}

					TextRelay[] textEntries = new TextRelay[textCount];

					for ( int j = 0; j < textEntries.Length; ++j )
					{
						int entryID = pvSrc.ReadUInt16();
						int textLength = pvSrc.ReadUInt16();

						if ( textLength > 239 )
							return;

						string text = pvSrc.ReadUnicodeStringSafe( textLength );
						textEntries[j] = new TextRelay( entryID, text );
					}

					try
					{
						gump.OnResponse( state, new RelayInfo( buttonID, switches, textEntries ) );
					}
					catch ( Exception e )
					{
						Logger.Error( "Exception disarmed in gump response of {0}: {1}", gump, e );
					}

					state.RemoveGump( gump );
					return;
				}
			}

			if ( typeID == 461 )
			{ // Virtue gump
				int switchCount = pvSrc.ReadInt32();

				if ( buttonID == 1 && switchCount > 0 )
				{
					Mobile beheld = World.FindMobile( pvSrc.ReadInt32() );

					if ( beheld != null )
						EventSink.InvokeVirtueGumpRequest( new VirtueGumpRequestEventArgs( state.Mobile, beheld ) );
				}
				else if ( buttonID == 1971 )
				{
					EventSink.InvokeVirtueGumpRequest( new VirtueGumpRequestEventArgs( state.Mobile, state.Mobile ) );
				}
				else
				{
					Mobile beheld = World.FindMobile( serial );

					if ( beheld != null )
						EventSink.InvokeVirtueItemRequest( new VirtueItemRequestEventArgs( state.Mobile, beheld, buttonID ) );
				}
			}
		}

		public void SetWarMode( NetState state, PacketReader pvSrc )
		{
			state.Mobile.DelayChangeWarmode( pvSrc.ReadBoolean() );
		}

		public void Resynchronize( NetState state, PacketReader pvSrc )
		{
			Mobile m = state.Mobile;

			state.Send( new MobileUpdate( m ) );
			state.Send( new MobileIncoming( m, m ) );

			m.SendEverything();

			state.Sequence = 0;

			m.ClearFastwalkStack();
		}

		private int[] m_EmptyInts = new int[0];

		public void AsciiSpeech( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			MessageType type = (MessageType) pvSrc.ReadByte();
			int hue = pvSrc.ReadInt16();
			pvSrc.ReadInt16(); // font
			string text = pvSrc.ReadStringSafe().Trim();

			if ( text.Length <= 0 || text.Length > 128 )
				return;

			if ( !Enum.IsDefined( typeof( MessageType ), type ) )
				type = MessageType.Regular;

			from.DoSpeech( text, m_EmptyInts, type, Utility.ClipDyedHue( hue ) );
		}

		private KeywordList m_KeywordList = new KeywordList();

		public void UnicodeSpeech( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			MessageType type = (MessageType) pvSrc.ReadByte();
			int hue = pvSrc.ReadInt16();
			pvSrc.ReadInt16(); // font
			string lang = pvSrc.ReadString( 4 );
			string text;

			bool isEncoded = ( type & MessageType.Encoded ) != 0;
			int[] keywords;

			if ( isEncoded )
			{
				int value = pvSrc.ReadInt16();
				int count = ( value & 0xFFF0 ) >> 4;
				int hold = value & 0xF;

				if ( count < 0 || count > 50 )
					return;

				KeywordList keyList = m_KeywordList;

				for ( int i = 0; i < count; ++i )
				{
					int speechID;

					if ( ( i & 1 ) == 0 )
					{
						hold <<= 8;
						hold |= pvSrc.ReadByte();
						speechID = hold;
						hold = 0;
					}
					else
					{
						value = pvSrc.ReadInt16();
						speechID = ( value & 0xFFF0 ) >> 4;
						hold = value & 0xF;
					}

					if ( !keyList.Contains( speechID ) )
						keyList.Add( speechID );
				}

				text = pvSrc.ReadUTF8StringSafe();

				keywords = keyList.ToArray();
			}
			else
			{
				text = pvSrc.ReadUnicodeStringSafe();

				keywords = m_EmptyInts;
			}

			text = text.Trim();

			if ( text.Length <= 0 || text.Length > 128 )
				return;

			type &= ~MessageType.Encoded;

			if ( !Enum.IsDefined( typeof( MessageType ), type ) )
				type = MessageType.Regular;

			from.Language = lang;
			from.DoSpeech( text, keywords, type, Utility.ClipDyedHue( hue ) );
		}

		public void UseReq( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			if ( from.AccessLevel >= AccessLevel.GameMaster || DateTime.UtcNow >= from.NextActionTime )
			{
				int value = pvSrc.ReadInt32();

				if ( ( value & ~0x7FFFFFFF ) != 0 )
				{
					from.OnPaperdollRequest();
				}
				else
				{
					Serial s = value;

					if ( s.IsMobile )
					{
						Mobile m = World.FindMobile( s );

						try
						{
							if ( m != null && !m.Deleted )
								from.Use( m );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in use {0} > {1}: {2}", from, m, e );
						}
					}
					else if ( s.IsItem )
					{
						Item item = World.FindItem( s );

						try
						{
							if ( item != null && !item.Deleted )
								from.Use( item );
						}
						catch ( Exception e )
						{
							Logger.Error( "Exception disarmed in use {0} > {1}: {2}", from, item, e );
						}
					}
				}

				from.NextActionTime = DateTime.UtcNow + TimeSpan.FromSeconds( 0.5 );
			}
			else
			{
				from.SendActionMessage();
			}
		}

		public void LookReq( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			Serial s = pvSrc.ReadInt32();

			if ( s.IsMobile )
			{
				Mobile m = World.FindMobile( s );

				if ( m != null && from.CanSee( m ) && m.InUpdateRange( from ) )
					m.OnAosSingleClick( from );
			}
			else if ( s.IsItem )
			{
				Item item = World.FindItem( s );

				if ( item != null && !item.Deleted && from.CanSee( item ) && from.InUpdateRange( item.GetWorldLocation() ) )
					item.OnAosSingleClick( from );
			}
		}

		public void PingReq( NetState state, PacketReader pvSrc )
		{
			state.Send( PingAck.Instantiate( pvSrc.ReadByte() ) );
		}

		public void SetUpdateRange( NetState state, PacketReader pvSrc )
		{
			state.Send( ChangeUpdateRange.Instantiate( 18 ) );
		}

		public void MovementReq( NetState state, PacketReader pvSrc )
		{
			Direction dir = (Direction) pvSrc.ReadByte();
			int seq = pvSrc.ReadByte();
			/*int key = */
			pvSrc.ReadInt32();

			Mobile m = state.Mobile;

			if ( ( state.Sequence == 0 && seq != 0 ) || !m.Move( dir ) )
			{
				state.Send( new MovementRej( seq, m ) );
				state.Sequence = 0;

				m.ClearFastwalkStack();
			}
			else
			{
				++seq;

				if ( seq == 256 )
					seq = 1;

				state.Sequence = seq;
			}
		}

		public int[] m_ValidAnimations = new int[]
			{
				6, 21, 32, 33,
				100, 101, 102,
				103, 104, 105,
				106, 107, 108,
				109, 110, 111,
				112, 113, 114,
				115, 116, 117,
				118, 119, 120,
				121, 123, 124,
				125, 126, 127,
				128
			};

		public int[] ValidAnimations { get { return m_ValidAnimations; } set { m_ValidAnimations = value; } }

		public void Animate( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			int action = pvSrc.ReadInt32();

			bool isValidAnimation = ValidAnimations.Contains( action );

			if ( isValidAnimation && from.Alive && from.Body.IsHuman && !from.Mounted )
				from.Animate( action, 7, 1, true, false, 0 );
		}

		public void QuestArrow( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			bool rightClick = pvSrc.ReadBoolean();

			if ( from.QuestArrow != null )
				from.QuestArrow.OnClick( rightClick );
		}

		public void ExtendedCommand( NetState state, PacketReader pvSrc )
		{
			int packetId = pvSrc.ReadUInt16();

			PacketHandler ph = GetExtendedHandler( packetId );

			if ( ph != null )
			{
				if ( ph.Ingame && state.Mobile == null )
				{
					Console.WriteLine( "Client: {0}: Sent ingame packet (0xBFx{1:X2}) before having been attached to a mobile", state, packetId );
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
			else
			{
				pvSrc.Trace( state );
			}
		}

		public void CastSpell( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			Item spellbook = null;

			if ( pvSrc.ReadInt16() == 1 )
				spellbook = World.FindItem( pvSrc.ReadInt32() );

			int spellId = pvSrc.ReadInt16() - 1;

			from.CastSpell( spellId, spellbook );
		}

		public void TargetedCastSpell( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			int spellId = pvSrc.ReadInt16() - 1;

			from.CastSpell( spellId, target: World.FindEntity( pvSrc.ReadInt32() ) );
		}

		public void TargetedSkillUse( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			int skillId = pvSrc.ReadInt16();
			IEntity target = World.FindEntity( pvSrc.ReadInt32() );

			Target oldTarget = from.Target;
			from.TargetLocked = true;

			from.UseSkill( skillId );

			Target newTarget = from.Target;

			if ( oldTarget != newTarget && newTarget != null && target != null )
				newTarget.Invoke( from, target );

			from.TargetLocked = false;
		}

		public void TargetedItemUse( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			Item item = World.FindItem( pvSrc.ReadInt32() );
			IEntity target = World.FindEntity( pvSrc.ReadInt32() );

			if ( item != null )
			{
				Target oldTarget = from.Target;
				from.TargetLocked = true;

				from.Use( item );

				Target newTarget = from.Target;

				if ( oldTarget != newTarget && newTarget != null && target != null )
					newTarget.Invoke( from, target );

				from.TargetLocked = false;
			}
		}

		public void EquipMacro( NetState ns, PacketReader pvSrc )
		{
			int count = pvSrc.ReadByte();

			List<int> serialList = new List<int>( count );

			for ( int i = 0; i < count; ++i )
			{
				Serial s = pvSrc.ReadInt32();
				serialList.Add( s );
			}

			EventSink.InvokeEquipMacro( new EquipMacroEventArgs( ns, serialList ) );
		}

		public void UnequipMacro( NetState ns, PacketReader pvSrc )
		{
			int count = pvSrc.ReadByte();

			List<int> layers = new List<int>( count );

			for ( int i = 0; i < count; ++i )
			{
				int s = pvSrc.ReadInt16();
				layers.Add( s );
			}

			EventSink.InvokeUnequipMacro( new UnequipMacroEventArgs( ns, layers ) );
		}

		public void TargetByResourceMacro( NetState ns, PacketReader pvSrc )
		{
			/*int command = */
			pvSrc.ReadInt16();
			Serial serial = pvSrc.ReadInt32();
			int resourceType = pvSrc.ReadInt16();

			if ( serial.IsItem )
			{
				TargetByResourceMacroEventArgs e = new TargetByResourceMacroEventArgs( ns, World.FindItem( serial ), resourceType );
				EventSink.InvokeTargetByResourceMacro( e );
			}
		}

		public void BatchQueryProperties( NetState state, PacketReader pvSrc )
		{
			if ( !ObjectPropertyListPacket.Enabled )
				return;

			Mobile from = state.Mobile;

			int length = pvSrc.Size - 3;

			if ( length < 0 || ( length % 4 ) != 0 )
				return;

			int count = length / 4;

			for ( int i = 0; i < count; ++i )
			{
				Serial s = pvSrc.ReadInt32();

				if ( s.IsMobile )
				{
					Mobile m = World.FindMobile( s );

					if ( m != null && from.CanSee( m ) && from.InUpdateRange( m ) )
						m.SendPropertiesTo( from );
				}
				else if ( s.IsItem )
				{
					Item item = World.FindItem( s );

					if ( item != null && !item.Deleted && from.CanSee( item ) && from.InUpdateRange( item.GetWorldLocation() ) )
						item.SendPropertiesTo( from );
				}
			}
		}

		public void QueryProperties( NetState state, PacketReader pvSrc )
		{
			if ( !ObjectPropertyListPacket.Enabled )
				return;

			Mobile from = state.Mobile;

			Serial s = pvSrc.ReadInt32();

			if ( s.IsMobile )
			{
				Mobile m = World.FindMobile( s );

				if ( m != null && from.CanSee( m ) && from.InUpdateRange( m ) )
					m.SendPropertiesTo( from );
			}
			else if ( s.IsItem )
			{
				Item item = World.FindItem( s );

				if ( item != null && !item.Deleted && from.CanSee( item ) && from.InUpdateRange( item.GetWorldLocation() ) )
					item.SendPropertiesTo( from );
			}
		}

		public void PartyMessage( NetState state, PacketReader pvSrc )
		{
			if ( state.Mobile == null )
				return;

			switch ( pvSrc.ReadByte() )
			{
				case 0x01:
					PartyMessage_AddMember( state, pvSrc );
					break;
				case 0x02:
					PartyMessage_RemoveMember( state, pvSrc );
					break;
				case 0x03:
					PartyMessage_PrivateMessage( state, pvSrc );
					break;
				case 0x04:
					PartyMessage_PublicMessage( state, pvSrc );
					break;
				case 0x06:
					PartyMessage_SetCanLoot( state, pvSrc );
					break;
				case 0x08:
					PartyMessage_Accept( state, pvSrc );
					break;
				case 0x09:
					PartyMessage_Decline( state, pvSrc );
					break;
				default:
					pvSrc.Trace( state );
					break;
			}
		}

		public void PartyMessage_AddMember( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnAdd( state.Mobile );
		}

		public void PartyMessage_RemoveMember( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnRemove( state.Mobile, World.FindMobile( pvSrc.ReadInt32() ) );
		}

		public void PartyMessage_PrivateMessage( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnPrivateMessage( state.Mobile, World.FindMobile( pvSrc.ReadInt32() ), pvSrc.ReadUnicodeStringSafe() );
		}

		public void PartyMessage_PublicMessage( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnPublicMessage( state.Mobile, pvSrc.ReadUnicodeStringSafe() );
		}

		public void PartyMessage_SetCanLoot( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnSetCanLoot( state.Mobile, pvSrc.ReadBoolean() );
		}

		public void PartyMessage_Accept( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnAccept( state.Mobile, World.FindMobile( pvSrc.ReadInt32() ) );
		}

		public void PartyMessage_Decline( NetState state, PacketReader pvSrc )
		{
			if ( PartyCommands.Handler != null )
				PartyCommands.Handler.OnDecline( state.Mobile, World.FindMobile( pvSrc.ReadInt32() ) );
		}

		public void StunRequest( NetState state, PacketReader pvSrc )
		{
			EventSink.InvokeStunRequest( new StunRequestEventArgs( state.Mobile ) );
		}

		public void DisarmRequest( NetState state, PacketReader pvSrc )
		{
			EventSink.InvokeDisarmRequest( new DisarmRequestEventArgs( state.Mobile ) );
		}

		public void StatLockChange( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			int stat = pvSrc.ReadByte();
			int lockValue = pvSrc.ReadByte();

			if ( lockValue > 2 )
				lockValue = 0;

			switch ( stat )
			{
				case 0:
					from.StrLock = (StatLockType) lockValue;
					break;
				case 1:
					from.DexLock = (StatLockType) lockValue;
					break;
				case 2:
					from.IntLock = (StatLockType) lockValue;
					break;
			}
		}

		public void ScreenSize( NetState state, PacketReader pvSrc )
		{
			/*int width = */
			pvSrc.ReadInt32();
			/*int unk = */
			pvSrc.ReadInt32();
		}

		public void ContextMenuResponse( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			if ( from != null )
			{
				ContextMenu menu = from.ContextMenu;

				from.ContextMenu = null;

				if ( menu != null && from != null && from == menu.From )
				{
					IEntity entity = World.FindEntity( pvSrc.ReadInt32() );

					if ( entity != null && entity == menu.Target && from.CanSee( entity ) )
					{
						IPoint3D p;

						if ( entity is Mobile )
							p = entity.Location;
						else if ( entity is Item )
							p = ( (Item) entity ).GetWorldLocation();
						else
							return;

						int index = pvSrc.ReadUInt16();

						if ( index >= 0 && index < menu.Entries.Length )
						{
							ContextMenuEntry e = (ContextMenuEntry) menu.Entries[index];

							int range = e.Range;

							if ( range == -1 )
								range = 18;

							if ( e.Enabled && from.InRange( p, range ) )
								e.OnClick();
						}
					}
				}
			}
		}

		public void ContextMenuRequest( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;
			IEntity target = World.FindEntity( pvSrc.ReadInt32() );

			if ( from != null && target != null && from.Map == target.Map && from.CanSee( target ) )
			{
				if ( target is Mobile && !from.InUpdateRange( target ) )
					return;
				else if ( target is Item && !from.InUpdateRange( ( (Item) target ).GetWorldLocation() ) )
					return;

				if ( !from.CheckContextMenuDisplay( target ) )
					return;

				ContextMenu c = new ContextMenu( from, target );

				if ( c.Entries.Length > 0 )
				{
					if ( target is Item )
					{
						object root = ( (Item) target ).RootParent;

						if ( root is Mobile && root != from && ( (Mobile) root ).AccessLevel >= from.AccessLevel )
						{
							for ( int i = 0; i < c.Entries.Length; ++i )
							{
								if ( !c.Entries[i].NonLocalUse )
									c.Entries[i].Enabled = false;
							}
						}
					}

					from.ContextMenu = c;
				}
			}
		}

		public void CloseStatus( NetState state, PacketReader pvSrc )
		{
			/*Serial serial = (Serial) */
			pvSrc.ReadInt32();
		}

		public void RacialAbility( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			int abilityId = pvSrc.ReadInt16();

			try
			{
				from.UseRacialAbility( abilityId );
			}
			catch ( Exception e )
			{
				Logger.Error( "Exception disarmed in RacialAbility {0}, ability {1}: {2}", from, abilityId, e );
			}
		}

		public void MouseBoatMovement( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			pvSrc.ReadInt32(); // mob serial
			Direction direction = (Direction) pvSrc.ReadByte();
			pvSrc.ReadByte(); // direction again
			int speed = pvSrc.ReadByte();

			try
			{
				EventSink.InvokeBoatMovementRequest( new BoatMovementRequestEventArgs( from, direction, speed ) );
			}
			catch ( Exception e )
			{
				Logger.Error( "Exception disarmed in MouseBoatMovement {0}, direction {1}, speed {2}: {3}", from, direction, speed, e );
			}
		}

		public void Language( NetState state, PacketReader pvSrc )
		{
			string lang = pvSrc.ReadString( 4 );

			if ( state.Mobile != null )
				state.Mobile.Language = lang;
		}

		public void AssistVersion( NetState state, PacketReader pvSrc )
		{
			/*int unk = */
			pvSrc.ReadInt32();
			/*string av = */
			pvSrc.ReadString();
		}

		public void ClientVersion( NetState state, PacketReader pvSrc )
		{
			CV version = state.Version = new CV( pvSrc.ReadString() );

			EventSink.InvokeClientVersionReceived( new ClientVersionReceivedArgs( state, version ) );
		}

		public void MobileQuery( NetState state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			pvSrc.ReadInt32(); // 0xEDEDEDED
			int type = pvSrc.ReadByte();
			Mobile m = World.FindMobile( pvSrc.ReadInt32() );

			if ( m != null )
			{
				switch ( type )
				{
					case 0x00: // Unknown, sent by godclient
						{
							if ( VerifyGC( state ) )
								Console.WriteLine( "God Client: {0}: Query 0x{1:X2} on {2} '{3}'", state, type, m.Serial, m.Name );

							break;
						}
					case 0x04: // Stats
						{
							m.OnStatsQuery( from );
							break;
						}
					case 0x05:
						{
							m.OnSkillsQuery( from );
							break;
						}
					default:
						{
							pvSrc.Trace( state );
							break;
						}
				}
			}
		}

		public void PlayCharacter( NetState state, PacketReader pvSrc )
		{
			pvSrc.ReadInt32(); // 0xEDEDEDED

			/*string name = */
			pvSrc.ReadString( 30 );

			pvSrc.Seek( 2, SeekOrigin.Current );
			int flags = pvSrc.ReadInt32();
			pvSrc.Seek( 24, SeekOrigin.Current );

			int charSlot = pvSrc.ReadInt32();
			uint clientIP = pvSrc.ReadUInt32();

			clientIP = (uint) IPAddress.NetworkToHostOrder( (int) clientIP );
			state.ClientAddress = new IPAddress( (long) clientIP );

			IAccount a = state.Account;

			if ( Utility.IsUsingMulticlient( state, Core.Config.Login.MaxLoginsPerPC ) )
			{
				Console.WriteLine( "Login: {0}: Multiclient detected, disconnecting...", state );
				state.Send( new PopupMessage( PMMessage.LoginSyncError ) );
				state.Dispose();
				return;
			}

			if ( a == null || charSlot < 0 || charSlot >= a.Length )
			{
				state.Dispose();
			}
			else
			{
				Mobile m = a[charSlot];

				// Check if anyone is using this account
				for ( int i = 0; i < a.Length; ++i )
				{
					Mobile check = a[i];

					if ( check != null && check.Map != Map.Internal && check != m )
					{
						Console.WriteLine( "Login: {0}: Account in use", state );
						state.Send( new PopupMessage( PMMessage.CharInWorld ) );
						return;
					}
				}

				if ( m == null )
				{
					state.Dispose();
				}
				else
				{
					if ( m.NetState != null )
						m.NetState.Dispose();

					GameServer.Instance.ProcessDisposedQueue();

					state.BlockAllPackets = true;

					state.Flags = flags;

					state.Mobile = m;
					m.NetState = state;

					state.BlockAllPackets = false;
					DoLogin( state, m );
				}
			}
		}

		public void DoLogin( NetState state, Mobile m )
		{
			state.Send( new LoginConfirm( m ) );

			if ( m.Map != null )
				state.Send( new MapChange( m ) );

			state.Send( SeasonChange.Instantiate( m.GetSeason(), true ) );

			state.Send( SupportedFeatures.Instantiate( state ) );

			state.Sequence = 0;
			state.Send( new MobileUpdate( m ) );
			state.Send( new MobileUpdate( m ) );

			m.CheckLightLevels( true );

			state.Send( new MobileUpdate( m ) );

			state.Send( new MobileIncoming( m, m ) );
			state.Send( new MobileStatus( m, m ) );
			state.Send( Server.Network.SetWarMode.Instantiate( m.Warmode ) );

			m.SendEverything();

			state.Send( SupportedFeatures.Instantiate( state ) );
			state.Send( new MobileUpdate( m ) );
			state.Send( new MobileStatus( m, m ) );
			state.Send( Server.Network.SetWarMode.Instantiate( m.Warmode ) );
			state.Send( new MobileIncoming( m, m ) );

			state.Send( LoginComplete.Instance );
			state.Send( new CurrentTime() );
			state.Send( SeasonChange.Instantiate( m.GetSeason(), true ) );
			state.Send( new MapChange( m ) );

			try
			{
				EventSink.InvokeLogin( new LoginEventArgs( m ) );
			}
			catch ( Exception ex )
			{
				Logger.Error( "Exception disarmed in Login: {0}", ex );
			}

			m.ClearFastwalkStack();
		}

		public void CreateCharacter( NetState state, PacketReader pvSrc )
		{
			/*int unk1 = */
			pvSrc.ReadInt32();
			/*int unk2 = */
			pvSrc.ReadInt32();
			/*int unk3 = */
			pvSrc.ReadByte();
			string name = pvSrc.ReadString( 30 );

			pvSrc.Seek( 2, SeekOrigin.Current );
			int flags = pvSrc.ReadInt32();
			pvSrc.Seek( 8, SeekOrigin.Current );
			int prof = pvSrc.ReadByte();
			pvSrc.Seek( 15, SeekOrigin.Current );

			int genderRace = pvSrc.ReadByte();

			int str = pvSrc.ReadByte();
			int dex = pvSrc.ReadByte();
			int intl = pvSrc.ReadByte();
			int is1 = pvSrc.ReadByte();
			int vs1 = pvSrc.ReadByte();
			int is2 = pvSrc.ReadByte();
			int vs2 = pvSrc.ReadByte();
			int is3 = pvSrc.ReadByte();
			int vs3 = pvSrc.ReadByte();
			int hue = pvSrc.ReadUInt16();
			int hairVal = pvSrc.ReadInt16();
			int hairHue = pvSrc.ReadInt16();
			int hairValf = pvSrc.ReadInt16();
			int hairHuef = pvSrc.ReadInt16();
			pvSrc.ReadByte();
			int cityIndex = pvSrc.ReadByte();
			/*int charSlot = */
			pvSrc.ReadInt32();
			uint clientIP = pvSrc.ReadUInt32();
			int shirtHue = pvSrc.ReadInt16();
			int pantsHue = pvSrc.ReadInt16();

			/*
			0x00, 0x01
			0x02, 0x03 -> Human Male, Human Female
			0x04, 0x05 -> Elf Male, Elf Female
			0x05, 0x06 -> Gargoyle Male, Gargoyle Female
			*/

			bool female = ( ( genderRace % 2 ) != 0 );

			Race race = null;

			byte raceId = (byte) ( genderRace < 4 ? 0 : ( ( genderRace / 2 ) - 1 ) );
			race = Race.Races[raceId];

			if ( race == null )
				race = Race.DefaultRace;

			clientIP = (uint) IPAddress.NetworkToHostOrder( (int) clientIP );

			state.ClientAddress = new IPAddress( (long) clientIP );

			CityInfo[] info = state.CityInfo;
			IAccount a = state.Account;

			if ( Utility.IsUsingMulticlient( state, Core.Config.Login.MaxLoginsPerPC ) )
			{
				Console.WriteLine( "Login: {0}: Multiclient detected, disconnecting...", state );
				state.Send( new PopupMessage( PMMessage.LoginSyncError ) );
				state.Dispose();
				return;
			}

			if ( info == null || a == null || cityIndex < 0 || cityIndex >= info.Length )
			{
				state.Dispose();
			}
			else
			{
				// Check if anyone is using this account
				for ( int i = 0; i < a.Length; ++i )
				{
					Mobile check = a[i];

					if ( check != null && check.Map != Map.Internal )
					{
						Console.WriteLine( "Login: {0}: Account in use", state );
						state.Send( new PopupMessage( PMMessage.CharInWorld ) );
						return;
					}
				}

				state.Flags = flags;

				CreateCharRequestEventArgs args = new CreateCharRequestEventArgs(
					state, a,
					name, female, hue,
					str, dex, intl,
					info[cityIndex],
					new SkillNameValue[3]
					{
						new SkillNameValue( (SkillName)is1, vs1 ),
						new SkillNameValue( (SkillName)is2, vs2 ),
						new SkillNameValue( (SkillName)is3, vs3 ),
					},
					shirtHue, pantsHue,
					hairVal, hairHue,
					hairValf, hairHuef,
					prof,
					race
					);

				state.BlockAllPackets = true;

				try
				{
					EventSink.InvokeCreateCharRequest( args );
				}
				catch ( Exception ex )
				{
					Logger.Error( "Exception disarmed in CreateCharRequest {0}: {1}", name, ex );
				}

				Mobile m = args.Mobile;

				if ( m != null )
				{
					state.Mobile = m;
					m.NetState = state;

					state.BlockAllPackets = false;

					try
					{
						EventSink.InvokeCharacterCreated( new CharacterCreatedEventArgs( m ) );
					}
					catch ( Exception ex )
					{
						Logger.Error( "Exception disarmed in CharacterCreated {0}: {1}", m, ex );
					}

					DoLogin( state, m );
				}
				else
				{
					state.BlockAllPackets = false;
					state.Dispose();
				}
			}
		}

		public void CreateCharacterNew( NetState state, PacketReader pvSrc )
		{
			/*int unk1 = */
			pvSrc.ReadInt32();
			/*int unk2 = */
			pvSrc.ReadInt32();
			/*int unk3 = */
			pvSrc.ReadByte();
			string name = pvSrc.ReadString( 30 );

			pvSrc.Seek( 2, SeekOrigin.Current );
			int flags = pvSrc.ReadInt32();
			pvSrc.Seek( 8, SeekOrigin.Current );
			int prof = pvSrc.ReadByte();
			pvSrc.Seek( 15, SeekOrigin.Current );

			int genderRace = pvSrc.ReadByte();

			int str = pvSrc.ReadByte();
			int dex = pvSrc.ReadByte();
			int intl = pvSrc.ReadByte();
			int is1 = pvSrc.ReadByte();
			int vs1 = pvSrc.ReadByte();
			int is2 = pvSrc.ReadByte();
			int vs2 = pvSrc.ReadByte();
			int is3 = pvSrc.ReadByte();
			int vs3 = pvSrc.ReadByte();
			int is4 = pvSrc.ReadByte();
			int vs4 = pvSrc.ReadByte();
			int hue = pvSrc.ReadUInt16();
			int hairVal = pvSrc.ReadInt16();
			int hairHue = pvSrc.ReadInt16();
			int hairValf = pvSrc.ReadInt16();
			int hairHuef = pvSrc.ReadInt16();
			pvSrc.ReadByte();
			int cityIndex = pvSrc.ReadByte();
			/*int charSlot = */
			pvSrc.ReadInt32();
			uint clientIP = pvSrc.ReadUInt32();
			int shirtHue = pvSrc.ReadInt16();
			int pantsHue = pvSrc.ReadInt16();

			/*
			0x00, 0x01
			0x02, 0x03 -> Human Male, Human Female
			0x04, 0x05 -> Elf Male, Elf Female
			0x05, 0x06 -> Gargoyle Male, Gargoyle Female
			*/

			bool female = ( ( genderRace % 2 ) != 0 );

			Race race = null;

			byte raceId = (byte) ( genderRace < 4 ? 0 : ( ( genderRace / 2 ) - 1 ) );
			race = Race.Races[raceId];

			if ( race == null )
				race = Race.DefaultRace;

			clientIP = (uint) IPAddress.NetworkToHostOrder( (int) clientIP );

			state.ClientAddress = new IPAddress( (long) clientIP );

			CityInfo[] info = state.CityInfo;
			IAccount a = state.Account;

			if ( Utility.IsUsingMulticlient( state, Core.Config.Login.MaxLoginsPerPC ) )
			{
				Console.WriteLine( "Login: {0}: Multiclient detected, disconnecting...", state );
				state.Send( new PopupMessage( PMMessage.LoginSyncError ) );
				state.Dispose();
				return;
			}

			if ( info == null || a == null || cityIndex < 0 || cityIndex >= info.Length )
			{
				state.Dispose();
			}
			else
			{
				// Check if anyone is using this account
				for ( int i = 0; i < a.Length; ++i )
				{
					Mobile check = a[i];

					if ( check != null && check.Map != Map.Internal )
					{
						Console.WriteLine( "Login: {0}: Account in use", state );
						state.Send( new PopupMessage( PMMessage.CharInWorld ) );
						return;
					}
				}

				state.Flags = flags;

				CreateCharRequestEventArgs args = new CreateCharRequestEventArgs(
					state, a,
					name, female, hue,
					str, dex, intl,
					info[cityIndex],
					new SkillNameValue[4]
					{
						new SkillNameValue( (SkillName)is1, vs1 ),
						new SkillNameValue( (SkillName)is2, vs2 ),
						new SkillNameValue( (SkillName)is3, vs3 ),
						new SkillNameValue( (SkillName)is4, vs4 ),
					},
					shirtHue, pantsHue,
					hairVal, hairHue,
					hairValf, hairHuef,
					prof,
					race
					);

				state.BlockAllPackets = true;

				try
				{
					EventSink.InvokeCreateCharRequest( args );
				}
				catch ( Exception ex )
				{
					Logger.Error( "Exception disarmed in CreateCharRequest {0}: {1}", name, ex );
				}

				Mobile m = args.Mobile;

				if ( m != null )
				{
					state.Mobile = m;
					m.NetState = state;

					state.BlockAllPackets = false;

					try
					{
						EventSink.InvokeCharacterCreated( new CharacterCreatedEventArgs( m ) );
					}
					catch ( Exception ex )
					{
						Logger.Error( "Exception disarmed in CharacterCreated {0}: {1}", m, ex );
					}

					DoLogin( state, m );
				}
				else
				{
					state.BlockAllPackets = false;
					state.Dispose();
				}
			}
		}

		public void CreateCharacterEnhanced( NetState state, PacketReader pvSrc )
		{
			pvSrc.ReadInt32(); // 0xEDEDEDED

			/*int charSlot = */
			pvSrc.ReadInt32();

			string name = pvSrc.ReadString( 30 );

			pvSrc.Seek( 30, SeekOrigin.Current );

			int prof = pvSrc.ReadByte();
			int cityIndex = pvSrc.ReadByte();
			int genderId = pvSrc.ReadByte();
			int raceId = pvSrc.ReadByte();

			int str = pvSrc.ReadByte();
			int dex = pvSrc.ReadByte();
			int intl = pvSrc.ReadByte();

			int hue = pvSrc.ReadUInt16();

			pvSrc.Seek( 8, SeekOrigin.Current );

			int is1 = pvSrc.ReadByte();
			int vs1 = pvSrc.ReadByte();
			int is2 = pvSrc.ReadByte();
			int vs2 = pvSrc.ReadByte();
			int is3 = pvSrc.ReadByte();
			int vs3 = pvSrc.ReadByte();
			int is4 = pvSrc.ReadByte();
			int vs4 = pvSrc.ReadByte();

			pvSrc.Seek( 25, SeekOrigin.Current );

			pvSrc.ReadByte(); // 0x0B
			int hairHue = pvSrc.ReadInt16();
			int hairVal = pvSrc.ReadInt16();

			pvSrc.ReadByte(); // 0x0C
			int pantsHue = pvSrc.ReadInt16();
			/*int pantsVal = */
			pvSrc.ReadInt16();

			pvSrc.ReadByte(); // 0x0D
			int shirtHue = pvSrc.ReadInt16();
			/*int shirtVal = */
			pvSrc.ReadInt16();

			pvSrc.ReadByte(); // 0x0F
			/*int faceHue = */
			pvSrc.ReadInt16();
			/*int faceVal = */
			pvSrc.ReadInt16();

			pvSrc.ReadByte(); // 0x10
			int hairValf = pvSrc.ReadInt16();
			int hairHuef = pvSrc.ReadInt16();

			// male=0, female=1
			bool female = ( genderId == 1 );

			// human=0, elf=1, gargoyle=2
			raceId = raceId - 1; // convert to zero-based
			if ( raceId < 0 )
				raceId = 0;
			if ( raceId >= Race.Races.Length )
				raceId = Race.Races.Length;

			Race race = Race.Races[raceId];
			if ( race == null )
				race = Race.DefaultRace;

			CityInfo[] info = state.CityInfo;
			IAccount a = state.Account;

			if ( info == null || a == null || cityIndex < 0 || cityIndex >= info.Length )
			{
				state.Dispose();
			}
			else
			{
				// Check if anyone is using this account
				for ( int i = 0; i < a.Length; ++i )
				{
					Mobile check = a[i];

					if ( check != null && check.Map != Map.Internal )
					{
						Console.WriteLine( "Login: {0}: Account in use", state );
						state.Send( new PopupMessage( PMMessage.CharInWorld ) );
						return;
					}
				}

				CreateCharRequestEventArgs args = new CreateCharRequestEventArgs(
					state, a,
					name, female, hue,
					str, dex, intl,
					info[cityIndex],
					new SkillNameValue[4]
					{
						new SkillNameValue( (SkillName)is1, vs1 ),
						new SkillNameValue( (SkillName)is2, vs2 ),
						new SkillNameValue( (SkillName)is3, vs3 ),
						new SkillNameValue( (SkillName)is4, vs4 ),
					},
					shirtHue, pantsHue,
					hairVal, hairHue,
					hairValf, hairHuef,
					prof,
					race
					);

				state.BlockAllPackets = true;

				try
				{
					EventSink.InvokeCreateCharRequest( args );
				}
				catch ( Exception ex )
				{
					Logger.Error( "Exception disarmed in CreateCharRequest {0}: {1}", name, ex );
				}

				Mobile m = args.Mobile;

				if ( m != null )
				{
					state.Mobile = m;
					m.NetState = state;

					state.BlockAllPackets = false;

					try
					{
						EventSink.InvokeCharacterCreated( new CharacterCreatedEventArgs( m ) );
					}
					catch ( Exception ex )
					{
						Logger.Error( "Exception disarmed in CharacterCreated {0}: {1}", m, ex );
					}

					DoLogin( state, m );
				}
				else
				{
					state.BlockAllPackets = false;
					state.Dispose();
				}
			}
		}

		private bool m_ClientVerification = !Core.Config.Login.IgnoreAuthID;

		public bool ClientVerification
		{
			get { return m_ClientVerification; }
			set { m_ClientVerification = value; }
		}

		internal struct AuthIDPersistence
		{
			public DateTime Age;
			public ClientVersion Version;

			public AuthIDPersistence( ClientVersion v )
			{
				Age = DateTime.UtcNow;
				Version = v;
			}
		}

		private const int m_AuthIDWindowSize = 128;
		private Dictionary<int, AuthIDPersistence> m_AuthIDWindow = new Dictionary<int, AuthIDPersistence>( m_AuthIDWindowSize );

		private int GenerateAuthID( NetState state )
		{
			if ( m_AuthIDWindow.Count == m_AuthIDWindowSize )
			{
				int oldestID = 0;
				DateTime oldest = DateTime.MaxValue;

				foreach ( KeyValuePair<int, AuthIDPersistence> kvp in m_AuthIDWindow )
				{
					if ( kvp.Value.Age < oldest )
					{
						oldestID = kvp.Key;
						oldest = kvp.Value.Age;
					}
				}

				m_AuthIDWindow.Remove( oldestID );
			}

			int authID;

			do
			{
				authID = Utility.Random( 1, int.MaxValue - 1 );

				if ( Utility.RandomBool() )
					authID |= 1 << 31;
			}
			while ( m_AuthIDWindow.ContainsKey( authID ) );

			m_AuthIDWindow[authID] = new AuthIDPersistence( state.Version );

			return authID;
		}

		public void GameLogin( NetState state, PacketReader pvSrc )
		{
			if ( state.SentFirstPacket )
			{
				state.Dispose();
				return;
			}

			state.SentFirstPacket = true;

			int authID = pvSrc.ReadInt32();

			if ( m_AuthIDWindow.ContainsKey( authID ) )
			{
				AuthIDPersistence ap = m_AuthIDWindow[authID];
				m_AuthIDWindow.Remove( authID );

				state.Version = ap.Version;
			}
			else if ( m_ClientVerification )
			{
				Console.WriteLine( "GameLogin: {0}: Invalid auth ID, disconnecting", state );
				state.Dispose();
				return;
			}

			if ( state.m_AuthID != 0 && authID != state.m_AuthID )
			{
				Console.WriteLine( "GameLogin: {0}: Invalid auth ID, disconnecting", state );
				state.Dispose();
				return;
			}
			else if ( state.m_AuthID == 0 && state.m_Seed != -1 && authID != state.m_Seed )
			{
				Console.WriteLine( "GameLogin: {0}: Invalid auth ID, disconnecting", state );
				state.Dispose();
				return;
			}

			string username = pvSrc.ReadString( 30 );
			string password = pvSrc.ReadString( 30 );

			GameLoginEventArgs e = new GameLoginEventArgs( state, username, password );

			try
			{
				EventSink.InvokeGameLogin( e );
			}
			catch ( Exception ex )
			{
				Logger.Error( "Exception disarmed in GameLogin {0}: {1}", username, ex );
			}

			if ( e.Accepted )
			{
				if ( state.Account == null )
				{
					state.Dispose();
					return;
				}

				state.CityInfo = e.CityInfo;
				state.CompressionEnabled = true;

				state.Send( SupportedFeatures.Instantiate( state ) );

				if ( state.Version != null && ( state.Version >= CV.Client70130 || state.Version.IsEnhanced ) )
					state.Send( new CharacterListHS( state.Account, state.CityInfo ) );
				else
					state.Send( new CharacterList( state.Account, state.CityInfo ) );
			}
			else
			{
				state.Dispose();
			}
		}

		public void PlayServer( NetState state, PacketReader pvSrc )
		{
			int index = pvSrc.ReadInt16();

			ServerInfo[] info = state.ServerInfo;
			IAccount a = state.Account;

			if ( info == null || a == null || index < 0 || index >= info.Length )
			{
				state.Dispose();
			}
			else
			{
				ServerInfo si = info[index];

				state.m_AuthID = PlayServerAck.m_AuthID = GenerateAuthID( state );

				state.SentFirstPacket = false;
				state.Send( new PlayServerAck( si ) );

				// Close the connection, since they will be redirected to the game server.
				state.Dispose();
			}
		}

		public void LoginServerSeed( NetState state, PacketReader pvSrc )
		{
			state.m_Seed = pvSrc.ReadInt32();
			state.Seeded = true;

			if ( state.m_Seed == 0 )
			{
				Console.WriteLine( "Login: {0}: Invalid client detected, disconnecting", state );
				state.Dispose();
				return;
			}

			int clientMaj = pvSrc.ReadInt32();
			int clientMin = pvSrc.ReadInt32();
			int clientRev = pvSrc.ReadInt32();
			int clientPat = pvSrc.ReadInt32();

			state.Version = new ClientVersion( clientMaj, clientMin, clientRev, clientPat );
		}

		public void AccountLogin( NetState state, PacketReader pvSrc )
		{
			if ( state.SentFirstPacket )
			{
				state.Dispose();
				return;
			}

			state.SentFirstPacket = true;

			string username = pvSrc.ReadString( 30 );
			string password = pvSrc.ReadString( 30 );

			AccountLoginEventArgs e = new AccountLoginEventArgs( state, username, password );

			try
			{
				EventSink.InvokeAccountLogin( e );
			}
			catch ( Exception ex )
			{
				Logger.Error( "Exception disarmed in AccountLogin {0}: {1}", username, ex );
			}

			if ( e.Accepted )
				AccountLogin_ReplyAck( state );
			else
				AccountLogin_ReplyRej( state, e.RejectReason );
		}

		public void AccountLogin_ReplyAck( NetState state )
		{
			ServerListEventArgs e = new ServerListEventArgs( state, state.Account );

			try
			{
				EventSink.InvokeServerList( e );
			}
			catch ( Exception ex )
			{
				Logger.Error( "Exception disarmed in ServerList: {0}", ex );
				e.Rejected = true;
			}

			if ( e.Rejected )
			{
				state.Account = null;
				state.Send( new AccountLoginRej( ALRReason.BadComm ) );
				state.Dispose();
			}
			else
			{
				ServerInfo[] info = e.Servers.ToArray();

				state.ServerInfo = info;

				state.Send( new AccountLoginAck( info ) );
			}
		}

		public void AccountLogin_ReplyRej( NetState state, ALRReason reason )
		{
			state.Send( new AccountLoginRej( reason ) );
			state.Dispose();
		}

		public void ClientType( NetState state, PacketReader pvSrc )
		{
			/*int always1 = */
			pvSrc.ReadInt16();

			int clientType = pvSrc.ReadInt32();

			switch ( clientType )
			{
				default:
					state.Version.Type = CT.Classic;
					break;
				case 2:
				case 3:
					state.Version.Type = CT.Enhanced;
					break;
			}
		}
	}
}
