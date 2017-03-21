using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class SeventhAniversaryGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private PromotionalToken m_Token;

		public SeventhAniversaryGump( PromotionalToken token )
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

			AddHtmlLocalized( 14, 12, 500, 20, 1062928, 0x7FFF, false, false ); // UO Seventh Anniversary gift

			AddButtonTileArt( 14, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 100, 0x14F0, 0, 15, 15 );
			AddHtmlLocalized( 98, 44, 250, 60, 1062919, 0x7FFF, false, false ); // Hearth of the Home Fire

			AddButtonTileArt( 264, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 101, 0xF61, 0, 0, 15 );
			AddHtmlLocalized( 348, 44, 250, 60, 1062921, 0x7FFF, false, false ); // The Holy Sword

			AddButtonTileArt( 14, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 102, 0x1411, 0, 15, 15 );
			AddHtmlLocalized( 98, 108, 250, 60, 1062911, 0x7FFF, false, false ); // Royal Leggings of Embers

			AddButtonTileArt( 264, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 103, 0x234D, 0, 15, 15 );
			AddHtmlLocalized( 348, 108, 250, 60, 1062913, 0x7FFF, false, false ); // Rose of Trinsic

			AddButtonTileArt( 14, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 104, 0x236C, 0, 15, 15 );
			AddHtmlLocalized( 98, 172, 250, 60, 1062923, 0x7FFF, false, false ); // Ancient Samurai Helm

			AddButtonTileArt( 264, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 105, 0x26C3, 5, 20, 15 );
			AddHtmlLocalized( 348, 172, 250, 60, 1062915, 0x7FFF, false, false ); // Shamino�s Best Crossbow

			AddButtonTileArt( 14, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 106, 0x234E, 0, 15, 0 );
			AddHtmlLocalized( 98, 236, 250, 60, 1062917, 0x7FFF, false, false ); // The Tapestry of Sosaria
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( !m_Token.IsChildOf( from.Backpack ) )
				return;

			Item reward = null;

			switch ( info.ButtonID )
			{
				case 100:
					reward = new HearthOfHomeFireDeed();
					break;
				case 101:
					reward = new HolySword();
					break;
				case 102:
					reward = new LeggingsOfEmbers();
					break;
				case 103:
					reward = new RoseOfTrinsic();
					break;
				case 104:
					reward = new SamuraiHelm();
					break;
				case 105:
					reward = new ShaminoCrossbow();
					break;
				case 106:
					reward = new TapestryOfSosaria();
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