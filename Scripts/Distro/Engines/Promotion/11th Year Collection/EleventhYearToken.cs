using System;
using Server;
using Server.Accounting;
using Server.Engines.Promotion;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Mobiles;

using Y11Info = Server.Engines.Promotion.EleventhYearInfo;
using Y11Type = Server.Engines.Promotion.EleventhYearTokenType;

namespace Server.Engines.Promotion
{
	public enum EleventhYearTokenType
	{
		Master,

		Addons,
		DecoItems,
		Miscs,

		ArmorEngravingTool,
		BagForBulkOrderCovers,
		EarringOfProtection,

		AncestralGravestone,
		WoodenBookcase,
		LampPost,
		HitchingPost,

		SnowTree,
		FallenLog,
		WillowTree,
		MapleTree
	}

	public interface IRewardGiver
	{
		bool GiveReward( EleventhYearToken token, Mobile m );
	}

	public class EleventhYearInfo
	{
		public static Y11Info[] Definitions = new Y11Info[]
			{
				new Y11Info( 1071184, 1071185, new Y11TokenGiver( Y11Type.Addons, Y11Type.DecoItems, Y11Type.Miscs ) ),

				new Y11Info( 1071186, 1071187, new Y11TokenGiver( Y11Type.WillowTree, Y11Type.FallenLog, Y11Type.SnowTree, Y11Type.MapleTree ) ),
				new Y11Info( 1071188, 1071189, new Y11TokenGiver( Y11Type.AncestralGravestone, Y11Type.WoodenBookcase, Y11Type.LampPost, Y11Type.HitchingPost ) ),
				new Y11Info( 1071190, 1071191, new Y11TokenGiver( Y11Type.ArmorEngravingTool, Y11Type.BagForBulkOrderCovers, Y11Type.EarringOfProtection ) ),

				new Y11Info( 1080547, 1071163, new Y11ItemGiver( typeof( ArmorEngravingTool ) ) ),
				new Y11Info( 1071174, 1071165, new Y11ItemGiver( typeof( BagForBulkOrderCovers ) ) ),
				new Y11Info( 1071173, 1071172, new Y11GumpSender( typeof( EarringOfProtectionGump ) ) ),

				new Y11Info( 1071096, 1071164, new Y11ItemGiver( typeof( AncestralGravestone ) ) ),
				new Y11Info( 1071102, 1071166, new Y11ItemGiver( typeof( WoodenBookcase ) ) ),
				new Y11Info( 1071089, 1071161, new Y11ItemGiver( typeof( LampPostRoundStyle ) ) ),
				new Y11Info( 1071090, 1071162, new Y11ItemGiver( typeof( HitchingPostAnniversary ) ) ),

				new Y11Info( 1071103, 1071167, new Y11ItemGiver( typeof( SnowTreeDeed ) ) ),
				new Y11Info( 1071088, 1071160, new Y11ItemGiver( typeof( FallenLogDeed ) ) ),
				new Y11Info( 1071105, 1071169, new Y11ItemGiver( typeof( WillowTreeDeed ) ) ),
				new Y11Info( 1071104, 1071168, new Y11ItemGiver( typeof( MapleTreeDeed ) ) )
			};

		private int m_TokenArgument;
		private int m_GumpArgument;
		private IRewardGiver m_RewardGiver;

		public int TokenArgument { get { return m_TokenArgument; } }
		public int GumpArgument { get { return m_GumpArgument; } }

		public EleventhYearInfo( int tokenArg, int gumpArg, IRewardGiver giver )
		{
			m_TokenArgument = tokenArg;
			m_GumpArgument = gumpArg;
			m_RewardGiver = giver;
		}

		public void GiveReward( EleventhYearToken token, Mobile m )
		{
			if ( m_RewardGiver.GiveReward( token, m ) )
				token.Delete();
		}
	}

	public class Y11ItemGiver : IRewardGiver
	{
		private Type m_ItemType;

		public Y11ItemGiver( Type itemType )
		{
			m_ItemType = itemType;
		}

		public bool GiveReward( EleventhYearToken token, Mobile m )
		{
			try
			{
				Item reward = (Item) Activator.CreateInstance( m_ItemType );

				if ( !m.PlaceInBackpack( reward ) && !m.BankBox.TryDropItem( m, reward, false ) )
					return false;

				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	public class Y11GumpSender : IRewardGiver
	{
		private Type m_GumpType;

		public Y11GumpSender( Type gumpType )
		{
			m_GumpType = gumpType;
		}

		public bool GiveReward( EleventhYearToken token, Mobile m )
		{
			try
			{
				Gump gump = (Gump) Activator.CreateInstance( m_GumpType, token );

				m.CloseGump( m_GumpType );
				m.SendGump( gump );
			}
			catch
			{
			}

			return false;
		}
	}

	public class Y11TokenGiver : IRewardGiver
	{
		private Y11Type[] m_TokenTypes;

		public Y11TokenGiver( params Y11Type[] tokenTypes )
		{
			m_TokenTypes = tokenTypes;
		}

		public bool GiveReward( EleventhYearToken token, Mobile m )
		{
			for ( int i = 0; i < m_TokenTypes.Length; i++ )
			{
				Item reward = new EleventhYearToken( m_TokenTypes[i] );

				m.AddToBackpack( reward );
			}

			return true;
		}
	}
}

namespace Server.Items
{
	public class EleventhYearToken : Item
	{
		public override int LabelNumber { get { return 1070997; } } // A promotional token

		private Y11Type m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public Y11Type Type
		{
			get { return m_Type; }
			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

		public Y11Info Info
		{
			get { return EleventhYearInfo.Definitions[(int) m_Type]; }
		}

		[Constructable]
		public EleventhYearToken()
			: this( Y11Type.Master )
		{
		}

		[Constructable]
		public EleventhYearToken( Y11Type type )
			: base( 0x2AAA )
		{
			Type = type;

			Light = LightType.Circle300;

			LootType = LootType.Blessed;

			Weight = 1.0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1070998, String.Format( "#{0}", Info.TokenArgument ) ); // Use this to redeem your ~1_PROMO~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !this.IsChildOf( from.Backpack ) )
				return;

			from.CloseGump<EleventhYearConfirmGump>();
			from.SendGump( new EleventhYearConfirmGump( this ) );
		}

		public override bool OnDragLift( Mobile from )
		{
			from.CloseGump<EleventhYearConfirmGump>();

			return base.OnDragLift( from );
		}

		public EleventhYearToken( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Type = (EleventhYearTokenType) reader.ReadInt();
		}
	}

	public class EleventhYearConfirmGump : Gump
	{
		public override int TypeID { get { return 0x2335; } }

		private EleventhYearToken m_Token;

		public EleventhYearConfirmGump( EleventhYearToken token )
			: base( 10, 10 )
		{
			m_Token = token;

			AddPage( 0 );
			AddBackground( 0, 0, 240, 135, 0x2422 );

			AddHtmlLocalized( 15, 15, 210, 75, 1070972, 0x0, true, false ); // Click "OKAY" to redeem the following promotional item:
			AddHtmlLocalized( 15, 60, 210, 75, m_Token.Info.GumpArgument, 0x0, false, false );

			AddButton( 160, 95, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0 );
			AddButton( 90, 95, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
			{
				Mobile from = sender.Mobile;

				if ( !m_Token.Deleted && m_Token.IsChildOf( from ) )
					m_Token.Info.GiveReward( m_Token, from );
			}
		}
	}
}
