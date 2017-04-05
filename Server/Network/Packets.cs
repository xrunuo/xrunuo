using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using Server;
using Server.Accounting;
using Server.ContextMenus;
using Server.Gumps;
using Server.HuePickers;
using Server.Items;
using Server.Menus;
using Server.Menus.ItemLists;
using Server.Menus.Questions;
using Server.Mobiles;
using Server.Prompts;
using Server.Targeting;

namespace Server.Network
{
	public enum PMMessage : byte
	{
		CharNoExist = 1,
		CharExists = 2,
		CharInWorld = 5,
		LoginSyncError = 6,
		IdleWarning = 7,
		InvalidName = 10
	}

	public enum LRReason : byte
	{
		CannotLift = 0,
		OutOfRange = 1,
		OutOfSight = 2,
		TryToSteal = 3,
		AreHolding = 4,
		Inspecific = 5
	}

	public sealed class DamagePacket : Packet
	{
		public DamagePacket( Mobile m, int amount )
			: base( 0x0B, 7 )
		{
			m_Stream.Write( (int) m.Serial );

			if ( amount > 0xFFFF )
				amount = 0xFFFF;
			else if ( amount < 0 )
				amount = 0;

			m_Stream.Write( (ushort) amount );
		}
	}

	public sealed class CancelArrow : Packet
	{
		public CancelArrow( int x, int y, Serial s )
			: base( 0xBA, 10 )
		{
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (short) x );
			m_Stream.Write( (short) y );
			m_Stream.Write( (int) s );
		}
	}

	public sealed class SetArrow : Packet
	{
		public SetArrow( int x, int y, Serial s )
			: base( 0xBA, 10 )
		{
			m_Stream.Write( (byte) 1 );
			m_Stream.Write( (short) x );
			m_Stream.Write( (short) y );
			m_Stream.Write( (int) s );
		}
	}

	public sealed class DisplaySecureTrade : Packet
	{
		public DisplaySecureTrade( Mobile them, Container first, Container second, string name )
			: base( 0x6F )
		{
			if ( name == null )
				name = "";

			EnsureCapacity( 18 + name.Length );

			m_Stream.Write( (byte) 0 ); // Display
			m_Stream.Write( (int) them.Serial );
			m_Stream.Write( (int) first.Serial );
			m_Stream.Write( (int) second.Serial );
			m_Stream.Write( (bool) true );

			m_Stream.WriteAsciiFixed( name, 30 );
		}
	}

	public sealed class CloseSecureTrade : Packet
	{
		public CloseSecureTrade( Container cont )
			: base( 0x6F )
		{
			EnsureCapacity( 8 );

			m_Stream.Write( (byte) 1 ); // Close
			m_Stream.Write( (int) cont.Serial );
		}
	}

	public sealed class UpdateSecureTrade : Packet
	{
		public UpdateSecureTrade( Container cont, bool first, bool second )
			: base( 0x6F )
		{
			EnsureCapacity( 17 );

			m_Stream.Write( (byte) 2 ); // Update
			m_Stream.Write( (int) cont.Serial );
			m_Stream.Write( (int) ( first ? 1 : 0 ) );
			m_Stream.Write( (int) ( second ? 1 : 0 ) );
			m_Stream.Write( (byte) 0 ); // unknown
		}
	}

	public sealed class SecureTradeEquip : Packet
	{
		public SecureTradeEquip( Item item, Mobile m )
			: base( 0x25, 21 )
		{
			m_Stream.Write( (int) item.Serial );
			m_Stream.Write( (short) item.ItemID );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (short) item.Amount );
			m_Stream.Write( (short) item.X );
			m_Stream.Write( (short) item.Y );
			m_Stream.Write( (byte) 0 ); // KR Inventory Slot
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) item.Hue );
		}
	}

	public sealed class ObjectHelpResponse : Packet
	{
		public ObjectHelpResponse( IEntity e, string text )
			: base( 0xB7 )
		{
			EnsureCapacity( 9 + ( text.Length * 2 ) );

			m_Stream.Write( (int) e.Serial );
			m_Stream.WriteBigUniNull( text );
		}
	}

	public sealed class VendorBuyContent : Packet
	{
		public VendorBuyContent( ArrayList list )
			: base( 0x3C )
		{
			EnsureCapacity( list.Count * 20 + 5 );

			m_Stream.Write( (short) list.Count );

			// The client sorts these by their X/Y value.
			// OSI sends these in wierd order. X/Y highest to lowest and serial loest to highest
			// These are already sorted by serial (done by the vendor class) but we have to send them by x/y
			// (the x74 packet is sent in 'correct' order.)
			for ( var i = list.Count - 1; i >= 0; --i )
			{
				var bis = (BuyItemState) list[i];

				m_Stream.Write( (int) bis.MySerial );
				m_Stream.Write( (ushort) ( bis.ItemID & TileData.MaxItemValue ) );
				m_Stream.Write( (byte) 0 );//itemid offset
				m_Stream.Write( (ushort) bis.Amount );
				m_Stream.Write( (short) ( i + 1 ) );//x
				m_Stream.Write( (short) 1 );//y
				m_Stream.Write( (byte) 0 );
				m_Stream.Write( (int) bis.ContainerSerial );
				m_Stream.Write( (ushort) bis.Hue );
			}
		}
	}

	public sealed class DisplayBuyList : Packet
	{
		public DisplayBuyList( Mobile vendor )
			: base( 0x24, 9 )
		{
			m_Stream.Write( (int) vendor.Serial );
			m_Stream.Write( (short) 0x30 ); // buy window id?
			m_Stream.Write( (short) 0 );
		}
	}

	public sealed class VendorBuyList : Packet
	{
		public VendorBuyList( Mobile vendor, ArrayList list )
			: base( 0x74 )
		{
			EnsureCapacity( 256 );

			var BuyPack = vendor.FindItemOnLayer( Layer.ShopBuy ) as Container;
			m_Stream.Write( (int) ( BuyPack == null ? Serial.MinusOne : BuyPack.Serial ) );

			m_Stream.Write( (byte) list.Count );

			for ( var i = 0; i < list.Count; ++i )
			{
				var bis = (BuyItemState) list[i];

				m_Stream.Write( (int) bis.Price );

				var desc = bis.Description;

				if ( desc == null )
					desc = "";

				m_Stream.Write( (byte) ( desc.Length + 1 ) );
				m_Stream.WriteAsciiNull( desc );
			}
		}
	}

	public sealed class VendorSellList : Packet
	{
		public VendorSellList( Mobile shopkeeper, Hashtable table )
			: base( 0x9E )
		{
			EnsureCapacity( 256 );

			m_Stream.Write( (int) shopkeeper.Serial );

			m_Stream.Write( (ushort) table.Count );

			foreach ( SellItemState state in table.Values )
			{
				m_Stream.Write( (int) state.Item.Serial );
				m_Stream.Write( (ushort) ( state.Item.ItemID & TileData.MaxItemValue ) );
				m_Stream.Write( (ushort) state.Item.Hue );
				m_Stream.Write( (ushort) state.Item.Amount );
				m_Stream.Write( (ushort) state.Price );

				var name = state.Item.Name;

				if ( name == null || ( name = name.Trim() ).Length <= 0 )
					name = state.Name;

				if ( name == null )
					name = "";

				m_Stream.Write( (ushort) ( name.Length ) );
				m_Stream.WriteAsciiFixed( name, (ushort) ( name.Length ) );
			}
		}
	}

	public sealed class EndVendorSell : Packet
	{
		public EndVendorSell( Mobile Vendor )
			: base( 0x3B, 8 )
		{
			m_Stream.Write( (ushort) 8 );//length
			m_Stream.Write( (int) Vendor.Serial );
			m_Stream.Write( (byte) 0 );
		}
	}

	public sealed class EndVendorBuy : Packet
	{
		public EndVendorBuy( Mobile Vendor )
			: base( 0x3B, 8 )
		{
			m_Stream.Write( (ushort) 8 );//length
			m_Stream.Write( (int) Vendor.Serial );
			m_Stream.Write( (byte) 0 );
		}
	}

	public sealed class DeathAnimation : Packet
	{
		public DeathAnimation( Mobile killed, Item corpse )
			: base( 0xAF, 13 )
		{
			m_Stream.Write( (int) killed.Serial );
			m_Stream.Write( (int) ( corpse == null ? Serial.Zero : corpse.Serial ) );
			m_Stream.Write( (int) 0 );
		}
	}

	public sealed class StatLockInfo : Packet
	{
		public StatLockInfo( Mobile m )
			: base( 0xBF )
		{
			EnsureCapacity( 17 );

			m_Stream.Write( (short) 0x19 );
			m_Stream.Write( (byte) 0x5 );
			m_Stream.Write( (int) m.Serial );

			var lockBits = 0;

			lockBits |= (int) m.StrLock << 4;
			lockBits |= (int) m.DexLock << 2;
			lockBits |= (int) m.IntLock;

			m_Stream.Write( (short) lockBits );

			m_Stream.Write( (byte) 0 ); // status
			m_Stream.Write( (short) 0 ); // animation
			m_Stream.Write( (short) 0 ); // frame
		}
	}

	public class EquipInfoAttribute
	{
		public int Number { get; }

		public int Charges { get; }

		public EquipInfoAttribute( int number )
			: this( number, -1 )
		{
		}

		public EquipInfoAttribute( int number, int charges )
		{
			Number = number;
			Charges = charges;
		}
	}

	public class EquipmentInfo
	{
		public int Number { get; }

		public Mobile Crafter { get; }

		public bool Unidentified { get; }

		public EquipInfoAttribute[] Attributes { get; }

		public EquipmentInfo( int number, Mobile crafter, bool unidentified, EquipInfoAttribute[] attributes )
		{
			Number = number;
			Crafter = crafter;
			Unidentified = unidentified;
			Attributes = attributes;
		}
	}

	public sealed class DisplayEquipmentInfo : Packet
	{
		public DisplayEquipmentInfo( Item item, EquipmentInfo info )
			: base( 0xBF )
		{
			var attrs = info.Attributes;

			EnsureCapacity( 17 + ( info.Crafter == null ? 0 : 6 + info.Crafter.Name == null ? 0 : info.Crafter.Name.Length ) + ( info.Unidentified ? 4 : 0 ) + ( attrs.Length * 6 ) );

			m_Stream.Write( (short) 0x10 );
			m_Stream.Write( (int) item.Serial );

			m_Stream.Write( (int) info.Number );

			if ( info.Crafter != null )
			{
				var name = info.Crafter.Name;

				if ( name == null )
					name = "";

				int length = (ushort) name.Length;

				m_Stream.Write( (int) -3 );
				m_Stream.Write( (ushort) length );
				m_Stream.WriteAsciiFixed( name, length );
			}

			if ( info.Unidentified )
			{
				m_Stream.Write( (int) -4 );
			}

			for ( var i = 0; i < attrs.Length; ++i )
			{
				m_Stream.Write( (int) attrs[i].Number );
				m_Stream.Write( (short) attrs[i].Charges );
			}

			m_Stream.Write( (int) -1 );
		}
	}

	public sealed class ChangeUpdateRange : Packet
	{
		private static readonly ChangeUpdateRange[] m_Cache = new ChangeUpdateRange[0x100];

		public static ChangeUpdateRange Instantiate( int range )
		{
			var idx = (byte) range;
			var p = m_Cache[idx];

			if ( p == null )
			{
				m_Cache[idx] = p = new ChangeUpdateRange( range );
				p.SetStatic();
			}

			return p;
		}

		public ChangeUpdateRange( int range )
			: base( 0xC8, 2 )
		{
			m_Stream.Write( (byte) range );
		}
	}

	public sealed class ChangeCombatant : Packet
	{
		public ChangeCombatant( Mobile combatant )
			: base( 0xAA, 5 )
		{
			m_Stream.Write( combatant != null ? combatant.Serial : Serial.Zero );
		}
	}

	public sealed class DisplayHuePicker : Packet
	{
		public DisplayHuePicker( HuePicker huePicker )
			: base( 0x95, 9 )
		{
			m_Stream.Write( (int) huePicker.Serial );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) huePicker.ItemID );
		}
	}

	public sealed class TripTimeResponse : Packet
	{
		public TripTimeResponse( int unk )
			: base( 0xC9, 6 )
		{
			m_Stream.Write( (byte) unk );
			m_Stream.Write( (int) Environment.TickCount );
		}
	}

	public sealed class UTripTimeResponse : Packet
	{
		public UTripTimeResponse( int unk )
			: base( 0xCA, 6 )
		{
			m_Stream.Write( (byte) unk );
			m_Stream.Write( (int) Environment.TickCount );
		}
	}

	public sealed class UnicodePrompt : Packet
	{
		public UnicodePrompt( Prompt prompt, Mobile to )
			: base( 0xC2 )
		{
			EnsureCapacity( 21 );

			var senderSerial = prompt.Sender != null ? prompt.Sender.Serial : to.Serial;

			m_Stream.Write( (int) senderSerial );
			m_Stream.Write( (int) prompt.TypeId ); //0x2C
			m_Stream.Write( (int) 0 ); // type
			m_Stream.Write( (int) 0 ); // language
			m_Stream.Write( (short) 0 ); // text
		}
	}

	public sealed class ChangeCharacter : Packet
	{
		public ChangeCharacter( IAccount a )
			: base( 0x81 )
		{
			EnsureCapacity( 305 );

			var count = 0;

			for ( var i = 0; i < a.Length; ++i )
			{
				if ( a[i] != null )
					++count;
			}

			m_Stream.Write( (byte) count );
			m_Stream.Write( (byte) 0 );

			for ( var i = 0; i < a.Length; ++i )
			{
				if ( a[i] != null )
				{
					var name = a[i].Name;

					if ( name == null )
						name = "-null-";
					else if ( ( name = name.Trim() ).Length == 0 )
						name = "-empty-";

					m_Stream.WriteAsciiFixed( name, 30 );
					m_Stream.Fill( 30 ); // password
				}
				else
				{
					m_Stream.Fill( 60 );
				}
			}
		}
	}

	public sealed class DeathStatus : Packet
	{
		public static readonly Packet Dead = SetStatic( new DeathStatus( true ) );
		public static readonly Packet Alive = SetStatic( new DeathStatus( false ) );

		public static Packet Instantiate( bool dead )
		{
			return ( dead ? Dead : Alive );
		}

		public DeathStatus( bool dead )
			: base( 0x2C, 2 )
		{
			m_Stream.Write( (byte) ( dead ? 0 : 2 ) );
		}
	}

	public sealed class SpeedControl : Packet
	{
		public static readonly Packet WalkSpeed = SetStatic( new SpeedControl( 2 ) );
		public static readonly Packet MountSpeed = SetStatic( new SpeedControl( 1 ) );
		public static readonly Packet Disable = SetStatic( new SpeedControl( 0 ) );

		public SpeedControl( int speedControl )
			: base( 0xBF )
		{
			EnsureCapacity( 3 );

			m_Stream.Write( (short) 0x26 );
			m_Stream.Write( (byte) speedControl );
		}
	}

	public sealed class InvalidMapEnable : Packet
	{
		public InvalidMapEnable()
			: base( 0xC6, 1 )
		{
		}
	}

	public sealed class BondedStatus : Packet
	{
		public BondedStatus( int val1, Serial serial, int val2 )
			: base( 0xBF )
		{
			EnsureCapacity( 11 );

			m_Stream.Write( (short) 0x19 );
			m_Stream.Write( (byte) val1 );
			m_Stream.Write( (int) serial );
			m_Stream.Write( (byte) val2 );
		}
	}

	public sealed class ToggleSpecialAbility : Packet
	{
		// RunUO 2.0 Constructor
		public ToggleSpecialAbility( int abilityID, bool active )
			: base( 0xBF )
		{
			EnsureCapacity( 7 );

			m_Stream.Write( (short) 0x25 );

			m_Stream.Write( (short) abilityID );
			m_Stream.Write( (bool) active );
		}

		// RunUO RE Constructor
		public ToggleSpecialAbility( int spellid, int turn )
			: base( 0xBF )
		{
			EnsureCapacity( 7 );

			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0x25 );
			m_Stream.Write( (byte) 1 );
			m_Stream.Write( (byte) spellid );
			m_Stream.Write( (byte) turn );
		}
	}

	public sealed class DisplayItemListMenu : Packet
	{
		public DisplayItemListMenu( ItemListMenu menu )
			: base( 0x7C )
		{
			EnsureCapacity( 256 );

			m_Stream.Write( (int) ( (IMenu) menu ).Serial );
			m_Stream.Write( (short) 0 );

			var question = menu.Question;

			if ( question == null )
				question = "";

			int questionLength = (byte) question.Length;

			m_Stream.Write( (byte) questionLength );
			m_Stream.WriteAsciiFixed( question, questionLength );

			var entries = menu.Entries;

			int entriesLength = (byte) entries.Length;

			m_Stream.Write( (byte) entriesLength );

			for ( var i = 0; i < entriesLength; ++i )
			{
				var e = entries[i];

				m_Stream.Write( (ushort) ( e.ItemID & TileData.MaxItemValue ) );
				m_Stream.Write( (short) e.Hue );

				var name = e.Name;

				if ( name == null )
					name = "";

				int nameLength = (byte) name.Length;

				m_Stream.Write( (byte) nameLength );
				m_Stream.WriteAsciiFixed( name, nameLength );
			}
		}
	}

	public sealed class DisplayQuestionMenu : Packet
	{
		public DisplayQuestionMenu( QuestionMenu menu )
			: base( 0x7C )
		{
			EnsureCapacity( 256 );

			m_Stream.Write( (int) ( (IMenu) menu ).Serial );
			m_Stream.Write( (short) 0 );

			var question = menu.Question;

			if ( question == null )
				question = "";

			int questionLength = (byte) question.Length;

			m_Stream.Write( (byte) questionLength );
			m_Stream.WriteAsciiFixed( question, questionLength );

			var answers = menu.Answers;

			int answersLength = (byte) answers.Length;

			m_Stream.Write( (byte) answersLength );

			for ( var i = 0; i < answersLength; ++i )
			{
				m_Stream.Write( (int) 0 );

				var answer = answers[i];

				if ( answer == null )
					answer = "";

				int answerLength = (byte) answer.Length;

				m_Stream.Write( (byte) answerLength );
				m_Stream.WriteAsciiFixed( answer, answerLength );
			}
		}
	}

	public sealed class GlobalLightLevel : Packet
	{
		private static readonly GlobalLightLevel[] m_Cache = new GlobalLightLevel[0x100];

		public static GlobalLightLevel Instantiate( int level )
		{
			var lvl = (byte) level;
			var p = m_Cache[lvl];

			if ( p == null )
			{
				m_Cache[lvl] = p = new GlobalLightLevel( level );
				p.SetStatic();
			}

			return p;
		}

		public GlobalLightLevel( int level )
			: base( 0x4F, 2 )
		{
			m_Stream.Write( (sbyte) level );
		}
	}

	public sealed class PersonalLightLevel : Packet
	{
		public PersonalLightLevel( Mobile m )
			: this( m, m.LightLevel )
		{
		}

		public PersonalLightLevel( Mobile m, int level )
			: base( 0x4E, 6 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (sbyte) level );
		}
	}

	public sealed class PersonalLightLevelZero : Packet
	{
		public PersonalLightLevelZero( Mobile m )
			: base( 0x4E, 6 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (sbyte) 0 );
		}
	}

	public enum CMEFlags
	{
		None = 0x00,
		Disabled = 0x01,
		Colored = 0x20
	}

	public sealed class DisplayContextMenu : Packet
	{
		public DisplayContextMenu( ContextMenu menu )
			: base( 0xBF )
		{
			var entries = menu.Entries;

			int length = (byte) entries.Length;

			EnsureCapacity( 12 + ( length * 8 ) );

			m_Stream.Write( (short) 0x14 );
			m_Stream.Write( (short) 0x02 );

			var target = menu.Target as IEntity;

			m_Stream.Write( (int) ( target == null ? Serial.MinusOne : target.Serial ) );

			m_Stream.Write( (byte) length );

			IPoint3D p;

			if ( target is Mobile )
				p = target.Location;
			else if ( target is Item )
				p = ( (Item) target ).GetWorldLocation();
			else
				p = Point3D.Zero;

			for ( var i = 0; i < length; ++i )
			{
				var e = entries[i];

				if ( e.Number >= 500000 )
					m_Stream.Write( (uint) e.Number );
				else
					m_Stream.Write( (uint) ( 3000000 + e.Number ) ); // legacy

				m_Stream.Write( (short) i );

				var range = e.Range;

				if ( range == -1 )
					range = 18;

				var flags = ( e.Enabled && menu.From.InRange( p, range ) ) ? CMEFlags.None : CMEFlags.Disabled;

				var color = e.Color & 0xFFFF;

				if ( color != 0xFFFF )
					flags |= CMEFlags.Colored;

				flags |= e.Flags;

				m_Stream.Write( (short) flags );
			}
		}
	}

	public sealed class DisplayProfile : Packet
	{
		public DisplayProfile( bool realSerial, Mobile m, string header, string body, string footer )
			: base( 0xB8 )
		{
			if ( header == null )
				header = "";

			if ( body == null )
				body = "";

			if ( footer == null )
				footer = "";

			EnsureCapacity( 12 + header.Length + ( footer.Length * 2 ) + ( body.Length * 2 ) );

			m_Stream.Write( (int) ( realSerial ? m.Serial : Serial.Zero ) );
			m_Stream.WriteAsciiNull( header );
			m_Stream.WriteBigUniNull( footer );
			m_Stream.WriteBigUniNull( body );
		}
	}

	public sealed class CloseGump : Packet
	{
		public CloseGump( int typeID, int buttonID )
			: base( 0xBF )
		{
			EnsureCapacity( 13 );

			m_Stream.Write( (short) 0x04 );
			m_Stream.Write( (int) typeID );
			m_Stream.Write( (int) buttonID );
		}
	}

	public sealed class EquipUpdate : Packet
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public EquipUpdate( Item item )
			: base( 0x2E, 15 )
		{
			Serial parentSerial;

			if ( item.Parent is Mobile )
			{
				parentSerial = ( (Mobile) item.Parent ).Serial;
			}
			else
			{
				log.Warning( "EquipUpdate on item with !(parent is Mobile)" );
				parentSerial = Serial.Zero;
			}

			var hue = item.Hue;

			if ( item.Parent is Mobile )
			{
				var mob = (Mobile) item.Parent;

				if ( mob.SolidHueOverride >= 0 )
					hue = mob.SolidHueOverride;
			}

			m_Stream.Write( (int) item.Serial );
			m_Stream.Write( (short) item.ItemID );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) item.Layer );
			m_Stream.Write( (int) parentSerial );
			m_Stream.Write( (short) hue );
		}
	}

	public enum GraphicData : byte
	{
		TileData = 0x0,
		BodyData = 0x1,
		MultiData = 0x2
	}

	public sealed class WorldItem : Packet
	{
		public WorldItem( Item item )
			: base( 0xF3, 26 )
		{
			var itemId = item.ItemID;

			m_Stream.Write( (short) 1 );
			m_Stream.Write( (byte) item.GraphicData );

			m_Stream.Write( (uint) item.Serial.Value );
			m_Stream.Write( (short) itemId );

			m_Stream.Write( (byte) item.Direction );

			m_Stream.Write( (short) item.Amount );
			m_Stream.Write( (short) item.Amount );

			var loc = item.Location;

			m_Stream.Write( (short) loc.X );
			m_Stream.Write( (short) loc.Y );
			m_Stream.Write( (sbyte) loc.Z );

			m_Stream.Write( (byte) item.Light );

			m_Stream.Write( (ushort) item.Hue );
			m_Stream.Write( (byte) item.GetPacketFlags() );

			m_Stream.Write( (short) 0x0 ); // 0 for new item, 1 for update
		}
	}

	public sealed class LiftRej : Packet
	{
		private static readonly LiftRej[] m_Cache = new LiftRej[6];

		public static LiftRej Instantiate( LRReason reason )
		{
			var idx = (byte) reason;
			var p = m_Cache[idx];

			if ( p == null )
			{
				m_Cache[idx] = p = new LiftRej( (byte) reason );
				p.SetStatic();
			}

			return p;
		}

		public LiftRej( byte reason )
			: base( 0x27, 2 )
		{
			m_Stream.Write( reason );
		}
	}

	public sealed class LogoutAck : Packet
	{
		public static readonly Packet Instance = SetStatic( new LogoutAck() );

		public LogoutAck()
			: base( 0xD1, 2 )
		{
			m_Stream.Write( (byte) 0x01 );
		}
	}

	public sealed class Weather : Packet
	{
		public Weather( int v1, int v2, int v3 )
			: base( 0x65, 4 )
		{
			m_Stream.Write( (byte) v1 );
			m_Stream.Write( (byte) v2 );
			m_Stream.Write( (byte) v3 );
		}
	}

	public sealed class UnkD3 : Packet
	{
		public UnkD3( Mobile beholder, Mobile beheld )
			: base( 0xD3 )
		{
			EnsureCapacity( 256 );

			//int
			//short
			//short
			//short
			//byte
			//byte
			//short
			//byte
			//byte
			//short
			//short
			//short
			//while ( int != 0 )
			//{
			//short
			//byte
			//short
			//}

			m_Stream.Write( (int) beheld.Serial );
			m_Stream.Write( (short) ( (int) beheld.Body ) );
			m_Stream.Write( (short) beheld.X );
			m_Stream.Write( (short) beheld.Y );
			m_Stream.Write( (sbyte) beheld.Z );
			m_Stream.Write( (byte) beheld.Direction );
			m_Stream.Write( (ushort) beheld.Hue );
			m_Stream.Write( (byte) beheld.GetPacketFlags() );
			m_Stream.Write( (byte) Notoriety.Compute( beholder, beheld ) );

			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 0 );

			m_Stream.Write( (int) 0 );
		}
	}

	public sealed class GQRequest : Packet
	{
		public GQRequest()
			: base( 0xC3 )
		{
			EnsureCapacity( 256 );

			m_Stream.Write( (int) 1 );
			m_Stream.Write( (int) 2 ); // ID
			m_Stream.Write( (int) 3 ); // Customer ? (this)
			m_Stream.Write( (int) 4 ); // Customer this (?)
			m_Stream.Write( (int) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 6 );
			m_Stream.Write( (byte) 'r' );
			m_Stream.Write( (byte) 'e' );
			m_Stream.Write( (byte) 'g' );
			m_Stream.Write( (byte) 'i' );
			m_Stream.Write( (byte) 'o' );
			m_Stream.Write( (byte) 'n' );
			m_Stream.Write( (int) 7 ); // Call time in seconds
			m_Stream.Write( (short) 2 ); // Map (0=fel,1=tram,2=ilsh)
			m_Stream.Write( (int) 8 ); // X
			m_Stream.Write( (int) 9 ); // Y
			m_Stream.Write( (int) 10 ); // Z
			m_Stream.Write( (int) 11 ); // Volume
			m_Stream.Write( (int) 12 ); // Rank
			m_Stream.Write( (int) -1 );
			m_Stream.Write( (int) 1 ); // type
		}
	}

	/// <summary>
	/// Causes the client to walk in a given direction. It does not send a movement request.
	/// </summary>
	public sealed class PlayerMove : Packet
	{
		public PlayerMove( Direction d )
			: base( 0x97, 2 )
		{
			m_Stream.Write( (byte) d );

			// @4C63B0
		}
	}

	/// <summary>
	/// Displays a message "There are currently [count] available calls in the global queue.".
	/// </summary>
	public sealed class GQCount : Packet
	{
		public GQCount( int unk, int count )
			: base( 0xCB, 7 )
		{
			m_Stream.Write( (short) unk );
			m_Stream.Write( (int) count );
		}
	}

	/// <summary>
	/// Asks the client for it's version
	/// </summary>
	public sealed class ClientVersionReq : Packet
	{
		public static readonly Packet Instance = SetStatic( new ClientVersionReq() );

		public ClientVersionReq()
			: base( 0xBD )
		{
			EnsureCapacity( 3 );
		}
	}

	/// <summary>
	/// Asks the client for it's "assist version". (Perhaps for UOAssist?)
	/// </summary>
	public sealed class AssistVersionReq : Packet
	{
		public AssistVersionReq( int unk )
			: base( 0xBE )
		{
			EnsureCapacity( 7 );

			m_Stream.Write( (int) unk );
		}
	}

	public enum EffectType
	{
		Moving = 0x00,
		Lightning = 0x01,
		FixedXYZ = 0x02,
		FixedFrom = 0x03
	}

	public class ParticleEffect : Packet
	{
		public ParticleEffect( EffectType type, Serial from, Serial to, int itemID, Point3D fromPoint, Point3D toPoint, int speed, int duration, bool fixedDirection, bool explode, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, Serial serial, int layer, int unknown )
			: base( 0xC7, 49 )
		{
			m_Stream.Write( (byte) type );
			m_Stream.Write( (int) from );
			m_Stream.Write( (int) to );
			m_Stream.Write( (short) itemID );
			m_Stream.Write( (short) fromPoint.X );
			m_Stream.Write( (short) fromPoint.Y );
			m_Stream.Write( (sbyte) fromPoint.Z );
			m_Stream.Write( (short) toPoint.X );
			m_Stream.Write( (short) toPoint.Y );
			m_Stream.Write( (sbyte) toPoint.Z );
			m_Stream.Write( (byte) speed );
			m_Stream.Write( (byte) duration );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (bool) fixedDirection );
			m_Stream.Write( (bool) explode );
			m_Stream.Write( (int) hue );
			m_Stream.Write( (int) renderMode );
			m_Stream.Write( (short) effect );
			m_Stream.Write( (short) explodeEffect );
			m_Stream.Write( (short) explodeSound );
			m_Stream.Write( (int) serial );
			m_Stream.Write( (byte) layer );
			m_Stream.Write( (short) unknown );
		}

		public ParticleEffect( EffectType type, Serial from, Serial to, int itemID, IPoint3D fromPoint, IPoint3D toPoint, int speed, int duration, bool fixedDirection, bool explode, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, Serial serial, int layer, int unknown )
			: base( 0xC7, 49 )
		{
			m_Stream.Write( (byte) type );
			m_Stream.Write( (int) from );
			m_Stream.Write( (int) to );
			m_Stream.Write( (short) itemID );
			m_Stream.Write( (short) fromPoint.X );
			m_Stream.Write( (short) fromPoint.Y );
			m_Stream.Write( (sbyte) fromPoint.Z );
			m_Stream.Write( (short) toPoint.X );
			m_Stream.Write( (short) toPoint.Y );
			m_Stream.Write( (sbyte) toPoint.Z );
			m_Stream.Write( (byte) speed );
			m_Stream.Write( (byte) duration );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (bool) fixedDirection );
			m_Stream.Write( (bool) explode );
			m_Stream.Write( (int) hue );
			m_Stream.Write( (int) renderMode );
			m_Stream.Write( (short) effect );
			m_Stream.Write( (short) explodeEffect );
			m_Stream.Write( (short) explodeSound );
			m_Stream.Write( (int) serial );
			m_Stream.Write( (byte) layer );
			m_Stream.Write( (short) unknown );
		}
	}

	public class GraphicalEffect : Packet
	{
		public GraphicalEffect( EffectType type, Serial from, Serial to, int itemID, Point3D fromPoint, Point3D toPoint, int speed, int duration, bool fixedDirection, bool explode )
			: this( type, from, to, itemID, fromPoint, toPoint, speed, duration, fixedDirection, explode ? 1 : 0 )
		{
		}

		public GraphicalEffect( EffectType type, Serial from, Serial to, int itemID, Point3D fromPoint, Point3D toPoint, int speed, int duration, bool fixedDirection, int explode )
			: base( 0x70, 28 )
		{
			m_Stream.Write( (byte) type );
			m_Stream.Write( (int) from );
			m_Stream.Write( (int) to );
			m_Stream.Write( (short) itemID );
			m_Stream.Write( (short) fromPoint.X );
			m_Stream.Write( (short) fromPoint.Y );
			m_Stream.Write( (sbyte) fromPoint.Z );
			m_Stream.Write( (short) toPoint.X );
			m_Stream.Write( (short) toPoint.Y );
			m_Stream.Write( (sbyte) toPoint.Z );
			m_Stream.Write( (byte) speed );
			m_Stream.Write( (byte) duration );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (bool) fixedDirection );
			m_Stream.Write( (byte) explode );
		}
	}

	public class HuedEffect : Packet
	{
		public HuedEffect( EffectType type, Serial from, Serial to, int itemID, IPoint3D fromPoint, IPoint3D toPoint, int speed, int duration, bool fixedDirection, bool explode, int hue, int renderMode )
			: base( 0xC0, 36 )
		{
			m_Stream.Write( (byte) type );
			m_Stream.Write( (int) from );
			m_Stream.Write( (int) to );
			m_Stream.Write( (short) itemID );
			m_Stream.Write( (short) fromPoint.X );
			m_Stream.Write( (short) fromPoint.Y );
			m_Stream.Write( (sbyte) fromPoint.Z );
			m_Stream.Write( (short) toPoint.X );
			m_Stream.Write( (short) toPoint.Y );
			m_Stream.Write( (sbyte) toPoint.Z );
			m_Stream.Write( (byte) speed );
			m_Stream.Write( (byte) duration );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (bool) fixedDirection );
			m_Stream.Write( (bool) explode );
			m_Stream.Write( (int) hue );
			m_Stream.Write( (int) renderMode );
		}
	}

	public sealed class TargetParticleEffect : ParticleEffect
	{
		public TargetParticleEffect( IEntity e, int itemID, int speed, int duration, int hue, int renderMode, int effect, int layer, int unknown )
			: base( EffectType.FixedFrom, e.Serial, Serial.Zero, itemID, e.Location, e.Location, speed, duration, true, false, hue, renderMode, effect, 1, 0, e.Serial, layer, unknown )
		{
		}
	}

	public sealed class TargetEffect : HuedEffect
	{
		public TargetEffect( IEntity e, int itemID, int speed, int duration, int hue, int renderMode )
			: base( EffectType.FixedFrom, e.Serial, Serial.Zero, itemID, e.Location, e.Location, speed, duration, true, false, hue, renderMode )
		{
		}
	}

	public sealed class LocationParticleEffect : ParticleEffect
	{
		public LocationParticleEffect( IEntity e, int itemID, int speed, int duration, int hue, int renderMode, int effect, int unknown )
			: base( EffectType.FixedXYZ, e.Serial, Serial.Zero, itemID, e.Location, e.Location, speed, duration, true, false, hue, renderMode, effect, 1, 0, e.Serial, 255, unknown )
		{
		}
	}

	public sealed class LocationEffect : HuedEffect
	{
		public LocationEffect( IPoint3D p, int itemID, int speed, int duration, int hue, int renderMode )
			: base( EffectType.FixedXYZ, Serial.Zero, Serial.Zero, itemID, p, p, speed, duration, true, false, hue, renderMode )
		{
		}
	}

	public sealed class MovingParticleEffect : ParticleEffect
	{
		public MovingParticleEffect( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )
			: base( EffectType.Moving, from.Serial, to.Serial, itemID, from.Location, to.Location, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, Serial.Zero, (int) layer, unknown )
		{
		}
	}

	public sealed class MovingEffect : HuedEffect
	{
		public MovingEffect( IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode )
			: base( EffectType.Moving, from.Serial, to.Serial, itemID, from.Location, to.Location, speed, duration, fixedDirection, explodes, hue, renderMode )
		{
		}
	}

	public enum FlashType : byte
	{
		FadeOut = 0x0,
		FadeIn = 0x1,
		LightFlash = 0x2,
		FadeInOut = 0x3
	}

	public sealed class FlashEffect : Packet
	{
		public FlashEffect( FlashType flashType )
			: base( 0x70, 28 )
		{
			m_Stream.Write( (byte) 4 ); // EffectType
			m_Stream.Write( (int) 0 );
			m_Stream.Write( (int) 0 );
			m_Stream.Write( (ushort) flashType );
			m_Stream.Fill( 16 );
		}
	}

	public enum DeleteResultType
	{
		PasswordInvalid,
		CharNotExist,
		CharBeingPlayed,
		CharTooYoung,
		CharQueued,
		BadRequest
	}

	public sealed class DeleteResult : Packet
	{
		public DeleteResult( DeleteResultType res )
			: base( 0x85, 2 )
		{
			m_Stream.Write( (byte) res );
		}
	}

	public sealed class BoltEffect : Packet
	{
		public BoltEffect( IEntity target, int hue )
			: base( 0xC0, 36 )
		{
			m_Stream.Write( (byte) 0x01 ); // type
			m_Stream.Write( (int) target.Serial );
			m_Stream.Write( (int) Serial.Zero );
			m_Stream.Write( (short) 0 ); // itemID
			m_Stream.Write( (short) target.X );
			m_Stream.Write( (short) target.Y );
			m_Stream.Write( (sbyte) target.Z );
			m_Stream.Write( (short) target.X );
			m_Stream.Write( (short) target.Y );
			m_Stream.Write( (sbyte) target.Z );
			m_Stream.Write( (byte) 0 ); // speed
			m_Stream.Write( (byte) 0 ); // duration
			m_Stream.Write( (short) 0 ); // unk
			m_Stream.Write( false ); // fixed direction
			m_Stream.Write( false ); // explode
			m_Stream.Write( (int) hue );
			m_Stream.Write( (int) 0 ); // render mode
		}
	}

	public sealed class DisplaySpellbook : Packet
	{
		public DisplaySpellbook( Item book )
			: base( 0x24, 9 )
		{
			m_Stream.Write( (int) book.Serial );
			m_Stream.Write( (short) -1 );
			m_Stream.Write( (short) 125 );
		}
	}

	public sealed class SpellbookContent : Packet
	{
		public SpellbookContent( Item item, int graphic, int offset, ulong content )
			: base( 0xBF )
		{
			EnsureCapacity( 23 );

			m_Stream.Write( (short) 0x1B );
			m_Stream.Write( (short) 0x01 );

			m_Stream.Write( (int) item.Serial );
			m_Stream.Write( (short) graphic );
			m_Stream.Write( (short) offset );

			for ( var i = 0; i < 8; ++i )
				m_Stream.Write( (byte) ( content >> ( i * 8 ) ) );
		}
	}

	public sealed class ContainerDisplay : Packet
	{
		public ContainerDisplay( Container c )
			: base( 0x24, 9 )
		{
			m_Stream.Write( (int) c.Serial );
			m_Stream.Write( (short) c.GumpID );
			m_Stream.Write( (short) c.MaxItems );
		}
	}

	public sealed class ContainerContentUpdate : Packet
	{
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public ContainerContentUpdate( Item item )
			: base( 0x25, 21 )
		{
			Serial parentSerial;

			if ( item.Parent is Item )
			{
				parentSerial = ( (Item) item.Parent ).Serial;
			}
			else
			{
				log.Info( "ContainerContentUpdate on item with !(parent is Item)" );
				parentSerial = Serial.Zero;
			}

			m_Stream.Write( (int) item.Serial );
			m_Stream.Write( (short) item.ItemID );
			m_Stream.Write( (byte) 0 ); // signed, itemID offset
			m_Stream.Write( (ushort) item.Amount );
			m_Stream.Write( (short) item.X );
			m_Stream.Write( (short) item.Y );
			m_Stream.Write( (byte) item.GridLocation );
			m_Stream.Write( (int) parentSerial );
			m_Stream.Write( (ushort) item.Hue );
		}
	}

	public sealed class ContainerContent : Packet
	{
		public ContainerContent( Mobile beholder, Item beheld )
			: base( 0x3C )
		{
			var items = beheld.Items;
			var count = items.Count;

			EnsureCapacity( 5 + ( count * 20 ) );

			var pos = m_Stream.Position;

			var written = 0;

			m_Stream.Write( (ushort) 0 );

			for ( var i = 0; i < count; ++i )
			{
				var child = items[i];

				if ( !child.Deleted && beholder.CanSee( child ) )
				{
					++written;

					var loc = child.Location;

					if ( child.GridLocation == 0xFF )
						child.GridLocation = (byte) ( count - written );

					m_Stream.Write( (int) child.Serial );
					m_Stream.Write( (ushort) child.ItemID );
					m_Stream.Write( (byte) 0 ); // signed, itemID offset
					m_Stream.Write( (ushort) child.Amount );
					m_Stream.Write( (short) loc.X );
					m_Stream.Write( (short) loc.Y );
					m_Stream.Write( (byte) child.GridLocation );
					m_Stream.Write( (int) beheld.Serial );
					m_Stream.Write( (ushort) child.Hue );
				}
			}

			m_Stream.Seek( pos, SeekOrigin.Begin );
			m_Stream.Write( (ushort) written );
		}
	}

	public sealed class SetWarMode : Packet
	{
		public static readonly Packet InWarMode = SetStatic( new SetWarMode( true ) );
		public static readonly Packet InPeaceMode = SetStatic( new SetWarMode( false ) );

		public static Packet Instantiate( bool mode )
		{
			return ( mode ? InWarMode : InPeaceMode );
		}

		public SetWarMode( bool mode )
			: base( 0x72, 5 )
		{
			m_Stream.Write( mode );
			m_Stream.Write( (byte) 0x00 );
			m_Stream.Write( (byte) 0x32 );
			m_Stream.Write( (byte) 0x00 );
		}
	}

	public sealed class Swing : Packet
	{
		public Swing( int flag, Mobile attacker, Mobile defender )
			: base( 0x2F, 10 )
		{
			m_Stream.Write( (byte) flag );
			m_Stream.Write( (int) attacker.Serial );
			m_Stream.Write( (int) defender.Serial );
		}
	}

	public sealed class NullFastwalkStack : Packet
	{
		public NullFastwalkStack()
			: base( 0xBF )
		{
			EnsureCapacity( 256 );
			m_Stream.Write( (short) 0x1 );
			m_Stream.Write( (int) 0x0 );
			m_Stream.Write( (int) 0x0 );
			m_Stream.Write( (int) 0x0 );
			m_Stream.Write( (int) 0x0 );
			m_Stream.Write( (int) 0x0 );
			m_Stream.Write( (int) 0x0 );
		}
	}

	public sealed class RemoveItem : Packet
	{
		public RemoveItem( Item item )
			: base( 0x1D, 5 )
		{
			m_Stream.Write( (int) item.Serial );
		}
	}

	public sealed class RemoveMobile : Packet
	{
		public RemoveMobile( Mobile m )
			: base( 0x1D, 5 )
		{
			m_Stream.Write( (int) m.Serial );
		}
	}

	public sealed class ServerChange : Packet
	{
		public ServerChange( Mobile m, Map map )
			: base( 0x76, 16 )
		{
			m_Stream.Write( (short) m.X );
			m_Stream.Write( (short) m.Y );
			m_Stream.Write( (short) m.Z );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) map.Width );
			m_Stream.Write( (short) map.Height );
		}
	}

	public sealed class SkillUpdate : Packet
	{
		public SkillUpdate( Skills skills )
			: base( 0x3A )
		{
			EnsureCapacity( 6 + ( skills.Length * 9 ) );

			m_Stream.Write( (byte) 0x02 ); // type: absolute, capped

			for ( var i = 0; i < skills.Length; ++i )
			{
				var s = skills[i];

				var v = s.NonRacialValue;
				var uv = (int) ( v * 10 );

				if ( uv < 0 )
					uv = 0;
				else if ( uv >= 0x10000 )
					uv = 0xFFFF;

				m_Stream.Write( (ushort) ( s.Info.SkillId + 1 ) );
				m_Stream.Write( (ushort) uv );
				m_Stream.Write( (ushort) s.BaseFixedPoint );
				m_Stream.Write( (byte) s.Lock );
				m_Stream.Write( (ushort) s.CapFixedPoint );
			}

			m_Stream.Write( (short) 0 ); // terminate
		}
	}

	public sealed class Sequence : Packet
	{
		public Sequence( int num )
			: base( 0x7B, 2 )
		{
			m_Stream.Write( (byte) num );
		}
	}

	public sealed class SkillChange : Packet
	{
		public SkillChange( Skill skill )
			: base( 0x3A )
		{
			EnsureCapacity( 13 );

			var v = skill.NonRacialValue;
			var uv = (int) ( v * 10 );

			if ( uv < 0 )
				uv = 0;
			else if ( uv >= 0x10000 )
				uv = 0xFFFF;

			m_Stream.Write( (byte) 0xDF ); // type: delta, capped
			m_Stream.Write( (ushort) skill.Info.SkillId );
			m_Stream.Write( (ushort) uv );
			m_Stream.Write( (ushort) skill.BaseFixedPoint );
			m_Stream.Write( (byte) skill.Lock );
			m_Stream.Write( (ushort) skill.CapFixedPoint );
		}
	}

	public sealed class LaunchBrowser : Packet
	{
		public LaunchBrowser( string url )
			: base( 0xA5 )
		{
			if ( url == null )
				url = "";

			EnsureCapacity( 4 + url.Length );

			m_Stream.WriteAsciiNull( url );
		}
	}

	public sealed class MessageLocalized : Packet
	{
		private static readonly MessageLocalized[] m_Cache_IntLoc = new MessageLocalized[15000];
		private static readonly MessageLocalized[] m_Cache_CliLoc = new MessageLocalized[100000];
		private static readonly MessageLocalized[] m_Cache_CliLocCmp = new MessageLocalized[5000];

		public static MessageLocalized InstantiateGeneric( int number )
		{
			MessageLocalized[] cache = null;
			var index = 0;

			if ( number >= 3000000 )
			{
				cache = m_Cache_IntLoc;
				index = number - 3000000;
			}
			else if ( number >= 1000000 )
			{
				cache = m_Cache_CliLoc;
				index = number - 1000000;
			}
			else if ( number >= 500000 )
			{
				cache = m_Cache_CliLocCmp;
				index = number - 500000;
			}

			MessageLocalized p;

			if ( cache != null && index >= 0 && index < cache.Length )
			{
				p = cache[index];

				if ( p == null )
				{
					cache[index] = p = new MessageLocalized( Serial.MinusOne, -1, MessageType.Regular, 0x3B2, 3, number, "System", "" );
					p.SetStatic();
				}
			}
			else
			{
				p = new MessageLocalized( Serial.MinusOne, -1, MessageType.Regular, 0x3B2, 3, number, "System", "" );
			}

			return p;
		}

		public MessageLocalized( Serial serial, int graphic, MessageType type, int hue, int font, int number, string name, string args )
			: base( 0xC1 )
		{
			if ( name == null )
				name = "";
			if ( args == null )
				args = "";

			if ( hue == 0 )
				hue = 0x3B2;

			EnsureCapacity( 50 + ( args.Length * 2 ) );

			m_Stream.Write( (int) serial );
			m_Stream.Write( (short) graphic );
			m_Stream.Write( (byte) type );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (short) font );
			m_Stream.Write( (int) number );
			m_Stream.WriteAsciiFixed( name, 30 );
			m_Stream.WriteLittleUniNull( args );
		}
	}

	public sealed class MultiTargetReq : Packet
	{
		public MultiTargetReq( MultiTarget t )
			: base( 0x99, 30 )
		{
			m_Stream.Write( (bool) t.AllowGround );
			m_Stream.Write( (int) t.TargetID );
			m_Stream.Write( (byte) t.Flags );

			m_Stream.Fill();

			m_Stream.Seek( 18, SeekOrigin.Begin );
			m_Stream.Write( (short) t.MultiID );
			m_Stream.Write( (short) t.Offset.X );
			m_Stream.Write( (short) t.Offset.Y );
			m_Stream.Write( (short) t.Offset.Z );
		}
	}

	public sealed class CancelTarget : Packet
	{
		public static readonly Packet Instance = SetStatic( new CancelTarget() );

		public CancelTarget()
			: base( 0x6C, 19 )
		{
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (int) 0 );
			m_Stream.Write( (byte) 3 );
			m_Stream.Fill();
		}
	}

	public sealed class TargetReq : Packet
	{
		public TargetReq( Target t )
			: base( 0x6C, 19 )
		{
			m_Stream.Write( (bool) t.AllowGround );
			m_Stream.Write( (int) t.TargetID );
			m_Stream.Write( (byte) t.Flags );
			m_Stream.Fill();
		}
	}

	public sealed class DragEffect : Packet
	{
		public DragEffect( IEntity src, IEntity trg, int itemID, int hue, int amount )
			: base( 0x23, 26 )
		{
			m_Stream.Write( (short) itemID );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (short) amount );
			m_Stream.Write( (int) src.Serial );
			m_Stream.Write( (short) src.X );
			m_Stream.Write( (short) src.Y );
			m_Stream.Write( (sbyte) src.Z );
			m_Stream.Write( (int) trg.Serial );
			m_Stream.Write( (short) trg.X );
			m_Stream.Write( (short) trg.Y );
			m_Stream.Write( (sbyte) trg.Z );
		}
	}

	public interface IGumpWriter
	{
		int TextEntries { get; set; }
		int Switches { get; set; }

		void AppendLayout( bool val );
		void AppendLayout( int val );
		void AppendLayoutNS( int val );
		void AppendLayout( string text );
		void AppendLayout( byte[] buffer );
		void WriteStrings( List<string> strings );
		void Flush();
	}

	public sealed class DisplayGumpPacked : Packet, IGumpWriter
	{
		public int TextEntries { get; set; }
		public int Switches { get; set; }

		private readonly Gump m_Gump;

		private readonly PacketWriter m_Layout;
		private readonly PacketWriter m_Strings;

		private int m_StringCount;

		public DisplayGumpPacked( Gump gump )
			: base( 0xDD )
		{
			m_Gump = gump;

			m_Layout = PacketWriter.CreateInstance( 8192 );
			m_Strings = PacketWriter.CreateInstance( 8192 );
		}

		private static readonly byte[] m_True = Gump.StringToBuffer( " 1" );
		private static readonly byte[] m_False = Gump.StringToBuffer( " 0" );

		private static readonly byte[] m_BeginTextSeparator = Gump.StringToBuffer( " @" );
		private static readonly byte[] m_EndTextSeparator = Gump.StringToBuffer( "@" );

		private static readonly byte[] m_Buffer = new byte[48];

		static DisplayGumpPacked()
		{
			m_Buffer[0] = (byte) ' ';
		}

		public void AppendLayout( bool val )
		{
			AppendLayout( val ? m_True : m_False );
		}

		public void AppendLayout( int val )
		{
			var toString = val.ToString();
			var bytes = System.Text.Encoding.ASCII.GetBytes( toString, 0, toString.Length, m_Buffer, 1 ) + 1;

			m_Layout.Write( m_Buffer, 0, bytes );
		}

		public void AppendLayoutNS( int val )
		{
			var toString = val.ToString();
			var bytes = System.Text.Encoding.ASCII.GetBytes( toString, 0, toString.Length, m_Buffer, 1 );

			m_Layout.Write( m_Buffer, 1, bytes );
		}

		public void AppendLayout( string text )
		{
			AppendLayout( m_BeginTextSeparator );

			m_Layout.WriteAsciiFixed( text, text.Length );

			AppendLayout( m_EndTextSeparator );
		}

		public void AppendLayout( byte[] buffer )
		{
			m_Layout.Write( buffer, 0, buffer.Length );
		}

		public void WriteStrings( List<string> strings )
		{
			m_StringCount = strings.Count;

			for ( var i = 0; i < strings.Count; ++i )
			{
				var v = strings[i];

				if ( v == null )
					v = String.Empty;

				m_Strings.Write( (ushort) v.Length );
				m_Strings.WriteBigUniFixed( v, v.Length );
			}
		}

		public void Flush()
		{
			EnsureCapacity( 28 + (int) m_Layout.Length + (int) m_Strings.Length );

			m_Stream.Write( (int) m_Gump.Serial );
			m_Stream.Write( (int) m_Gump.TypeID );
			m_Stream.Write( (int) m_Gump.X );
			m_Stream.Write( (int) m_Gump.Y );

			m_Layout.Write( (byte) 0 );
			WritePacked( m_Layout );

			m_Stream.Write( (int) m_StringCount );

			WritePacked( m_Strings );

			PacketWriter.ReleaseInstance( m_Layout );
			PacketWriter.ReleaseInstance( m_Strings );
		}

		private static byte[] m_PackBuffer;

		private void WritePacked( PacketWriter src )
		{
			var buffer = src.UnderlyingStream.GetBuffer();
			var length = (int) src.Length;

			if ( length == 0 )
			{
				m_Stream.Write( (int) 0 );
				return;
			}

			var wantLength = 1 + ( ( buffer.Length * 1024 ) / 1000 );

			wantLength += 4095;
			wantLength &= ~4095;

			if ( m_PackBuffer == null || m_PackBuffer.Length < wantLength )
				m_PackBuffer = new byte[wantLength];

			var packLength = m_PackBuffer.Length;

			ZLib.compress2( m_PackBuffer, ref packLength, buffer, length, ZLibCompressionLevel.Z_DEFAULT_COMPRESSION );

			m_Stream.Write( (int) ( 4 + packLength ) );
			m_Stream.Write( (int) length );
			m_Stream.Write( m_PackBuffer, 0, packLength );
		}
	}

	public sealed class DisplayGumpFast : Packet, IGumpWriter
	{
		private int m_LayoutLength;

		public int TextEntries { get; set; }

		public int Switches { get; set; }

		public DisplayGumpFast( Gump g )
			: base( 0xB0 )
		{
			EnsureCapacity( 4096 );

			m_Stream.Write( (int) g.Serial );
			m_Stream.Write( (int) g.TypeID );
			m_Stream.Write( (int) g.X );
			m_Stream.Write( (int) g.Y );
			m_Stream.Write( (ushort) 0xFFFF );
		}

		private static readonly byte[] m_True = Gump.StringToBuffer( " 1" );
		private static readonly byte[] m_False = Gump.StringToBuffer( " 0" );

		private static readonly byte[] m_BeginTextSeparator = Gump.StringToBuffer( " @" );
		private static readonly byte[] m_EndTextSeparator = Gump.StringToBuffer( "@" );

		private static readonly byte[] m_Buffer = new byte[48];

		static DisplayGumpFast()
		{
			m_Buffer[0] = (byte) ' ';
		}

		public void AppendLayout( bool val )
		{
			AppendLayout( val ? m_True : m_False );
		}

		public void AppendLayout( int val )
		{
			var toString = val.ToString();
			var bytes = System.Text.Encoding.ASCII.GetBytes( toString, 0, toString.Length, m_Buffer, 1 ) + 1;

			m_Stream.Write( m_Buffer, 0, bytes );
			m_LayoutLength += bytes;
		}

		public void AppendLayoutNS( int val )
		{
			var toString = val.ToString();
			var bytes = System.Text.Encoding.ASCII.GetBytes( toString, 0, toString.Length, m_Buffer, 1 );

			m_Stream.Write( m_Buffer, 1, bytes );
			m_LayoutLength += bytes;
		}

		public void AppendLayout( string text )
		{
			AppendLayout( m_BeginTextSeparator );

			var length = text.Length;
			m_Stream.WriteAsciiFixed( text, length );
			m_LayoutLength += length;

			AppendLayout( m_EndTextSeparator );
		}

		public void AppendLayout( byte[] buffer )
		{
			var length = buffer.Length;
			m_Stream.Write( buffer, 0, length );
			m_LayoutLength += length;
		}

		public void WriteStrings( List<string> text )
		{
			m_Stream.Seek( 19, SeekOrigin.Begin );
			m_Stream.Write( (ushort) m_LayoutLength );
			m_Stream.Seek( 0, SeekOrigin.End );

			m_Stream.Write( (ushort) text.Count );

			for ( var i = 0; i < text.Count; ++i )
			{
				var v = text[i];

				if ( v == null )
					v = String.Empty;

				int length = (ushort) v.Length;

				m_Stream.Write( (ushort) length );
				m_Stream.WriteBigUniFixed( v, length );
			}
		}

		public void Flush()
		{
		}
	}

	public sealed class DisplayGump : Packet
	{
		public DisplayGump( Gump g, string layout, string[] text )
			: base( 0xB0 )
		{
			if ( layout == null )
				layout = "";

			EnsureCapacity( 256 );

			m_Stream.Write( (int) g.Serial );
			m_Stream.Write( (int) g.TypeID );
			m_Stream.Write( (int) g.X );
			m_Stream.Write( (int) g.Y );
			m_Stream.Write( (ushort) ( layout.Length + 1 ) );
			m_Stream.WriteAsciiNull( layout );

			m_Stream.Write( (ushort) text.Length );

			for ( var i = 0; i < text.Length; ++i )
			{
				var v = text[i];

				if ( v == null )
					v = "";

				int length = (ushort) v.Length;

				m_Stream.Write( (ushort) length );
				m_Stream.WriteBigUniFixed( v, length );
			}
		}
	}

	public sealed class DisplayPaperdoll : Packet
	{
		public DisplayPaperdoll( Mobile m, string text, bool canLift )
			: base( 0x88, 66 )
		{
			byte flags = 0x00;

			if ( m.Warmode )
				flags |= 0x01;

			if ( canLift )
				flags |= 0x02;

			m_Stream.Write( (int) m.Serial );
			m_Stream.WriteAsciiFixed( text, 60 );
			m_Stream.Write( (byte) flags );
		}
	}

	public sealed class PopupMessage : Packet
	{
		public PopupMessage( PMMessage msg )
			: base( 0x53, 2 )
		{
			m_Stream.Write( (byte) msg );
		}
	}

	public sealed class PlayMusic : Packet
	{
		public static readonly Packet InvalidInstance = SetStatic( new PlayMusic( MusicName.Invalid ) );

		private static readonly Packet[] m_Instances = new Packet[60];

		public static Packet GetInstance( MusicName name )
		{
			if ( name == MusicName.Invalid )
				return InvalidInstance;

			var v = (int) name;
			Packet p;

			if ( v >= 0 && v < m_Instances.Length )
			{
				p = m_Instances[v];

				if ( p == null )
					m_Instances[v] = p = SetStatic( new PlayMusic( name ) );
			}
			else
			{
				p = new PlayMusic( name );
			}

			return p;
		}

		public PlayMusic( MusicName name )
			: base( 0x6D, 3 )
		{
			m_Stream.Write( (short) name );
		}
	}

	public sealed class ScrollMessage : Packet
	{
		public ScrollMessage( int type, int tip, string text )
			: base( 0xA6 )
		{
			if ( text == null )
				text = "";

			EnsureCapacity( 10 + text.Length );
			m_Stream.Write( (byte) type );
			m_Stream.Write( (int) tip );
			m_Stream.Write( (ushort) text.Length );
			m_Stream.WriteAsciiFixed( text, text.Length );
		}
	}

	public sealed class CurrentTime : Packet
	{
		public CurrentTime()
			: base( 0x5B, 4 )
		{
			var now = DateTime.UtcNow;

			m_Stream.Write( (byte) now.Hour );
			m_Stream.Write( (byte) now.Minute );
			m_Stream.Write( (byte) now.Second );
		}
	}

	public sealed class MapChange : Packet
	{
		public MapChange( Mobile m )
			: base( 0xBF )
		{
			EnsureCapacity( 6 );

			m_Stream.Write( (short) 0x08 );
			m_Stream.Write( (byte) ( m.Map == null ? 0 : m.Map.MapID ) );
		}
	}

	public sealed class SeasonChange : Packet
	{
		private static readonly SeasonChange[][] m_Cache = new SeasonChange[5][]
			{
				new SeasonChange[2],
				new SeasonChange[2],
				new SeasonChange[2],
				new SeasonChange[2],
				new SeasonChange[2]
			};

		public static SeasonChange Instantiate( int season )
		{
			return Instantiate( season, true );
		}

		public static SeasonChange Instantiate( int season, bool playSound )
		{
			if ( season >= 0 && season < m_Cache.Length )
			{
				var idx = playSound ? 1 : 0;

				var p = m_Cache[season][idx];

				if ( p == null )
				{
					m_Cache[season][idx] = p = new SeasonChange( season, playSound );
					p.SetStatic();
				}

				return p;
			}
			else
			{
				return new SeasonChange( season, playSound );
			}
		}

		public SeasonChange( int season )
			: this( season, true )
		{
		}

		public SeasonChange( int season, bool playSound )
			: base( 0xBC, 3 )
		{
			m_Stream.Write( (byte) season );
			m_Stream.Write( (bool) playSound );
		}
	}

	public sealed class SupportedFeatures : Packet
	{
		public static int Value { get; set; }

		public static SupportedFeatures Instantiate( NetState state )
		{
			return new SupportedFeatures( state );
		}

		public SupportedFeatures( NetState state )
			: base( 0xB9, 5 )
		{
			m_Stream.Write( (ushort) 1 ); // Stygian Abyss

			var acct = state.Account;

			if ( acct == null ) // should never be
			{
				m_Stream.Write( (ushort) 0x00 );
				return;
			}

			var flags = 0x3 | Value;

			flags |= 0x0040; // Samurai Empire
			flags |= 0x801C; // Age of Shadows

			if ( acct.Limit >= 6 )
			{
				flags |= 0x8020;
				flags &= ~0x004;
			}

			flags |= 0x0080; // Mondain's Legacy

			if ( acct.FeatureEnabled( AccountFeature.TheEightAge ) )
				flags |= 0x0100; // The Eight Age

			if ( acct.FeatureEnabled( AccountFeature.NinthAnniversary ) )
				flags |= 0x0200; // 9th Anniversary Collection

			if ( acct.FeatureEnabled( AccountFeature.SeventhCharacter ) )
				flags |= 0x1000; // 7th Character slot

			// TODO: 0x2000 - 10th Age KR Faces

			m_Stream.Write( (ushort) flags );
		}
	}

	public class AttributeNormalizer
	{
		public static int Maximum { get; set; } = 25;

		public static bool Enabled { get; set; } = true;

		public static void Write( PacketWriter stream, int cur, int max )
		{
			if ( Enabled && max != 0 )
			{
				stream.Write( (short) Maximum );
				stream.Write( (short) ( ( cur * Maximum ) / max ) );
			}
			else
			{
				stream.Write( (short) max );
				stream.Write( (short) cur );
			}
		}

		public static void WriteReverse( PacketWriter stream, int cur, int max )
		{
			if ( Enabled && max != 0 )
			{
				stream.Write( (short) ( ( cur * Maximum ) / max ) );
				stream.Write( (short) Maximum );
			}
			else
			{
				stream.Write( (short) cur );
				stream.Write( (short) max );
			}
		}
	}

	public sealed class MobileHits : Packet
	{
		public MobileHits( Mobile m )
			: base( 0xA1, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) m.HitsMax );
			m_Stream.Write( (short) m.Hits );
		}
	}

	public sealed class MobileHitsN : Packet
	{
		public MobileHitsN( Mobile m )
			: base( 0xA1, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			AttributeNormalizer.Write( m_Stream, m.Hits, m.HitsMax );
		}
	}

	public sealed class MobileMana : Packet
	{
		public MobileMana( Mobile m )
			: base( 0xA2, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) m.ManaMax );
			m_Stream.Write( (short) m.Mana );
		}
	}

	public sealed class MobileManaN : Packet
	{
		public MobileManaN( Mobile m )
			: base( 0xA2, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			AttributeNormalizer.Write( m_Stream, m.Mana, m.ManaMax );
		}
	}

	public sealed class MobileStam : Packet
	{
		public MobileStam( Mobile m )
			: base( 0xA3, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) m.StamMax );
			m_Stream.Write( (short) m.Stam );
		}
	}

	public sealed class MobileStamN : Packet
	{
		public MobileStamN( Mobile m )
			: base( 0xA3, 9 )
		{
			m_Stream.Write( (int) m.Serial );
			AttributeNormalizer.Write( m_Stream, m.Stam, m.StamMax );
		}
	}

	public sealed class MobileAttributes : Packet
	{
		public MobileAttributes( Mobile m )
			: base( 0x2D, 17 )
		{
			m_Stream.Write( m.Serial );

			m_Stream.Write( (short) m.HitsMax );
			m_Stream.Write( (short) m.Hits );

			m_Stream.Write( (short) m.ManaMax );
			m_Stream.Write( (short) m.Mana );

			m_Stream.Write( (short) m.StamMax );
			m_Stream.Write( (short) m.Stam );
		}
	}

	public sealed class MobileAttributesN : Packet
	{
		public MobileAttributesN( Mobile m )
			: base( 0x2D, 17 )
		{
			m_Stream.Write( m.Serial );

			AttributeNormalizer.Write( m_Stream, m.Hits, m.HitsMax );
			AttributeNormalizer.Write( m_Stream, m.Mana, m.ManaMax );
			AttributeNormalizer.Write( m_Stream, m.Stam, m.StamMax );
		}
	}

	public sealed class PathfindMessage : Packet
	{
		public PathfindMessage( IPoint3D p )
			: base( 0x38, 7 )
		{
			m_Stream.Write( (short) p.X );
			m_Stream.Write( (short) p.Y );
			m_Stream.Write( (short) p.Z );
		}
	}

	// unsure of proper format, client crashes
	public sealed class MobileName : Packet
	{
		public MobileName( Mobile m )
			: base( 0x98 )
		{
			var name = m.Name;

			if ( name == null )
				name = "";

			EnsureCapacity( 37 );

			m_Stream.Write( (int) m.Serial );
			m_Stream.WriteAsciiFixed( name, 30 );
		}
	}

	public sealed class MobileAnimation : Packet
	{
		public MobileAnimation( Mobile m, int action, int subAction )
			: base( 0xE2, 10 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) action );
			m_Stream.Write( (short) subAction );
			m_Stream.Write( (byte) 0 );
		}
	}

	public sealed class OldMobileAnimation : Packet
	{
		public OldMobileAnimation( Mobile m, int action, int frameCount, int repeatCount, bool forward, bool repeat, int delay )
			: base( 0x6E, 14 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) action );
			m_Stream.Write( (short) frameCount );
			m_Stream.Write( (short) repeatCount );
			m_Stream.Write( (bool) !forward ); // protocol has really "reverse" but I find this more intuitive
			m_Stream.Write( (bool) repeat );
			m_Stream.Write( (byte) delay );
		}
	}

	public sealed class MobileStatusCompact : Packet
	{
		public MobileStatusCompact( bool canBeRenamed, Mobile m )
			: base( 0x11 )
		{
			var name = m.Name;
			if ( name == null )
				name = "";

			EnsureCapacity( 43 );

			m_Stream.Write( (int) m.Serial );
			m_Stream.WriteAsciiFixed( name, 30 );

			AttributeNormalizer.WriteReverse( m_Stream, m.Hits, m.HitsMax );

			m_Stream.Write( canBeRenamed );

			m_Stream.Write( (byte) 0 ); // type
		}
	}

	public sealed class MobileStatus : Packet
	{
		public MobileStatus( Mobile m )
			: this( m, m )
		{
		}

		public MobileStatus( Mobile beholder, Mobile beheld )
			: base( 0x11 )
		{
			var name = beheld.Name;

			if ( name == null )
				name = "";

			byte type = 0x0;

			if ( beholder == beheld )
				type = 0x6;

			var size = 0;

			var isEnhancedClient = beholder.NetState != null && beholder.NetState.Version.IsEnhanced;

			if ( type == 0 )
				size = 43;
			else if ( isEnhancedClient )
				size = 149;
			else
				size = 121;

			EnsureCapacity( size );

			m_Stream.Write( beheld.Serial );

			m_Stream.WriteAsciiFixed( name, 30 );

			if ( type > 0x0 )
				WriteAttr( beheld.Hits, beheld.HitsMax );
			else
				WriteAttrNorm( beheld.Hits, beheld.HitsMax );

			m_Stream.Write( beheld.CanBeRenamedBy( beholder ) );

			m_Stream.Write( type );

			if ( type > 0x0 )
			{
				m_Stream.Write( beheld.Female );

				m_Stream.Write( (short) beheld.Str );
				m_Stream.Write( (short) beheld.Dex );
				m_Stream.Write( (short) beheld.Int );

				WriteAttr( beheld.Stam, beheld.StamMax );
				WriteAttr( beheld.Mana, beheld.ManaMax );

				m_Stream.Write( (int) beheld.TotalGold );
				m_Stream.Write( (short) beheld.PhysicalResistance );
				m_Stream.Write( (short) ( Mobile.BodyWeight + beheld.TotalWeight ) );

				m_Stream.Write( (short) beheld.GetMaxWeight() );
				m_Stream.Write( (byte) ( beheld.Race.RaceID + 1 ) ); // Would be 0x00 if it's a non-ML enabled account but...

				m_Stream.Write( (short) beheld.StatCap );

				m_Stream.Write( (byte) beheld.Followers );
				m_Stream.Write( (byte) beheld.FollowersMax );

				m_Stream.Write( (short) beheld.FireResistance ); // Fire
				m_Stream.Write( (short) beheld.ColdResistance ); // Cold
				m_Stream.Write( (short) beheld.PoisonResistance ); // Poison
				m_Stream.Write( (short) beheld.EnergyResistance ); // Energy
				m_Stream.Write( (short) beheld.Luck ); // Luck

				var weapon = beheld.Weapon;

				int min = 0, max = 0;

				if ( weapon != null )
					weapon.GetStatusDamage( beheld, out min, out max );

				m_Stream.Write( (short) min ); // Damage min
				m_Stream.Write( (short) max ); // Damage max

				m_Stream.Write( (int) beheld.TithingPoints );

				if ( type >= 0x6 )
				{
					m_Stream.Write( (short) beheld.GetMaxResistance( ResistanceType.Physical ) );
					m_Stream.Write( (short) beheld.GetMaxResistance( ResistanceType.Fire ) );
					m_Stream.Write( (short) beheld.GetMaxResistance( ResistanceType.Cold ) );
					m_Stream.Write( (short) beheld.GetMaxResistance( ResistanceType.Poison ) );
					m_Stream.Write( (short) beheld.GetMaxResistance( ResistanceType.Energy ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.DefendChance ) );
					m_Stream.Write( (short) 45 ); // Max dci
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.AttackChance ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.WeaponSpeed ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.WeaponDamage ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.LowerRegCost ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.SpellDamage ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.CastRecovery ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.CastSpeed ) );
					m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.LowerManaCost ) );

					if ( isEnhancedClient )
					{
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.RegenHits ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.RegenStam ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.RegenMana ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.ReflectPhysical ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.EnhancePotions ) );
						m_Stream.Write( (short) beheld.GetStatOffset( StatType.Str ) );
						m_Stream.Write( (short) beheld.GetStatOffset( StatType.Dex ) );
						m_Stream.Write( (short) beheld.GetStatOffset( StatType.Int ) );
						m_Stream.Write( (short) 0 );
						m_Stream.Write( (short) 0 );
						m_Stream.Write( (short) 0 );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.BonusHits ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.BonusStam ) );
						m_Stream.Write( (short) beheld.GetMagicalAttribute( AosAttribute.BonusMana ) );
					}
				}
			}
		}

		private void WriteAttr( int current, int maximum )
		{
			m_Stream.Write( (short) current );
			m_Stream.Write( (short) maximum );
		}

		private void WriteAttrNorm( int current, int maximum )
		{
			AttributeNormalizer.WriteReverse( m_Stream, current, maximum );
		}
	}

	public sealed class MobileUpdate : Packet
	{
		public MobileUpdate( Mobile m )
			: base( 0x20, 19 )
		{
			var hue = m.Hue;

			if ( m.SolidHueOverride >= 0 )
				hue = m.SolidHueOverride;

			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (short) ( (int) m.Body ) );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (byte) m.GetPacketFlags() );
			m_Stream.Write( (short) m.X );
			m_Stream.Write( (short) m.Y );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (byte) m.Direction );
			m_Stream.Write( (sbyte) m.Z );
		}
	}

	public sealed class MobileIncoming : Packet
	{
		private static readonly int[] m_DupedLayers = new int[256];
		private static int m_Version;

		public Mobile m_Beheld;

		public MobileIncoming( Mobile beholder, Mobile beheld )
			: base( 0x78 )
		{
			var isPrior7033 = ( beholder.NetState != null && beholder.NetState.Version < ClientVersion.Client70330 );

			m_Beheld = beheld;
			++m_Version;

			var eq = beheld.GetEquippedItems();
			var count = eq.Count();

			if ( beheld.HairItemID > 0 )
				count++;
			if ( beheld.FacialHairItemID > 0 )
				count++;

			EnsureCapacity( 23 + ( count * 9 ) );

			var hue = beheld.Hue;

			if ( beheld.SolidHueOverride >= 0 )
				hue = beheld.SolidHueOverride;

			m_Stream.Write( (int) beheld.Serial );
			m_Stream.Write( (short) ( (int) beheld.Body ) );
			m_Stream.Write( (short) beheld.X );
			m_Stream.Write( (short) beheld.Y );
			m_Stream.Write( (sbyte) beheld.Z );
			m_Stream.Write( (byte) beheld.Direction );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (byte) beheld.GetPacketFlags() );
			m_Stream.Write( (byte) Notoriety.Compute( beholder, beheld ) );

			foreach ( var item in eq )
			{
				var layer = (byte) item.Layer;

				if ( !item.Deleted && beholder.CanSee( item ) && m_DupedLayers[layer] != m_Version )
				{
					m_DupedLayers[layer] = m_Version;

					hue = item.Hue;

					if ( beheld.SolidHueOverride >= 0 )
						hue = beheld.SolidHueOverride;

					var itemID = item.ItemID;

					if ( isPrior7033 )
						itemID = itemID | 0x8000;

					m_Stream.Write( (int) item.Serial );
					m_Stream.Write( (short) itemID );
					m_Stream.Write( (byte) layer );
					m_Stream.Write( (short) hue );
				}
			}

			if ( beheld.HairItemID > 0 )
			{
				if ( m_DupedLayers[(int) Layer.Hair] != m_Version )
				{
					m_DupedLayers[(int) Layer.Hair] = m_Version;
					hue = beheld.HairHue;

					if ( beheld.SolidHueOverride >= 0 )
						hue = beheld.SolidHueOverride;

					var itemID = beheld.HairItemID;

					if ( isPrior7033 )
						itemID = itemID | 0x8000;

					m_Stream.Write( (int) HairInfo.FakeSerial( beheld ) );
					m_Stream.Write( (short) itemID );
					m_Stream.Write( (byte) Layer.Hair );
					m_Stream.Write( (short) hue );
				}
			}

			if ( beheld.FacialHairItemID > 0 )
			{
				if ( m_DupedLayers[(int) Layer.FacialHair] != m_Version )
				{
					m_DupedLayers[(int) Layer.FacialHair] = m_Version;
					hue = beheld.FacialHairHue;

					if ( beheld.SolidHueOverride >= 0 )
						hue = beheld.SolidHueOverride;

					var itemID = beheld.FacialHairItemID;

					if ( isPrior7033 )
						itemID = itemID | 0x8000;

					m_Stream.Write( (int) FacialHairInfo.FakeSerial( beheld ) );
					m_Stream.Write( (short) itemID );
					m_Stream.Write( (byte) Layer.FacialHair );
					m_Stream.Write( (short) hue );
				}
			}

			m_Stream.Write( (int) 0 ); // terminate
		}
	}

	public sealed class HealthBarStatus : Packet
	{
		public HealthBarStatus( Mobile m, HealthBarColor color, bool enabled )
			: base( 0x17 )
		{
			EnsureCapacity( 12 );

			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (ushort) 1 );
			m_Stream.Write( (ushort) color );
			m_Stream.Write( (byte) ( enabled ? 1 : 0 ) );
		}
	}

	public sealed class AsciiMessage : Packet
	{
		public AsciiMessage( Serial serial, int graphic, MessageType type, int hue, int font, string name, string text )
			: base( 0x1C )
		{
			if ( name == null )
				name = "";

			if ( text == null )
				text = "";

			if ( hue == 0 )
				hue = 0x3B2;

			EnsureCapacity( 45 + text.Length );

			m_Stream.Write( (int) serial );
			m_Stream.Write( (short) graphic );
			m_Stream.Write( (byte) type );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (short) font );
			m_Stream.WriteAsciiFixed( name, 30 );
			m_Stream.WriteAsciiNull( text );
		}
	}

	public sealed class UnicodeMessage : Packet
	{
		public UnicodeMessage( Serial serial, int graphic, MessageType type, int hue, int font, string lang, string name, string text )
			: base( 0xAE )
		{
			if ( lang == null || lang == "" )
				lang = "ENU";
			if ( name == null )
				name = "";
			if ( text == null )
				text = "";

			if ( hue == 0 )
				hue = 0x3B2;

			EnsureCapacity( 50 + ( text.Length * 2 ) );

			m_Stream.Write( (int) serial );
			m_Stream.Write( (short) graphic );
			m_Stream.Write( (byte) type );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (short) font );
			m_Stream.WriteAsciiFixed( lang, 4 );
			m_Stream.WriteAsciiFixed( name, 30 );
			m_Stream.WriteBigUniNull( text );
		}
	}

	public sealed class PingAck : Packet
	{
		private static readonly PingAck[] m_Cache = new PingAck[0x100];

		public static PingAck Instantiate( byte ping )
		{
			var p = m_Cache[ping];

			if ( p == null )
			{
				m_Cache[ping] = p = new PingAck( ping );
				p.SetStatic();
			}

			return p;
		}

		public PingAck( byte ping )
			: base( 0x73, 2 )
		{
			m_Stream.Write( ping );
		}
	}

	public sealed class MovementRej : Packet
	{
		public MovementRej( int seq, Mobile m )
			: base( 0x21, 8 )
		{
			m_Stream.Write( (byte) seq );
			m_Stream.Write( (short) m.X );
			m_Stream.Write( (short) m.Y );
			m_Stream.Write( (byte) m.Direction );
			m_Stream.Write( (sbyte) m.Z );
		}
	}

	public sealed class MovementAck : Packet
	{
		private static readonly MovementAck[][] m_Cache = new MovementAck[8][]
			{
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256],
				new MovementAck[256]
			};

		public static MovementAck Instantiate( int seq, Mobile m )
		{
			var noto = Notoriety.Compute( m, m );

			var p = m_Cache[noto][seq];

			if ( p == null )
			{
				m_Cache[noto][seq] = p = new MovementAck( seq, noto );
				p.SetStatic();
			}

			return p;
		}

		private MovementAck( int seq, int noto )
			: base( 0x22, 3 )
		{
			m_Stream.Write( (byte) seq );
			m_Stream.Write( (byte) noto );
		}
	}

	public sealed class LoginConfirm : Packet
	{
		public LoginConfirm( Mobile m )
			: base( 0x1B, 37 )
		{
			m_Stream.Write( (int) m.Serial );
			m_Stream.Write( (int) 0 );
			m_Stream.Write( (short) ( (int) m.Body ) );
			m_Stream.Write( (short) m.X );
			m_Stream.Write( (short) m.Y );
			m_Stream.Write( (short) m.Z );
			m_Stream.Write( (byte) m.Direction );
			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (int) -1 );

			var map = m.Map;

			if ( map == null || map == Map.Internal )
				map = m.LogoutMap;

			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) 0 );
			m_Stream.Write( (short) ( map == null ? 6144 : map.Width ) );
			m_Stream.Write( (short) ( map == null ? 4096 : map.Height ) );

			m_Stream.Fill();
		}
	}

	public sealed class LoginComplete : Packet
	{
		public static readonly Packet Instance = SetStatic( new LoginComplete() );

		public LoginComplete()
			: base( 0x55, 1 )
		{
		}
	}

	public sealed class CityInfo
	{
		private Point3D m_Location;

		public CityInfo( string city, string building, int x, int y, int z, Map m, int cliloc )
		{
			City = city;
			Building = building;
			m_Location = new Point3D( x, y, z );
			Map = m;
			Cliloc = cliloc;
		}

		public string City { get; set; }

		public string Building { get; set; }

		public int X
		{
			get { return m_Location.X; }
			set { m_Location.X = value; }
		}

		public int Y
		{
			get { return m_Location.Y; }
			set { m_Location.Y = value; }
		}

		public int Z
		{
			get { return m_Location.Z; }
			set { m_Location.Z = value; }
		}

		public Point3D Location
		{
			get { return m_Location; }
			set { m_Location = value; }
		}

		public Map Map { get; set; }

		public int Cliloc { get; set; }
	}

	public sealed class CharacterListUpdate : Packet
	{
		public CharacterListUpdate( IAccount a )
			: base( 0x86 )
		{
			EnsureCapacity( 304 );

			m_Stream.Write( (byte) a.Count );

			for ( var i = 0; i < a.Length; ++i )
			{
				var m = a[i];

				if ( m != null )
				{
					m_Stream.WriteAsciiFixed( m.Name, 30 );
					m_Stream.Fill( 30 ); // password
				}
				else
				{
					m_Stream.Fill( 60 );
				}
			}
		}
	}

	public sealed class CharacterListHS : Packet
	{
		public CharacterListHS( IAccount a, CityInfo[] info )
			: base( 0xA9 )
		{
			EnsureCapacity( 311 + ( info.Length * 89 ) );

			var highSlot = -1;

			for ( var i = 0; i < a.Length; ++i )
			{
				if ( a[i] != null )
					highSlot = i;
			}

			var slotCount = Math.Max( Math.Max( highSlot + 1, a.Limit ), 5 );

			m_Stream.Write( (byte) slotCount );

			for ( var i = 0; i < slotCount; ++i )
			{
				if ( a[i] != null )
				{
					m_Stream.WriteAsciiFixed( a[i].Name, 30 );
					m_Stream.Fill( 30 ); // password
				}
				else
				{
					m_Stream.Fill( 60 );
				}
			}

			m_Stream.Write( (byte) info.Length );

			for ( var i = 0; i < info.Length; ++i )
			{
				var ci = info[i];

				m_Stream.Write( (byte) i );

				m_Stream.WriteAsciiFixed( ci.City, 32 );
				m_Stream.WriteAsciiFixed( ci.Building, 32 );

				m_Stream.Write( (int) ci.X );
				m_Stream.Write( (int) ci.Y );
				m_Stream.Write( (int) ci.Z );
				m_Stream.Write( (int) ci.Map.MapID );
				m_Stream.Write( (int) ci.Cliloc );
				m_Stream.Write( (int) 0 );
			}

			m_Stream.Write( (int) ( CharacterList.GetFlags( slotCount ) ) ); // flags
			m_Stream.Write( (short) -1 );
		}
	}

	public sealed class CharacterList : Packet
	{
		public CharacterList( IAccount a, CityInfo[] info )
			: base( 0xA9 )
		{
			EnsureCapacity( 309 + ( info.Length * 63 ) );

			var highSlot = -1;

			for ( var i = 0; i < a.Length; ++i )
			{
				if ( a[i] != null )
					highSlot = i;
			}

			var slotCount = Math.Max( Math.Max( highSlot + 1, a.Limit ), 5 );

			m_Stream.Write( (byte) slotCount );

			for ( var i = 0; i < slotCount; ++i )
			{
				if ( a[i] != null )
				{
					m_Stream.WriteAsciiFixed( a[i].Name, 30 );
					m_Stream.Fill( 30 ); // password
				}
				else
				{
					m_Stream.Fill( 60 );
				}
			}

			m_Stream.Write( (byte) info.Length );

			for ( var i = 0; i < info.Length; ++i )
			{
				var ci = info[i];

				m_Stream.Write( (byte) i );
				m_Stream.WriteAsciiFixed( ci.City, 31 );
				m_Stream.WriteAsciiFixed( ci.Building, 31 );
			}

			m_Stream.Write( (int) ( GetFlags( slotCount ) ) ); // flags
		}

		public static int AdditionalFlags { get; set; }

		public static int GetFlags( int slotCount )
		{
			var flags = 0x08; // Context Menus

			flags |= 0x20; // Age of Shadows
			flags |= 0x80; // Samurai Empire
			flags |= 0x100; // Mondain's Legacy

			if ( slotCount >= 7 )
				flags |= 0x1000; // 7th character slot
			else if ( slotCount >= 6 )
				flags |= 0x40; // 6th character slot
			else if ( slotCount == 1 )
				flags |= 0x14; // Limit characters & one character

			flags |= 0x200; // Kingdom Reborn
			flags |= 0x400; // Send Client Type
			flags |= 0x800; // Unknown flag for UO:KR client
			flags |= 0x2000; // Stygian Abyss

			return ( flags | AdditionalFlags );
		}
	}

	public class ClearWeaponAbility : Packet
	{
		public static readonly Packet Instance = SetStatic( new ClearWeaponAbility() );

		public ClearWeaponAbility()
			: base( 0xBF )
		{
			EnsureCapacity( 5 );

			m_Stream.Write( (short) 0x21 );
		}
	}

	public enum ALRReason : byte
	{
		Invalid = 0x00,
		InUse = 0x01,
		Blocked = 0x02,
		BadPass = 0x03,
		Idle = 0xFE,
		BadComm = 0xFF
	}

	public sealed class AccountLoginRej : Packet
	{
		public AccountLoginRej( ALRReason reason )
			: base( 0x82, 2 )
		{
			m_Stream.Write( (byte) reason );
		}
	}

	public enum AffixType : byte
	{
		Append = 0x00,
		Prepend = 0x01,
		System = 0x02
	}

	public sealed class MessageLocalizedAffix : Packet
	{
		public MessageLocalizedAffix( Serial serial, int graphic, MessageType messageType, int hue, int font, int number, string name, AffixType affixType, string affix, string args )
			: base( 0xCC )
		{
			if ( name == null )
				name = "";
			if ( affix == null )
				affix = "";
			if ( args == null )
				args = "";

			if ( hue == 0 )
				hue = 0x3B2;

			EnsureCapacity( 52 + affix.Length + ( args.Length * 2 ) );

			m_Stream.Write( (int) serial );
			m_Stream.Write( (short) graphic );
			m_Stream.Write( (byte) messageType );
			m_Stream.Write( (short) hue );
			m_Stream.Write( (short) font );
			m_Stream.Write( (int) number );
			m_Stream.Write( (byte) affixType );
			m_Stream.WriteAsciiFixed( name, 30 );
			m_Stream.WriteAsciiNull( affix );
			m_Stream.WriteBigUniNull( args );
		}
	}

	public sealed class ServerInfo
	{
		public string Name { get; set; }

		public int FullPercent { get; set; }

		public int TimeZone { get; set; }

		public IPEndPoint Address { get; set; }

		public ServerInfo( string name, int fullPercent, TimeZone tz, IPEndPoint address )
		{
			Name = name;
			FullPercent = fullPercent;
			TimeZone = tz.GetUtcOffset( DateTime.Now ).Hours;
			Address = address;
		}
	}

	public sealed class FollowMessage : Packet
	{
		public FollowMessage( Serial serial1, Serial serial2 )
			: base( 0x15, 9 )
		{
			m_Stream.Write( (int) serial1 );
			m_Stream.Write( (int) serial2 );
		}
	}

	public sealed class AccountLoginAck : Packet
	{
		public AccountLoginAck( ServerInfo[] info )
			: base( 0xA8 )
		{
			EnsureCapacity( 6 + ( info.Length * 40 ) );

			m_Stream.Write( (byte) 0x5D ); // Unknown

			m_Stream.Write( (ushort) info.Length );

			for ( var i = 0; i < info.Length; ++i )
			{
				var si = info[i];

				m_Stream.Write( (ushort) i );
				m_Stream.WriteAsciiFixed( si.Name, 32 );
				m_Stream.Write( (byte) si.FullPercent );
				m_Stream.Write( (sbyte) si.TimeZone );
				m_Stream.Write( (int) Utility.GetAddressValue( si.Address.Address ) );
			}
		}
	}

	public sealed class DisplaySignGump : Packet
	{
		public DisplaySignGump( Serial serial, int gumpID, string unknown, string caption )
			: base( 0x8B )
		{
			if ( unknown == null )
				unknown = "";
			if ( caption == null )
				caption = "";

			EnsureCapacity( 16 + unknown.Length + caption.Length );

			m_Stream.Write( (int) serial );
			m_Stream.Write( (short) gumpID );
			m_Stream.Write( (short) ( unknown.Length ) );
			m_Stream.WriteAsciiFixed( unknown, unknown.Length );
			m_Stream.Write( (short) ( caption.Length + 1 ) );
			m_Stream.WriteAsciiFixed( caption, caption.Length + 1 );
		}
	}

	public sealed class GodModeReply : Packet
	{
		public GodModeReply( bool reply )
			: base( 0x2B, 2 )
		{
			m_Stream.Write( reply );
		}
	}

	public sealed class PlayServerAck : Packet
	{
		internal static int m_AuthID = -1;

		public PlayServerAck( ServerInfo si )
			: base( 0x8C, 11 )
		{
			var addr = Utility.GetAddressValue( si.Address.Address );

			m_Stream.Write( (byte) addr );
			m_Stream.Write( (byte) ( addr >> 8 ) );
			m_Stream.Write( (byte) ( addr >> 16 ) );
			m_Stream.Write( (byte) ( addr >> 24 ) );

			m_Stream.Write( (short) si.Address.Port );
			m_Stream.Write( (int) m_AuthID );
		}
	}

	public sealed class InvalidDrop : Packet
	{
		public InvalidDrop( Serial serial )
			: base( 0x28, 5 )
		{
			m_Stream.Write( serial.Value );
		}
	}

	public sealed class ConfirmDrop : Packet
	{
		public static readonly Packet Instance = SetStatic( new ConfirmDrop() );

		public ConfirmDrop()
			: base( 0x29, 1 )
		{
		}
	}

	public sealed class CooldownInfo : Packet
	{
		public CooldownInfo( Item item, int seconds )
			: base( 0xBF )
		{
			EnsureCapacity( 15 );

			m_Stream.Write( (short) 0x31 ); // packet subcommand
			m_Stream.Write( (short) 0x1 );
			m_Stream.Write( (int) item.ItemID );
			m_Stream.Write( (int) seconds );
		}
	}

	public enum WaypointType : ushort
	{
		Corpse = 0x01,
		PartyMember = 0x02,
		RallyPoint = 0x03,
		QuestGiver = 0x04,
		QuestDestination = 0x05,
		Resurrection = 0x06,
		PointOfInterest = 0x07,
		Landmark = 0x08,
		Town = 0x09,
		Dungeon = 0x0A,
		Moongate = 0x0B,
		Shop = 0x0C,
		Player = 0x0D,
	}

	public class DisplayWaypoint : Packet
	{
		public DisplayWaypoint( IEntity e, WaypointType type, int cliLoc )
			: this( e.Serial, e.Location, e.Map, type, false, cliLoc, String.Empty )
		{
		}

		public DisplayWaypoint( IEntity e, WaypointType type, bool ignoreSerial, int cliLoc, string args )
			: this( e.Serial, e.Location, e.Map, type, ignoreSerial, cliLoc, args )
		{
		}

		public DisplayWaypoint( Serial serial, IPoint3D location, IMap map, WaypointType type, bool ignoreSerial, int cliLoc )
			: this( serial, location, map, type, ignoreSerial, cliLoc, String.Empty )
		{
		}

		public DisplayWaypoint( Serial serial, IPoint3D location, IMap map, WaypointType type, bool ignoreSerial, int cliLoc, string args )
			: base( 0xE5 )
		{
			if ( args == null )
				args = String.Empty;

			EnsureCapacity( 21 + ( args.Length * 2 ) );

			m_Stream.Write( (int) serial );

			m_Stream.Write( (ushort) location.X );
			m_Stream.Write( (ushort) location.Y );
			m_Stream.Write( (byte) location.Z );

			m_Stream.Write( (byte) map.MapID );

			m_Stream.Write( (ushort) type );

			m_Stream.Write( (ushort) ( ignoreSerial ? 1 : 0 ) );

			m_Stream.Write( cliLoc );
			m_Stream.WriteLittleUniNull( args );
		}
	}

	public class RemoveWaypoint : Packet
	{
		public RemoveWaypoint( Serial serial )
			: base( 0xE6, 5 )
		{
			m_Stream.Write( (int) serial );
		}
	}

	public interface IBoat : IEntity
	{
		Direction Facing { get; }
		Direction Moving { get; }
	}

	public enum BoatMovementSpeed : byte
	{
		None = 0x0,
		OneTileMovement = 0x1,
		MouseSlowMovement = 0x2,
		VerbalSlowMovement = 0x3,
		FastMovement = 0x4
	}

	public sealed class SmoothBoatMove : Packet
	{
		public SmoothBoatMove( IBoat boat, List<IEntity> entities, Point3D offset, Direction d, BoatMovementSpeed speed )
			: base( 0xF6 )
		{
			EnsureCapacity( 18 + entities.Count * 10 );

			m_Stream.Write( (int) boat.Serial );

			m_Stream.Write( (byte) speed );
			m_Stream.Write( (byte) d );
			m_Stream.Write( (byte) boat.Facing );

			m_Stream.Write( (short) ( boat.X + offset.X ) );
			m_Stream.Write( (short) ( boat.Y + offset.Y ) );
			m_Stream.Write( (byte) 0xFF );
			m_Stream.Write( (sbyte) ( boat.Z + offset.Z ) );

			m_Stream.Write( (byte) 0 );
			m_Stream.Write( (byte) entities.Count );

			for ( var i = 0; i < entities.Count; i++ )
			{
				var entity = entities[i];

				m_Stream.Write( (int) entity.Serial );
				m_Stream.Write( (short) ( entity.X + offset.X ) );
				m_Stream.Write( (short) ( entity.Y + offset.Y ) );
				m_Stream.Write( (byte) 0xFF );
				m_Stream.Write( (sbyte) ( entity.Z + offset.Z ) );
			}
		}
	}
}
