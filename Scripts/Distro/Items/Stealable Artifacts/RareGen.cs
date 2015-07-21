using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public class StealableArtifactsSpawner : Item
	{
		public class StealableEntry
		{
			private Map m_Map;
			private Point3D m_Location;
			private int m_MinDelay;
			private int m_MaxDelay;
			private Type m_Type;

			public Map Map { get { return m_Map; } }
			public Point3D Location { get { return m_Location; } }
			public int MinDelay { get { return m_MinDelay; } }
			public int MaxDelay { get { return m_MaxDelay; } }
			public Type Type { get { return m_Type; } }

			public StealableEntry( Map map, Point3D location, int minDelay, int maxDelay, Type type )
			{
				m_Map = map;
				m_Location = location;
				m_MinDelay = minDelay;
				m_MaxDelay = maxDelay;
				m_Type = type;
			}

			public Item CreateInstance()
			{
				Item item = (Item) Activator.CreateInstance( m_Type );

				item.Movable = false;
				item.MoveToWorld( this.Location, this.Map );

				return item;
			}
		}

		private static StealableEntry[] m_Entries = new StealableEntry[]
			{
				/*				
				// Basket
				new StealableEntry( Map.Felucca, new Point3D( 3778, 1251, 6 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Trammel, new Point3D( 3778, 1251, 6 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Felucca, new Point3D( 3778, 1250, 6 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Trammel, new Point3D( 3778, 1250, 6 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Felucca, new Point3D( 3787, 1122, 20 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Trammel, new Point3D( 3787, 1122, 20 ), 24, 25, typeof( BasketRegular ) ),
        		new StealableEntry( Map.Felucca, new Point3D( 3684, 2205, 20 ), 24, 25, typeof( BasketRegular ) ),
				new StealableEntry( Map.Trammel, new Point3D( 3684, 2205, 20 ), 24, 25, typeof( BasketRegular ) ),

				// Fruit Basket
        		new StealableEntry( Map.Felucca, new Point3D( 284, 986, 4 ), 24, 25, typeof( FruitBasketA ) ),
				new StealableEntry( Map.Trammel, new Point3D( 284, 986, 4 ), 24, 25, typeof( FruitBasketA ) ),

				// Broken Chair
        		new StealableEntry( Map.Ilshenar, new Point3D( 148, 946, -29 ), 24, 25, typeof( BrokenChair ) ),

				// Rocks
        		new StealableEntry( Map.Felucca, new Point3D( 2683, 2056, 17 ), 24, 25, typeof( Rocks ) ),
				new StealableEntry( Map.Trammel, new Point3D( 2683, 2056, 17 ), 24, 25, typeof( Rocks ) ),

				// Rock
        		new StealableEntry( Map.Felucca, new Point3D( 5511, 3116, -4 ), 24, 25, typeof( RockA ) ),
				new StealableEntry( Map.Trammel, new Point3D( 5511, 3116, -4 ), 24, 25, typeof( RockA ) ),

				// Scattered Hay
        		new StealableEntry( Map.Felucca, new Point3D( 5999, 3773, 21 ), 24, 25, typeof( Hay ) ),

				// Full Jars
        		new StealableEntry( Map.Felucca, new Point3D( 3656, 2506, 0 ), 24, 25, typeof( FullJars ) ),

				// Tall Candle
        		new StealableEntry( Map.Felucca, new Point3D( 5577, 1829, 6 ), 24, 25, typeof( TallCandle ) ),
				new StealableEntry( Map.Trammel, new Point3D( 5577, 1829, 6 ), 24, 25, typeof( TallCandle ) ),
        		new StealableEntry( Map.Felucca, new Point3D( 5583, 1829, 6 ), 24, 25, typeof( TallCandle ) ),
				new StealableEntry( Map.Trammel, new Point3D( 5583, 1829, 6 ), 24, 25, typeof( TallCandle ) ),

				// Bucket
        		new StealableEntry( Map.Felucca, new Point3D( 1129, 2238, 40 ), 24, 25, typeof( Bucket ) ),
				new StealableEntry( Map.Trammel, new Point3D( 1129, 2238, 40 ), 24, 25, typeof( Bucket ) ),
        		new StealableEntry( Map.Felucca, new Point3D( 1130, 2239, 40 ), 24, 25, typeof( Bucket ) ),
				new StealableEntry( Map.Trammel, new Point3D( 1130, 2239, 40 ), 24, 25, typeof( Bucket ) ),
        		new StealableEntry( Map.Felucca, new Point3D( 1140, 2239, 40 ), 24, 25, typeof( Bucket ) ),
				new StealableEntry( Map.Trammel, new Point3D( 1140, 2239, 40 ), 24, 25, typeof( Bucket ) ),

				// Closed Barrel
        		new StealableEntry( Map.Felucca, new Point3D( 5191, 587, 0 ), 24, 25, typeof( ClosedBarrel ) ),
				new StealableEntry( Map.Trammel, new Point3D( 5191, 587, 0 ), 24, 25, typeof( ClosedBarrel ) ),
				*/

				// Doom - Artifact rarity 1
				new StealableEntry( Map.Malas, new Point3D( 312,  55, -1 ), 72, 108, typeof( Rock ) ),
				new StealableEntry( Map.Malas, new Point3D( 361,  31,  5 ), 72, 108, typeof( SkullCandle ) ),
				new StealableEntry( Map.Malas, new Point3D( 369, 371,  0 ), 72, 108, typeof( BottleA ) ),
				new StealableEntry( Map.Malas, new Point3D( 378, 372,  0 ), 72, 108, typeof( DamagedBooks ) ),
				// Doom - Artifact rarity 2
				new StealableEntry( Map.Malas, new Point3D( 432,  19, -1 ), 144, 216, typeof( StretchedHide ) ),
				new StealableEntry( Map.Malas, new Point3D( 462,  10, -1 ), 144, 216, typeof( BrazierA ) ),
				// Doom - Artifact rarity 3
				new StealableEntry( Map.Malas, new Point3D( 471,  96, -1 ), 288, 432, typeof( LampPost ) ),
				new StealableEntry( Map.Malas, new Point3D( 418, 198,  0 ), 288, 432, typeof( StackedBooks1 ) ),
				new StealableEntry( Map.Malas, new Point3D( 426, 189, -1 ), 288, 432, typeof( StackedBooks2 ) ),
				new StealableEntry( Map.Malas, new Point3D( 429, 193, -1 ), 288, 432, typeof( FallenBook ) ),
				// Doom - Artifact rarity 5
				new StealableEntry( Map.Malas, new Point3D( 445,   9,  6 ), 1152, 1728, typeof( StuddedLeggings ) ),
				new StealableEntry( Map.Malas, new Point3D( 422,  25,  0 ), 1152, 1728, typeof( EggCase ) ),
				new StealableEntry( Map.Malas, new Point3D( 347,  41, -1 ), 1152, 1728, typeof( SkinnedGoat ) ),
				new StealableEntry( Map.Malas, new Point3D( 498,  54, -1 ), 1152, 1728, typeof( GruesomeStandard ) ),
				new StealableEntry( Map.Malas, new Point3D( 381, 375, 10 ), 1152, 1728, typeof( BloodyWater ) ),
				new StealableEntry( Map.Malas, new Point3D( 489, 370,  5 ), 1152, 1728, typeof( TarotCards ) ),
				new StealableEntry( Map.Malas, new Point3D( 497, 368, 12 ), 1152, 1728, typeof( ReverseBackpack ) ),
				// Doom - Artifact rarity 7
				new StealableEntry( Map.Malas, new Point3D( 491,   8,  5 ), 4608, 6912, typeof( StuddedTunic1 ) ),
				new StealableEntry( Map.Malas, new Point3D( 423,  23, -1 ), 4608, 6912, typeof( Cocoon ) ),
				// Doom - Artifact rarity 8
				new StealableEntry( Map.Malas, new Point3D( 352,  32, -1 ), 9216, 13824, typeof( SkinnedDeer ) ),
				// Doom - Artifact rarity 9
				new StealableEntry( Map.Malas, new Point3D( 433,  12, -1 ), 18432, 27648, typeof( Saddle ) ),
				new StealableEntry( Map.Malas, new Point3D( 460,  9,   6 ), 18432, 27648, typeof( LeatherTunic ) ),
				// Doom - Artifact rarity 10
				new StealableEntry( Map.Malas, new Point3D( 259,  72, -1 ), 36864, 55296, typeof( ZyronicClawA ) ),
				new StealableEntry( Map.Malas, new Point3D( 354, 176,  8 ), 36864, 55296, typeof( TitansHammerA ) ),
				new StealableEntry( Map.Malas, new Point3D( 369, 389, -1 ), 36864, 55296, typeof( BladeOfTheRighteousA ) ),
				new StealableEntry( Map.Malas, new Point3D( 467,  92,  4 ), 36864, 55296, typeof( InquisitorsResolutionA ) ),
				// Doom - Artifact rarity 12
				new StealableEntry( Map.Malas, new Point3D( 488, 364, -1 ), 147456, 221184, typeof( RuinedPaintingA ) ),

				// Yomotsu Mines - Artifact rarity 1
				new StealableEntry( Map.Malas, new Point3D(  18, 110, -1 ), 72, 108, typeof( BasketA ) ),
				new StealableEntry( Map.Malas, new Point3D(  66, 114, -1 ), 72, 108, typeof( BasketB ) ),
				// Yomotsu Mines - Artifact rarity 2
				new StealableEntry( Map.Malas, new Point3D(  63,  12, 11 ), 144, 216, typeof( BasketD ) ),
				new StealableEntry( Map.Malas, new Point3D(   5,  29, -1 ), 144, 216, typeof( BasketE0 ) ),
				new StealableEntry( Map.Malas, new Point3D(  30,  81,  3 ), 144, 216, typeof( BasketE ) ),
				// Yomotsu Mines - Artifact rarity 3
				new StealableEntry( Map.Malas, new Point3D( 115,   7, -1 ), 288, 432, typeof( UrnA ) ),
				new StealableEntry( Map.Malas, new Point3D(  85,  13, -1 ), 288, 432, typeof( UrnB ) ),
				new StealableEntry( Map.Malas, new Point3D( 110,  53, -1 ), 288, 432, typeof( SculptureA ) ),
				new StealableEntry( Map.Malas, new Point3D( 108,  37, -1 ), 288, 432, typeof( SculptureC ) ),
				new StealableEntry( Map.Malas, new Point3D( 121,  14, -1 ), 288, 432, typeof( Teapot ) ),
				new StealableEntry( Map.Malas, new Point3D( 121, 115, -1 ), 288, 432, typeof( Teapot1 ) ),
				new StealableEntry( Map.Malas, new Point3D(  84,  40, -1 ), 288, 432, typeof( TowerLantern ) ),
				// Yomotsu Mines - Artifact rarity 9
				new StealableEntry( Map.Malas, new Point3D(  94,   7, -1 ), 18432, 27648, typeof( SculptureE ) ),

				// Fan Dancer's Dojo - Artifact rarity 1
				new StealableEntry( Map.Malas, new Point3D( 112, 640,  0 ), 72, 108, typeof( BasketC ) ),
				new StealableEntry( Map.Malas, new Point3D( 102, 355, -1 ), 72, 108, typeof( BasketC1 ) ),
				// Fan Dancer's Dojo - Artifact rarity 2
				new StealableEntry( Map.Malas, new Point3D(  99, 370, -1 ), 144, 216, typeof( BasketF ) ),
				new StealableEntry( Map.Malas, new Point3D( 100, 357, -1 ), 144, 216, typeof( ZenRockGardenA ) ),
				// Fan Dancer's Dojo - Artifact rarity 3
				new StealableEntry( Map.Malas, new Point3D(  72, 473, -1 ), 288, 432, typeof( FanAO ) ),
				new StealableEntry( Map.Malas, new Point3D(  99, 372, -1 ), 288, 432, typeof( FanA ) ),
				new StealableEntry( Map.Malas, new Point3D(  92, 326, -1 ), 288, 432, typeof( BowlsLight ) ),
				new StealableEntry( Map.Malas, new Point3D(  98, 469, -1 ), 288, 432, typeof( ZenRockGardenB ) ),
				new StealableEntry( Map.Malas, new Point3D( 104, 691, -1 ), 288, 432, typeof( ZenRockGardenC ) ),
				// Fan Dancer's Dojo - Artifact rarity 4
				new StealableEntry( Map.Malas, new Point3D( 103, 336, -1 ), 576, 864, typeof( PaintingA0 ) ),
				new StealableEntry( Map.Malas, new Point3D(  59, 380, -1 ), 576, 864, typeof( PaintingA ) ),
				new StealableEntry( Map.Malas, new Point3D(  85, 401, -1 ), 576, 864, typeof( PaintingBO ) ),
				new StealableEntry( Map.Malas, new Point3D(  59, 391, -1 ), 576, 864, typeof( PaintingB ) ),
				new StealableEntry( Map.Malas, new Point3D( 107, 483, -1 ), 576, 864, typeof( FanBO ) ),
				new StealableEntry( Map.Malas, new Point3D(  50, 475, -1 ), 576, 864, typeof( FanB ) ),
				new StealableEntry( Map.Malas, new Point3D( 107, 460, -1 ), 576, 864, typeof( Bowl ) ),
				new StealableEntry( Map.Malas, new Point3D(  90, 502, -1 ), 576, 864, typeof( Cups ) ),
				new StealableEntry( Map.Malas, new Point3D( 107, 688, -1 ), 576, 864, typeof( BowlsDark ) ),
				new StealableEntry( Map.Malas, new Point3D( 112, 676, -1 ), 576, 864, typeof( Sake ) ),
				// Fan Dancer's Dojo - Artifact rarity 5
				new StealableEntry( Map.Malas, new Point3D( 134, 614, -1 ), 1152, 1728, typeof( SwordDisplayAO ) ),
				new StealableEntry( Map.Malas, new Point3D(  50, 482, -1 ), 1152, 1728, typeof( SwordDisplayA ) ),
				new StealableEntry( Map.Malas, new Point3D( 119, 672, -1 ), 1152, 1728, typeof( PaintingCO ) ),
				// Fan Dancer's Dojo - Artifact rarity 6
				new StealableEntry( Map.Malas, new Point3D(  90, 326, -1 ), 2304, 3456, typeof( PaintingDO ) ),
				new StealableEntry( Map.Malas, new Point3D(  99, 354, -1 ), 2304, 3456, typeof( PaintingD ) ),
				new StealableEntry( Map.Malas, new Point3D( 179, 652, -1 ), 2304, 3456, typeof( SwordDisplayBO ) ),
				new StealableEntry( Map.Malas, new Point3D( 118, 626, -1 ), 2304, 3456, typeof( SwordDisplayB ) ),
				// Fan Dancer's Dojo - Artifact rarity 7
				new StealableEntry( Map.Malas, new Point3D(  90, 483, -1 ), 4608, 6912, typeof( Flowers ) ),
				// Fan Dancer's Dojo - Artifact rarity 8
				new StealableEntry( Map.Malas, new Point3D(  71, 562, -1 ), 9216, 13824, typeof( SculptureD ) ),
				new StealableEntry( Map.Malas, new Point3D( 102, 677, -1 ), 9216, 13824, typeof( SculptureDO ) ),
				new StealableEntry( Map.Malas, new Point3D(  61, 495, -1 ), 9216, 13824, typeof( SwordDisplayC ) ),
				new StealableEntry( Map.Malas, new Point3D( 180, 668, -1 ), 9216, 13824, typeof( SwordDisplayCO ) ),
				new StealableEntry( Map.Malas, new Point3D( 162, 647, -1 ), 9216, 13824, typeof( SwordDisplayD ) ),
				new StealableEntry( Map.Malas, new Point3D( 100, 489, -1 ), 18432, 27648, typeof( SwordDisplayDO ) ),
				new StealableEntry( Map.Malas, new Point3D( 123, 624, -1 ), 9216, 13824, typeof( PaintingEO ) ),
				new StealableEntry( Map.Malas, new Point3D( 146, 649, -1 ), 9216, 13824, typeof( PaintingE ) ),
				// Fan Dancer's Dojo - Artifact rarity 9
				new StealableEntry( Map.Malas, new Point3D( 174, 606,  0 ), 18432, 27648, typeof( SwordDisplayEO ) ),
				new StealableEntry( Map.Malas, new Point3D( 157, 608, -1 ), 18432, 27648, typeof( SwordDisplayE ) ),
				new StealableEntry( Map.Malas, new Point3D( 187, 643, -1 ), 18432, 27648, typeof( PaintingFO ) ),
				new StealableEntry( Map.Malas, new Point3D( 146, 623,  0 ), 18432, 27648, typeof( PaintingF ) ),
				new StealableEntry( Map.Malas, new Point3D( 181, 629,  0 ), 18432, 27648, typeof( SculptureEO ) ),
			
				// Underworld - Artifact rarity 3
				new StealableEntry( Map.TerMur, new Point3D( 1239, 1019, -37 ), 288, 432, typeof( JugsofGoblinRotgut ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1046, 1106, -63 ), 288, 432, typeof( MysteriousSupper ) ),
				// Underworld - Artifact rarity 4
				new StealableEntry( Map.TerMur, new Point3D( 1210, 1035, -22 ), 576, 864, typeof( BottlesofSpoiledWine2 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1015, 1029, -36 ), 576, 864, typeof( StolenBottlesofLiquor2 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1015, 1013, -36 ), 576, 864, typeof( StolenBottlesofLiquor2 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1077,  975, -23 ), 576, 864, typeof( NavreysWebA1 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1087,  981, -16 ), 576, 864, typeof( NavreysWebA2 ) ),
				// Underworld - Artifact rarity 5
				new StealableEntry( Map.TerMur, new Point3D( 1047, 1108, -65 ), 1152, 1728, typeof( MysticsGuard ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1119,  974, -41 ), 1152, 1728, typeof( NavreysWebB1 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1147, 1012, -52 ), 1152, 1728, typeof( NavreysWebB2 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1081,  847,   3 ), 1152, 1728, typeof( NavreysWeb3 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1064,  868, -28 ), 1152, 1728, typeof( NavreysWeb4 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1134, 1204,   7 ), 1152, 1728, typeof( DirtyPlateA ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1137, 1134, -38 ), 1152, 1728, typeof( RemnantsOfMeatLoaf ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1048, 1109, -65 ), 1152, 1728, typeof( BloodySpoon ) ),
				// Underworld - Artifact rarity 6
				new StealableEntry( Map.TerMur, new Point3D( 1188, 1015, -36 ), 2304, 3456, typeof( BottlesofSpoiledWine3 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1015, 1017, -34 ), 2304, 3456, typeof( BatteredPan ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1007,  975, -24 ), 2304, 3456, typeof( RustedPan ) ),
				// Underworld - Artifact rarity 7
				new StealableEntry( Map.TerMur, new Point3D( 1226,  963, -25 ), 4608, 6912, typeof( BottlesofSpoiledWine4 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1015, 1026, -37 ), 4608, 6912, typeof( StolenBottlesofLiquor3 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1089, 1126, -38 ), 4608, 6912, typeof( DriedUpInkWell ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1227,  964, -29 ), 4608, 6912, typeof( FakeCopperIngots ) ),
				// Underworld - Artifact rarity 8
				new StealableEntry( Map.TerMur, new Point3D( 1031,  998, -39 ), 9216, 13824, typeof( StolenBottlesofLiquor4 ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1226,  966, -29 ), 9216, 13824, typeof( PricelessTreasure ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1017, 1150, -64 ), 9216, 13824, typeof( RottedOars ) ),
				// Underworld - Artifact rarity 9
				new StealableEntry( Map.TerMur, new Point3D( 1131, 1128, -42 ), 18432, 27648, typeof( BlockAndTackle ) ),
				new StealableEntry( Map.TerMur, new Point3D( 1066, 1193, -38 ), 18432, 27648, typeof( TyballsFlaskStand ) ),

				// Stygian Abyss - Artifact rarity 5
				new StealableEntry( Map.TerMur, new Point3D( 877, 527, -13 ), 1152, 1728, typeof( CrownofArcaneTemperament ) ),
				new StealableEntry( Map.TerMur, new Point3D( 345, 617,  26 ), 1152, 1728, typeof( LightInTheVoid ) ),
				new StealableEntry( Map.TerMur, new Point3D( 578, 856, -46 ), 1152, 1728, typeof( StaffOfResonance ) ),
				new StealableEntry( Map.TerMur, new Point3D( 841, 666,  25 ), 1152, 1728, typeof( ValkyriesGlaive ) ),
				new StealableEntry( Map.TerMur, new Point3D( 716, 416,  51 ), 1152, 1728, typeof( DyingPlant ) ),
				new StealableEntry( Map.TerMur, new Point3D( 950, 542, -14 ), 1152, 1728, typeof( LargePewterBowl ) ),
				// Stygian Abyss - Artifact rarity 6
				new StealableEntry( Map.TerMur, new Point3D( 785, 438,  -15 ), 2304, 3456, typeof( LargeDyingPlant ) ),
				new StealableEntry( Map.TerMur, new Point3D( 849, 282,   -4 ), 2304, 3456, typeof( GargishLuckTotem ) ),
				new StealableEntry( Map.TerMur, new Point3D( 715, 782,   27 ), 2304, 3456, typeof( GargishProtectiveTotem ) ),
				new StealableEntry( Map.TerMur, new Point3D( 916, 374,   -6 ), 2304, 3456, typeof( BookofTruth ) ),
				new StealableEntry( Map.TerMur, new Point3D( 670, 818, -107 ), 2304, 3456, typeof( GargishTraditionalVase ) ),
				// Stygian Abyss - Artifact rarity 7
				new StealableEntry( Map.TerMur, new Point3D( 555, 670, 55 ), 4608, 6912, typeof( GargishKnowledgeTotem ) ),
				new StealableEntry( Map.TerMur, new Point3D( 948, 394, 88 ), 4608, 6912, typeof( GargishMemorialStatue ) ),
				new StealableEntry( Map.TerMur, new Point3D( 368, 605, 28 ), 4608, 6912, typeof( GargishBentasVase ) ),
				new StealableEntry( Map.TerMur, new Point3D( 670, 441, 50 ), 4608, 6912, typeof( GargishPortrait ) ),
				// Stygian Abyss - Artifact rarity 8
				new StealableEntry( Map.TerMur, new Point3D( 926, 598, -6 ), 9216, 13824, typeof( PushmePullyu ) )
			};

		public static StealableEntry[] Entries { get { return m_Entries; } }

		private static StealableArtifactsSpawner m_Instance;

		public static StealableArtifactsSpawner Instance { get { return m_Instance; } }

		public static void Initialize()
		{
			CommandSystem.Register( "RaresGenerate", AccessLevel.Owner, new CommandEventHandler( RaresGenerate_OnCommand ) );
			CommandSystem.Register( "RaresDelete", AccessLevel.Owner, new CommandEventHandler( RaresDelete_OnCommand ) );
		}

		[Usage( "RaresGenerate" )]
		[Description( "Generates the stealable artifacts spawner." )]
		private static void RaresGenerate_OnCommand( CommandEventArgs args )
		{
			Mobile from = args.Mobile;

			if ( Create() )
				from.SendMessage( "Stealable artifacts spawner generated." );
			else
				from.SendMessage( "Stealable artifacts spawner already present." );
		}

		[Usage( "RaresDelete" )]
		[Description( "Removes the stealable artifacts spawner and every not yet stolen stealable artifacts." )]
		private static void RaresDelete_OnCommand( CommandEventArgs args )
		{
			Mobile from = args.Mobile;

			if ( Remove() )
				from.SendMessage( "Stealable artifacts spawner removed." );
			else
				from.SendMessage( "Stealable artifacts spawner not present." );
		}

		public static bool Create()
		{
			if ( m_Instance != null && !m_Instance.Deleted )
				return false;

			m_Instance = new StealableArtifactsSpawner();
			return true;
		}

		public static bool Remove()
		{
			if ( m_Instance == null )
				return false;

			m_Instance.Delete();
			m_Instance = null;
			return true;
		}

		public static StealableInstance GetStealableInstance( Item item )
		{
			if ( Instance == null )
				return null;

			return (StealableInstance) Instance.m_Table[item];
		}

		public class StealableInstance
		{
			private StealableEntry m_Entry;
			private Item m_Item;
			private DateTime m_NextRespawn;

			public StealableEntry Entry { get { return m_Entry; } }

			public Item Item
			{
				get { return m_Item; }
				set
				{
					if ( m_Item != null && value == null )
					{
						int delay = Utility.RandomMinMax( this.Entry.MinDelay, this.Entry.MaxDelay );
						this.NextRespawn = DateTime.Now + TimeSpan.FromMinutes( delay );
					}

					if ( Instance != null )
					{
						if ( m_Item != null )
							Instance.m_Table.Remove( m_Item );

						if ( value != null )
							Instance.m_Table[value] = this;
					}

					m_Item = value;
				}
			}

			public DateTime NextRespawn
			{
				get { return m_NextRespawn; }
				set { m_NextRespawn = value; }
			}

			public StealableInstance( StealableEntry entry )
				: this( entry, null, DateTime.Now )
			{
			}

			public StealableInstance( StealableEntry entry, Item item, DateTime nextRespawn )
			{
				m_Item = item;
				m_NextRespawn = nextRespawn;
				m_Entry = entry;
			}

			public void CheckRespawn()
			{
				if ( this.Item != null && ( this.Item.Deleted || this.Item.Movable || this.Item.Parent != null ) )
					this.Item = null;

				if ( this.Item == null && DateTime.Now >= this.NextRespawn )
					this.Item = this.Entry.CreateInstance();
			}
		}

		private Timer m_RespawnTimer;
		private StealableInstance[] m_Artifacts;
		private Hashtable m_Table;

		private StealableArtifactsSpawner()
			: base( 1 )
		{
			Name = "Stealable Artifacts Spawner - Internal";
			Movable = false;

			m_Artifacts = new StealableInstance[m_Entries.Length];
			m_Table = new Hashtable( m_Entries.Length );

			for ( int i = 0; i < m_Entries.Length; i++ )
				m_Artifacts[i] = new StealableInstance( m_Entries[i] );

			m_RespawnTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromMinutes( 15.0 ), new TimerCallback( CheckRespawn ) );
		}

		public override void OnDelete()
		{
			base.OnDelete();

			if ( m_RespawnTimer != null )
			{
				m_RespawnTimer.Stop();
				m_RespawnTimer = null;
			}

			foreach ( StealableInstance si in m_Artifacts )
			{
				if ( si.Item != null )
					si.Item.Delete();
			}

			m_Instance = null;
		}

		public void CheckRespawn()
		{
			foreach ( StealableInstance si in m_Artifacts )
			{
				si.CheckRespawn();
			}
		}

		public StealableArtifactsSpawner( Serial serial )
			: base( serial )
		{
			m_Instance = this;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.WriteEncodedInt( m_Artifacts.Length );

			for ( int i = 0; i < m_Artifacts.Length; i++ )
			{
				StealableInstance si = m_Artifacts[i];

				writer.Write( (Item) si.Item );
				writer.WriteDeltaTime( (DateTime) si.NextRespawn );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Artifacts = new StealableInstance[m_Entries.Length];
			m_Table = new Hashtable( m_Entries.Length );

			int length = reader.ReadEncodedInt();

			for ( int i = 0; i < length; i++ )
			{
				Item item = reader.ReadItem();
				DateTime nextRespawn = reader.ReadDeltaTime();

				if ( i < m_Artifacts.Length )
				{
					StealableInstance si = new StealableInstance( m_Entries[i], item, nextRespawn );
					m_Artifacts[i] = si;

					if ( si.Item != null )
						m_Table[si.Item] = si;
				}
			}

			for ( int i = length; i < m_Entries.Length; i++ )
				m_Artifacts[i] = new StealableInstance( m_Entries[i] );

			m_RespawnTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromMinutes( 15.0 ), new TimerCallback( CheckRespawn ) );
		}
	}
}
