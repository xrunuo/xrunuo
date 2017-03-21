using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class ChoosePigmentGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private Mobile from;
		private IharaSoko minister;

		public ChoosePigmentGump( Mobile m, IharaSoko min )
			: base( 60, 36 )
		{
			from = m;

			minister = min;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );

			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 324, 0xA40 );
			AddImageTiled( 10, 374, 500, 20, 0xA40 );

			AddAlphaRegion( 10, 10, 500, 384 );

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

			AddHtmlLocalized( 14, 12, 500, 20, 1070986, 0x7FFF, false, false ); // Choose a pigment color.

			AddButtonTileArt( 14, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 100, 0xEFF, 0x501, 11, 19 );
			AddHtmlLocalized( 98, 44, 250, 60, 1070987, 0x7FFF, false, false ); // Paragon Gold

			AddButtonTileArt( 264, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 101, 0xEFF, 0x486, 11, 19 );
			AddHtmlLocalized( 348, 44, 250, 60, 1070988, 0x7FFF, false, false ); // Violet Courage Purple

			AddButtonTileArt( 14, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 102, 0xEFF, 0x4F2, 11, 19 );
			AddHtmlLocalized( 98, 108, 250, 60, 1070989, 0x7FFF, false, false ); // Invulnerability Blue

			AddButtonTileArt( 264, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 103, 0xEFF, 0x47E, 11, 19 );
			AddHtmlLocalized( 348, 108, 250, 60, 1070990, 0x7FFF, false, false ); // Luna White

			AddButtonTileArt( 14, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 104, 0xEFF, 0x48F, 11, 19 );
			AddHtmlLocalized( 98, 172, 250, 60, 1070991, 0x7FFF, false, false ); // Dryad Green

			AddButtonTileArt( 264, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 105, 0xEFF, 0x455, 11, 19 );
			AddHtmlLocalized( 348, 172, 250, 60, 1070992, 0x7FFF, false, false ); // Shadow Dancer Black

			AddButtonTileArt( 14, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 106, 0xEFF, 0x21, 11, 19 );
			AddHtmlLocalized( 98, 236, 250, 60, 1070993, 0x7FFF, false, false ); // Berserker Red

			AddButtonTileArt( 264, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 107, 0xEFF, 0x58C, 11, 19 );
			AddHtmlLocalized( 348, 236, 250, 60, 1070994, 0x7FFF, false, false ); // Nox Green

			AddButtonTileArt( 14, 300, 0x918, 0x919, GumpButtonType.Reply, 0, 108, 0xEFF, 0x66C, 11, 19 );
			AddHtmlLocalized( 98, 300, 250, 60, 1070995, 0x7FFF, false, false ); // Rum Red

			AddButtonTileArt( 264, 300, 0x918, 0x919, GumpButtonType.Reply, 0, 109, 0xEFF, 0x54F, 11, 19 );
			AddHtmlLocalized( 348, 300, 250, 60, 1070996, 0x7FFF, false, false ); // Fire Orange
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Item reward = null;

			switch ( info.ButtonID )
			{
				case 100:
					reward = new PigmentsOfTokunoMajor( PigmentsType.ParagonGold, 50 );
					break;
				case 101:
					reward = new PigmentsOfTokunoMajor( PigmentsType.VioletCouragePurple, 50 );
					break;
				case 102:
					reward = new PigmentsOfTokunoMajor( PigmentsType.InvulnerabilityBlue, 50 );
					break;
				case 103:
					reward = new PigmentsOfTokunoMajor( PigmentsType.LunaWhite, 50 );
					break;
				case 104:
					reward = new PigmentsOfTokunoMajor( PigmentsType.DryadGreen, 50 );
					break;
				case 105:
					reward = new PigmentsOfTokunoMajor( PigmentsType.ShadowDancerBlack, 50 );
					break;
				case 106:
					reward = new PigmentsOfTokunoMajor( PigmentsType.BerserkerRed, 50 );
					break;
				case 107:
					reward = new PigmentsOfTokunoMajor( PigmentsType.NoxGreen, 50 );
					break;
				case 108:
					reward = new PigmentsOfTokunoMajor( PigmentsType.RumRed, 50 );
					break;
				case 109:
					reward = new PigmentsOfTokunoMajor( PigmentsType.FireOrange, 50 );
					break;
			}

			if ( reward != null )
			{
				minister.Say( 1070984, String.Format( "#{0}", reward.LabelNumber ) ); // You have earned the gratitude of the Empire. I have placed the ~1_OBJTYPE~ in your backpack.

				from.AddToBackpack( reward );

				( (PlayerMobile) from ).ToTItemsTurnedIn = 0;
			}
			else
			{
				minister.Say( 1070982 ); // When you wish to choose your reward, you have but to approach me again.
			}
		}
	}
}