using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class ChooseRewardGump : Gump
	{
		public override int TypeID { get { return 0x2336; } }

		private Mobile from;
		private IharaSoko minister;

		public ChooseRewardGump( Mobile m, IharaSoko min )
			: base( 60, 36 )
		{
			minister = min;

			from = m;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );

			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 324, 0xA40 );
			AddImageTiled( 10, 374, 500, 20, 0xA40 );

			AddAlphaRegion( 10, 10, 500, 384 );

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
			AddHtmlLocalized( 14, 12, 500, 20, 1070985, 0x7FFF, false, false ); // Choose your reward.

			AddPage( 1 );

			AddButtonTileArt( 14, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 100, 0x27A9, 0, 15, 10 );
			AddTooltip( 1071002 ); // <body><basefont color="#ffff00">Swords of Prosperity<basefont color="#ffffff"><br>Spell Channeling<br>Mage Weapon -0 Skill<br>Luck 200<br>Faster Casting 1<br>Fire Damage 100%</basefont></basefont></body>
			AddHtmlLocalized( 98, 44, 250, 60, 1070963, 0x7FFF, false, false ); // Swords of Prosperity

			AddButtonTileArt( 264, 44, 0x918, 0x919, GumpButtonType.Reply, 0, 101, 0x27A2, 0, 5, 10 );
			AddTooltip( 1070978 ); // <body><basefont color="#ffff00">Sword of the Stampede<basefont color="#ffffff"><br>Hit Harm 100%<br>Hit Chance Increase 10%<br>Damage Increase 60%<br>Cold Damage 100%</basefont></basefont></body>
			AddHtmlLocalized( 348, 44, 250, 60, 1070964, 0x7FFF, false, false ); // Sword of the Stampede

			AddButtonTileArt( 14, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 102, 0x27A3, 0, 17, 10 );
			AddTooltip( 1071003 ); // <body><basefont color="#ffff00">Wind's Edge<basefont color="#ffffff"><br>Defense Chance Increase 10%<br>Swing Speed Increase 50%<br>Damage Increase 50%<br>Energy Damage 100%</basefont></basefont></body>
			AddHtmlLocalized( 98, 108, 250, 60, 1070965, 0x7FFF, false, false ); // Wind's Edge

			AddButtonTileArt( 264, 108, 0x918, 0x919, GumpButtonType.Reply, 0, 103, 0x27AD, 0, 16, 12 );
			AddTooltip( 1071004 ); // <body><basefont color="#ffff00">Darkened Sky<basefont color="#ffffff"><br>Hit Lightning 60%<br>Swing Speed Increase 25%<br>Damage Increase 50%<br>Cold Damage 50%<br>Energy Damage 50%</basefont></basefont></body>
			AddHtmlLocalized( 348, 108, 250, 60, 1070966, 0x7FFF, false, false ); // Darkened Sky

			AddButtonTileArt( 14, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 104, 0x27A5, 0, 2, 10 );
			AddTooltip( 1071005 ); // <body><basefont color="#ffff00">The Horselord<basefont color="#ffffff"><br>Hit Lower Defense 50%<br>Hit Stamina Leech 50%<br>Spell Channeling<br>Dexterity Bonus 5<br>Damage Increase 50%</basefont></basefont></body>
			AddHtmlLocalized( 98, 172, 250, 60, 1070967, 0x7FFF, false, false ); // The Horselord

			AddButtonTileArt( 264, 172, 0x918, 0x919, GumpButtonType.Reply, 0, 105, 0x277D, 0, 17, 12 );
			AddTooltip( 1071006 ); // <body><basefont color="#ffff00">Rune Beetle Carapace<basefont color="#ffffff"><br>Mana Increase 10<br>Mana Regeneration 3<br>Lower Mana Cost 15%<br>Physical Resist 5%<br>Fire Resist 3%<br>Cold Resist 14%<br>Poison Resist 3%<br>Energy Resist 14%<br>Lower Requirements 100%<br>Mage Armor</basefont></basefont></body>
			AddHtmlLocalized( 348, 172, 250, 60, 1070968, 0x7FFF, false, false ); // Rune Beetle Carapace

			AddButtonTileArt( 14, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 106, 0x2798, 0, 17, 16 );
			AddTooltip( 1071007 ); // <body><basefont color="#ffff00">Kasa of the Raj-In<basefont color="#ffffff"><br>Spell Damage Increase 12%<br>Physical Resist 12%<br>Fire Resist 17%<br>Cold Resist 21%<br>Poison Resist 17%<br>Energy Resist 17%</basefont></basefont></body>
			AddHtmlLocalized( 98, 236, 250, 60, 1070969, 0x7FFF, false, false ); // Kasa of the Raj-in

			AddButtonTileArt( 264, 236, 0x918, 0x919, GumpButtonType.Reply, 0, 107, 0x2792, 0, 16, 17 );
			AddTooltip( 1071008 ); // <body><basefont color="#ffff00">Stormgrip<basefont color="#ffffff"><br>Intelligence Bonus 8<br>Lower Reagent Cost 25%<br>Physical Resist 2%<br>Fire Resist 4%<br>Cold Resist 18%<br>Poison Resist 3%<br>Energy Resist 18%</basefont></basefont></body>
			AddHtmlLocalized( 348, 236, 250, 60, 1070970, 0x7FFF, false, false ); // Stormgrip

			AddButtonTileArt( 14, 300, 0x918, 0x919, GumpButtonType.Reply, 0, 108, 0xEFA, 0x530, 17, 19 );
			AddTooltip( 1071009 ); // <body><basefont color="#ffff00">Tome of Lost Knowledge<basefont color="#ffffff"><br>Magery +15<br>Intelligence Bonus 8<br>Spell Damage Increase 15%<br>Lower Mana Cost 15%</basefont></basefont></body>
			AddHtmlLocalized( 98, 300, 250, 60, 1070971, 0x7FFF, false, false ); // Tome of Lost Knowledge

			AddButtonTileArt( 264, 300, 0x918, 0x919, GumpButtonType.Reply, 0, 109, 0xEFF, 0, 11, 19 );
			AddTooltip( 1071011 ); // <body><basefont color="#ffff00">Pigments of Tokuno<basefont color="#ffffff"><br>Use to dye artifacts and enhanced metal items<br>50 charges<br>Click to choose a color</basefont></basefont></body>
			AddHtmlLocalized( 348, 300, 250, 60, 1070933, 0x7FFF, false, false ); // Pigments of Tokuno
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Item reward = null;

			switch ( info.ButtonID )
			{
				case 100:
					reward = new SwordsOfProsperity();
					break;
				case 101:
					reward = new SwordOfStampede();
					break;
				case 102:
					reward = new WindsEdge();
					break;
				case 103:
					reward = new DarkenedSky();
					break;
				case 104:
					reward = new TheHorselord();
					break;
				case 105:
					reward = new RuneBeetleCarapace();
					break;
				case 106:
					reward = new KasaOfRajin();
					break;
				case 107:
					reward = new Stormgrip();
					break;
				case 108:
					reward = new TomeOfLostKnowledge();
					break;
				case 109:
					{
						from.CloseGump( typeof( ChooseRewardGump ) );
						from.SendGump( new ChoosePigmentGump( from, minister ) );
						break;
					}
			}

			if ( reward != null )
			{
				minister.Say( 1070984, String.Format( "#{0}", reward.LabelNumber ) ); // You have earned the gratitude of the Empire. I have placed the ~1_OBJTYPE~ in your backpack.

				from.AddToBackpack( reward );

				( (PlayerMobile) from ).ToTItemsTurnedIn = 0;
			}
			else if ( info.ButtonID != 109 )
			{
				minister.Say( 1070982 ); // When you wish to choose your reward, you have but to approach me again.
			}
		}
	}
}