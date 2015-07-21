using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Items.MusicBox;

namespace Server.Gumps
{
	public class EighthAnniversaryGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private PromotionalToken m_Token;

		public EighthAnniversaryGump( PromotionalToken token )
			: base( 0, 0 )
		{
			m_Token = token;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );

			AddImageTiled( 10, 10, 500, 20, 0xA40 );

			AddImageTiled( 10, 40, 500, 324, 0xA40 );

			AddImageTiled( 10, 374, 500, 20, 0xA40 );

			AddAlphaRegion( 10, 10, 500, 384 );

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

			AddHtml( 14, 12, 500, 20, @"<basefont color=#FFFFFF>UO Eighth Anniversary gift</basefont>", false, false ); // UO Eighth Anniversary gift

			AddButtonTileArt( 14, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 100, 0x2B01, 0, 15, 15 );
			AddHtmlLocalized( 98, 44, 250, 60, 1075196, 0x7FFF, false, false ); // Dupre’s Shield

			AddButtonTileArt( 264, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 101, 0x2253, 0, 20, 15 );
			AddHtmlLocalized( 348, 44, 250, 60, 1078148, 0x7FFF, false, false ); // Ossian Grimoire

			AddButtonTileArt( 14, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 102, 0x2D98, 0, 20, 15 );
			AddHtmlLocalized( 98, 108, 250, 60, 1078142, 0x7FFF, false, false ); // Talisman of the Fey:<br>Ferret

			AddButtonTileArt( 264, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 103, 0x2D97, 0, 15, 15 );
			AddHtmlLocalized( 348, 108, 250, 60, 1078143, 0x7FFF, false, false ); // Talisman of the Fey:<br>Squirrel

			AddButtonTileArt( 14, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 104, 0x2D96, 0, 15, 5 );
			AddHtmlLocalized( 98, 172, 250, 60, 1078144, 0x7FFF, false, false ); // Talisman of the Fey:<br>Cu Sidhe

			AddButtonTileArt( 264, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 105, 0x2D95, 0, 10, 5 );
			AddHtmlLocalized( 348, 172, 250, 60, 1078145, 0x7FFF, false, false ); // Talisman of the Fey:<br>Reptalon

			AddButtonTileArt( 14, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 106, 0x2B02, 0, -10, 10 );
			AddHtmlLocalized( 98, 236, 250, 60, 1075201, 0x7FFF, false, false ); // Quiver of Infinity

			AddButtonTileArt( 264, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 107, 0x2AF9, 0, -10, 0 );
			AddHtmlLocalized( 348, 236, 250, 60, 1075198, 0x7FFF, false, false ); // Dawn's Music Box

			AddButtonTileArt( 14, 300, 0x918, 0x919, GumpButtonType.Reply, 0, 108, 0x2AC6, 0, 25, 0 );
			AddHtmlLocalized( 98, 300, 250, 60, 1075197, 0x7FFF, false, false ); // Fountain Of Life
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( !m_Token.IsChildOf( from.Backpack ) )
				return;

			Item reward = null;

			switch ( info.ButtonID )
			{
				case 100:
					reward = new DupresShield();
					break;
				case 101:
					reward = new OssianGrimoire();
					break;
				case 102:
					reward = new FerretFormTalisman();
					break;
				case 103:
					reward = new SquirrelFormTalisman();
					break;
				case 104:
					reward = new CuSidheFormTalisman();
					break;
				case 105:
					reward = new ReptalonFormTalisman();
					break;
				case 106:
					reward = new QuiverOfInfinity();
					break;
				case 107:
					reward = new DawnsMusicBox();
					break;
				case 108:
					reward = new FountainOfLife();
					break;
			}

			if ( reward != null )
			{
				if ( !from.AddToBackpack( reward ) )
				{
					if ( from.BankBox != null )
						from.BankBox.DropItem( reward );
				}

				if ( m_Token != null )
					m_Token.Delete();
			}
		}
	}
}
