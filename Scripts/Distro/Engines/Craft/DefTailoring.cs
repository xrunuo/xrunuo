using System;
using Server.Items;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum TailorRecipe
	{
		ElvenQuiver = 65,
		QuiverOfFire = 67,
		QuiverOfIce = 68,
		QuiverOfBlight = 66,
		QuiverOfLightning = 69
	}

	public enum TailorRecipeGreater
	{
		SongWovenMantle = 70,
		SpellWovenBritches = 71,
		StitchersMittens = 72
	}
	#endregion

	public class DefTailoring : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefTailoring();
		}

		public override SkillName MainSkill { get { return SkillName.Tailoring; } }

		public override int GumpTitleNumber
		{
			get { return 1044005; } // <CENTER>TAILORING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		public override CraftECA ECA { get { return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double DefaultChanceAtMin
		{
			get { return 0.5; }
		}

		private DefTailoring()
			: base( 1, 1, 1.25 ) // base( 1, 1, 4.5 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x248 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				if ( makersMark && exceptional )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( exceptional )
					return 1044155; // You create an exceptional quality item.
				else
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region Materials
			AddCraft( 1, typeof( CutUpCloth ), 1044457, 1044458, 0.0, 0.0, typeof( BoltOfCloth ), 1044453, 1, 1044253 );
			AddCraft( 2, typeof( CombineCloth ), 1044457, 1044459, 0.0, 0.0, typeof( Cloth ), 1044455, 1, 1044253 );
			#endregion

			#region Hats
			AddCraft( 3, typeof( SkullCap ), 1011375, 1025444, 0.0, 25.0, typeof( Cloth ), 1044455, 2, 1044287 );
			AddCraft( 4, typeof( Bandana ), 1011375, 1025440, 0.0, 25.0, typeof( Cloth ), 1044455, 2, 1044287 );
			AddCraft( 5, typeof( FloppyHat ), 1011375, 1025907, 6.2, 31.2, typeof( Cloth ), 1044455, 11, 1044287 );
			AddCraft( 6, typeof( Cap ), 1011375, 1025909, -18.8, 6.2, typeof( Cloth ), 1044455, 11, 1044287 );
			AddCraft( 7, typeof( WideBrimHat ), 1011375, 1025908, 6.2, 31.2, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 8, typeof( StrawHat ), 1011375, 1025911, 6.2, 31.2, typeof( Cloth ), 1044455, 10, 1044287 );
			AddCraft( 9, typeof( TallStrawHat ), 1011375, 1025910, 6.7, 31.7, typeof( Cloth ), 1044455, 13, 1044287 );
			AddCraft( 10, typeof( WizardsHat ), 1011375, 1025912, 7.2, 32.2, typeof( Cloth ), 1044455, 15, 1044287 );
			AddCraft( 11, typeof( Bonnet ), 1011375, 1025913, 6.2, 31.2, typeof( Cloth ), 1044455, 11, 1044287 );
			AddCraft( 12, typeof( FeatheredHat ), 1011375, 1025914, 6.2, 31.2, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 13, typeof( TricorneHat ), 1011375, 1025915, 6.2, 31.2, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 14, typeof( JesterHat ), 1011375, 1025916, 7.2, 32.2, typeof( Cloth ), 1044455, 15, 1044287 );

			AddCraft( 15, typeof( FlowerGarland ), 1011375, 1028965, 10.0, 35.0, typeof( Cloth ), 1044455, 5, 1044287 );

			craft = AddCraft( 16, typeof( ClothNinjaHood ), 1011375, 1030202, 80.0, 105.0, typeof( Cloth ), 1044455, 13, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 17, typeof( Kasa ), 1011375, 1030211, 60.0, 85.0, typeof( Cloth ), 1044455, 12, 1044287 );
			craft.RequiresSE = true;
			#endregion

			#region Shirts and Pants
			AddCraft( 18, typeof( Doublet ), 1111747, 1028059, 0, 25.0, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 19, typeof( Shirt ), 1111747, 1025399, 20.7, 45.7, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 20, typeof( FancyShirt ), 1111747, 1027933, 24.8, 49.8, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 21, typeof( Tunic ), 1111747, 1028097, 00.0, 25.0, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 22, typeof( Surcoat ), 1111747, 1028189, 8.2, 33.2, typeof( Cloth ), 1044455, 14, 1044287 );
			AddCraft( 23, typeof( PlainDress ), 1111747, 1027937, 12.4, 37.4, typeof( Cloth ), 1044455, 10, 1044287 );
			AddCraft( 24, typeof( FancyDress ), 1111747, 1027935, 33.1, 58.1, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 25, typeof( Cloak ), 1111747, 1025397, 41.4, 66.4, typeof( Cloth ), 1044455, 14, 1044287 );
			AddCraft( 26, typeof( Robe ), 1111747, 1027939, 53.9, 78.9, typeof( Cloth ), 1044455, 16, 1044287 );
			AddCraft( 27, typeof( JesterSuit ), 1111747, 1028095, 8.2, 33.2, typeof( Cloth ), 1044455, 24, 1044287 );

			AddCraft( 28, typeof( FurCape ), 1111747, 1028969, 35.0, 60.0, typeof( Cloth ), 1044455, 13, 1044287 );
			AddCraft( 29, typeof( GildedDress ), 1111747, 1028973, 37.5, 62.5, typeof( Cloth ), 1044455, 16, 1044287 );
			AddCraft( 30, typeof( FormalShirt ), 1111747, 1028975, 26.0, 51.0, typeof( Cloth ), 1044455, 16, 1044287 );

			craft = AddCraft( 31, typeof( ClothNinjaJacket ), 1111747, 1030207, 75.0, 100.0, typeof( Cloth ), 1044455, 12, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 32, typeof( Kamishimo ), 1111747, 1030212, 75.0, 100.0, typeof( Cloth ), 1044455, 15, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 33, typeof( HakamaShita ), 1111747, 1030215, 40.0, 65.0, typeof( Cloth ), 1044455, 14, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 34, typeof( MaleKimono ), 1111747, 1030189, 50.0, 75.0, typeof( Cloth ), 1044455, 16, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 35, typeof( FemaleKimono ), 1111747, 1030190, 50.0, 75.0, typeof( Cloth ), 1044455, 16, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 36, typeof( JinBaori ), 1111747, 1030220, 30.0, 55.0, typeof( Cloth ), 1044455, 12, 1044287 );
			craft.RequiresSE = true;

			AddCraft( 37, typeof( ShortPants ), 1111747, 1025422, 24.8, 49.8, typeof( Cloth ), 1044455, 6, 1044287 );
			AddCraft( 38, typeof( LongPants ), 1111747, 1025433, 24.8, 49.8, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 39, typeof( Kilt ), 1111747, 1025431, 20.7, 45.7, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 40, typeof( Skirt ), 1111747, 1025398, 29.0, 54.0, typeof( Cloth ), 1044455, 10, 1044287 );
			AddCraft( 41, typeof( FurSarong ), 1111747, 1028971, 35.0, 60.0, typeof( Cloth ), 1044455, 12, 1044287 );

			craft = AddCraft( 42, typeof( Hakama ), 1111747, 1030213, 50.0, 75.0, typeof( Cloth ), 1044455, 16, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 43, typeof( TattsukeHakama ), 1111747, 1030214, 50.0, 75.0, typeof( Cloth ), 1044455, 16, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 70, typeof( ElvenShirt ), 1111747, 1032219, 80.0, 105.0, typeof( Cloth ), 1044455, 10, 1044287 );
			craft.RequiresML = true;
			craft = AddCraft( 71, typeof( ElvenShirtDark ), 1111747, 1032219, 80.0, 105.0, typeof( Cloth ), 1044455, 10, 1044287 );
			craft.RequiresML = true;
			craft = AddCraft( 72, typeof( ElvenPants ), 1111747, 1032227, 80.0, 105.0, typeof( Cloth ), 1044455, 12, 1044287 );
			craft.RequiresML = true;
			craft = AddCraft( 73, typeof( ElvenRobe ), 1111747, 1032217, 80.0, 105.0, typeof( Cloth ), 1044455, 30, 1044287 );
			craft.RequiresML = true;
			craft = AddCraft( 74, typeof( FemaleElvenRobe ), 1111747, 1032218, 80.0, 105.0, typeof( Cloth ), 1044455, 30, 1044287 );
			craft.RequiresML = true;

			craft = AddCraft( 76, typeof( WoodlandBelt ), 1111747, 1031112, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044287 );
			craft.RequiresML = true;

			AddCraft( 83, typeof( GargishRobe ), 1111747, 1095256, 53.9, 78.9, typeof( Cloth ), 1044455, 16, 1044287 );
			AddCraft( 84, typeof( GargishFancyRobe ), 1111747, 1095258, 75.0, 100.0, typeof( Cloth ), 1044455, 18, 1044287 );
			#endregion

			#region Miscellaneous
			AddCraft( 44, typeof( BodySash ), 1015283, 1025441, 4.1, 29.1, typeof( Cloth ), 1044455, 4, 1044287 );
			AddCraft( 45, typeof( HalfApron ), 1015283, 1025435, 20.7, 45.7, typeof( Cloth ), 1044455, 6, 1044287 );
			AddCraft( 46, typeof( FullApron ), 1015283, 1025437, 29.0, 54.0, typeof( Cloth ), 1044455, 10, 1044287 );

			craft = AddCraft( 47, typeof( Obi ), 1015283, 1030219, 20.0, 45.0, typeof( Cloth ), 1044455, 6, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 48, typeof( ElvenQuiver ), 1015283, 1032657, 60.0, 115.0, typeof( Leather ), 1044462, 28, 1044287 );
			craft.AddRecipe( (int) TailorRecipe.ElvenQuiver, this );
			craft.RequiresML = true;

			craft = AddCraft( 49, typeof( QuiverOfFire ), 1015283, 1073109, 60.0, 115.0, typeof( Leather ), 1044462, 28, 1044287 );
			craft.AddRes( typeof( FireRuby ), 1026254, 15, 1044253 );
			craft.AddRecipe( (int) TailorRecipe.QuiverOfFire, this );
			craft.RequiresML = true;

			craft = AddCraft( 50, typeof( QuiverOfIce ), 1015283, 1073110, 60.0, 115.0, typeof( Leather ), 1044462, 28, 1044287 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 15, 1044253 );
			craft.AddRecipe( (int) TailorRecipe.QuiverOfIce, this );
			craft.RequiresML = true;

			craft = AddCraft( 51, typeof( QuiverOfBlight ), 1015283, 1073111, 60.0, 115.0, typeof( Leather ), 1044462, 28, 1044287 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRecipe( (int) TailorRecipe.QuiverOfBlight, this );
			craft.RequiresML = true;

			craft = AddCraft( 52, typeof( QuiverOfLightning ), 1015283, 1073112, 60.0, 115.0, typeof( Leather ), 1044462, 28, 1044287 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRecipe( (int) TailorRecipe.QuiverOfLightning, this );
			craft.RequiresML = true;

			craft = AddCraft( 53, typeof( LeatherContainerEngravingTool ), 1015283, 1072152, 75.0, 100.0, typeof( Leather ), 1044462, 6, 1044287 );
			craft.AddRes( typeof( Bone ), 1049064, 1, 1049063 );
			craft.AddRes( typeof( Dyes ), 1024009, 1, 1044253 );
			craft.AddRes( typeof( SpoolOfThread ), 1073462, 2, 1044253 );
			craft.RequiresML = true;

			AddCraft( 668, typeof( GargishHalfApron ), 1015283, 1115391, 20.7, 45.7, typeof( Cloth ), 1044455, 6, 1044287 );
			AddCraft( 669, typeof( GargishSash ), 1015283, 1115388, 4.1, 29.1, typeof( Cloth ), 1044455, 4, 1044287 );

			AddCraft( 501, typeof( OilCloth ), 1015283, 1041498, 74.6, 99.6, typeof( Cloth ), 1044455, 1, 1044287 );

			craft = AddCraft( 502, typeof( GozaMatEastDeed ), 1015283, 1030404, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 503, typeof( GozaMatSouthDeed ), 1015283, 1030405, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 504, typeof( SquareGozaMatEastDeed ), 1015283, 1030407, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 505, typeof( SquareGozaMatSouthDeed ), 1015283, 1030406, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 506, typeof( BrocadeGozaMatEastDeed ), 1015283, 1030408, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 507, typeof( BrocadeGozaMatSouthDeed ), 1015283, 1030409, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 508, typeof( BrocadeSquareGozaMatEastDeed ), 1015283, 1030411, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 509, typeof( BrocadeSquareGozaMatSouthDeed ), 1015283, 1030410, 55.0, 80.0, typeof( Cloth ), 1044455, 25, 1044287 );
			craft.RequiresSE = true;
			#endregion

			#region Footwear
			craft = AddCraft( 77, typeof( ElvenBoots ), 1015288, 1032228, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044287 );
			craft.RequiresML = true;

			AddCraft( 601, typeof( FurBoots ), 1015288, 1028967, 50.0, 75.0, typeof( Cloth ), 1044455, 12, 1044287 );

			craft = AddCraft( 602, typeof( NinjaTabi ), 1015288, 1030210, 70.0, 95.0, typeof( Cloth ), 1044455, 10, 1044287 );
			craft.RequiresSE = true;
			craft = AddCraft( 603, typeof( SamuraiTabi ), 1015288, 1030209, 20.0, 45.0, typeof( Cloth ), 1044455, 6, 1044287 );
			craft.RequiresSE = true;

			AddCraft( 604, typeof( Sandals ), 1015288, 1025901, 12.4, 37.4, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( 605, typeof( Shoes ), 1015288, 1025904, 16.5, 41.5, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( 606, typeof( Boots ), 1015288, 1025899, 33.1, 58.1, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( 607, typeof( ThighBoots ), 1015288, 1025906, 41.4, 66.4, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( 667, typeof( GargishLeatherTalons ), 1015288, 1115390, 40.4, 65.4, typeof( Leather ), 1044462, 6, 1044463 );
			#endregion

			#region Leather Armor
			craft = AddCraft( 80, typeof( SpellWovenBritches ), 1015293, 1072929, 92.5, 117.5, typeof( Leather ), 1044462, 15, 1044463 );
			craft.AddRes( typeof( EyeOfTheTravesty ), 1032685, 1, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRecipe( (int) TailorRecipeGreater.SpellWovenBritches, this );
			craft.ForceNonExceptional = true;

			craft = AddCraft( 81, typeof( SongWovenMantle ), 1015293, 1072931, 92.5, 117.5, typeof( Leather ), 1044462, 15, 1044463 );
			craft.AddRes( typeof( EyeOfTheTravesty ), 1032685, 1, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Muculent ), 1032680, 10, 1044253 );
			craft.AddRecipe( (int) TailorRecipeGreater.SongWovenMantle, this );
			craft.ForceNonExceptional = true;

			craft = AddCraft( 82, typeof( StitchersMittens ), 1015293, 1072932, 92.5, 117.5, typeof( Leather ), 1044462, 15, 1044463 );
			craft.AddRes( typeof( CapturedEssence ), 1032686, 1, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.AddRecipe( (int) TailorRecipeGreater.StitchersMittens, this );
			craft.ForceNonExceptional = true;

			AddCraft( 608, typeof( LeatherGorget ), 1015293, 1025063, 53.9, 78.9, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( 609, typeof( LeatherCap ), 1015293, 1027609, 6.2, 31.2, typeof( Leather ), 1044462, 2, 1044463 );
			AddCraft( 610, typeof( LeatherGloves ), 1015293, 1025062, 51.8, 76.8, typeof( Leather ), 1044462, 3, 1044463 );
			AddCraft( 611, typeof( LeatherArms ), 1015293, 1025061, 53.9, 78.9, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( 612, typeof( LeatherLegs ), 1015293, 1025067, 66.3, 91.3, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( 613, typeof( LeatherChest ), 1015293, 1025068, 70.5, 95.5, typeof( Leather ), 1044462, 12, 1044463 );

			craft = AddCraft( 614, typeof( LeatherJingasa ), 1015293, 1030177, 45.0, 70.0, typeof( Leather ), 1044462, 4, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 615, typeof( LeatherMempo ), 1015293, 1030181, 80.0, 105.0, typeof( Leather ), 1044462, 8, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 616, typeof( LeatherDo ), 1015293, 1030182, 75.0, 100.0, typeof( Leather ), 1044462, 12, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 617, typeof( LeatherHiroSode ), 1015293, 1030185, 55.0, 80.0, typeof( Leather ), 1044462, 5, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 618, typeof( LeatherSuneate ), 1015293, 1030193, 68.0, 93.0, typeof( Leather ), 1044462, 12, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 619, typeof( LeatherHaidate ), 1015293, 1030197, 68.0, 93.0, typeof( Leather ), 1044462, 12, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 620, typeof( LeatherNinjaPants ), 1015293, 1030204, 80.0, 105.0, typeof( Leather ), 1044462, 13, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 621, typeof( LeatherNinjaJacket ), 1015293, 1030206, 85.0, 110.0, typeof( Leather ), 1044462, 13, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 622, typeof( LeatherNinjaBelt ), 1015293, 1030203, 50.0, 75.0, typeof( Leather ), 1044462, 5, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 623, typeof( LeatherNinjaMitts ), 1015293, 1030205, 65.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 624, typeof( LeatherNinjaHood ), 1015293, 1030201, 90.0, 115.0, typeof( Leather ), 1044462, 14, 1044463 );
			craft.RequiresSE = true;

			craft = AddCraft( 647, typeof( LeafTunic ), 1015293, 1032229, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 648, typeof( LeafArms ), 1015293, 1032232, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 649, typeof( LeafGloves ), 1015293, 1032230, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 650, typeof( LeafLeggings ), 1015293, 1032233, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 651, typeof( LeafGorget ), 1015293, 1032231, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 652, typeof( LeafTonlet ), 1015293, 1032234, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;

			craft = AddCraft( 658, typeof( FemaleGargishLeatherArms ), 1015293, 1095327, 53.9, 103.9, typeof( Leather ), 1044462, 8, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 659, typeof( FemaleGargishLeatherChest ), 1015293, 1095329, 70.5, 120.5, typeof( Leather ), 1044462, 8, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 660, typeof( FemaleGargishLeatherLeggings ), 1015293, 1095333, 66.3, 116.3, typeof( Leather ), 1044462, 10, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 661, typeof( FemaleGargishLeatherKilt ), 1015293, 1095331, 58.0, 108.0, typeof( Leather ), 1044462, 6, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 662, typeof( GargishLeatherArms ), 1015293, 1095328, 53.9, 103.9, typeof( Leather ), 1044462, 8, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 663, typeof( GargishLeatherChest ), 1015293, 1095330, 70.5, 120.5, typeof( Leather ), 1044462, 8, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 664, typeof( GargishLeatherLeggings ), 1015293, 1095334, 66.3, 116.3, typeof( Leather ), 1044462, 10, 1044463 );
			craft.ChanceAtMin = 0.0;
			craft = AddCraft( 665, typeof( GargishLeatherKilt ), 1015293, 1095332, 58.0, 108.0, typeof( Leather ), 1044462, 6, 1044463 );
			craft.ChanceAtMin = 0.0;
			AddCraft( 666, typeof( GargishLeatherWingArmor ), 1015293, 1115389, 65.0, 90.0, typeof( Leather ), 1044462, 12, 1044463 );
			#endregion

			#region Cloth Armor
			AddCraft( 200, typeof( FemaleGargishClothArms ), 1111748, 1095351, 87.1, 137.1, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 201, typeof( FemaleGargishClothChest ), 1111748, 1095353, 94.0, 144.0, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 202, typeof( FemaleGargishClothLeggings ), 1111748, 1095357, 91.2, 141.2, typeof( Cloth ), 1044455, 10, 1044287 );
			AddCraft( 203, typeof( FemaleGargishClothKilt ), 1111748, 1095355, 82.9, 132.9, typeof( Cloth ), 1044455, 6, 1044287 );
			AddCraft( 204, typeof( GargishClothArms ), 1111748, 1095352, 87.1, 137.1, typeof( Cloth ), 1044455, 8, 1044287 );
			AddCraft( 205, typeof( GargishClothChest ), 1111748, 1095354, 94.0, 144.0, typeof( Cloth ), 1044455, 12, 1044287 );
			AddCraft( 206, typeof( GargishClothLeggings ), 1111748, 1095358, 91.2, 141.2, typeof( Cloth ), 1044455, 10, 1044287 );
			AddCraft( 207, typeof( GargishClothKilt ), 1111748, 1095356, 82.9, 132.9, typeof( Cloth ), 1044455, 6, 1044287 );
			#endregion

			#region Studded Armor
			AddCraft( 625, typeof( StuddedGorget ), 1015300, 1025078, 78.8, 103.8, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( 626, typeof( StuddedGloves ), 1015300, 1025077, 82.9, 107.9, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( 627, typeof( StuddedArms ), 1015300, 1025076, 87.1, 112.1, typeof( Leather ), 1044462, 10, 1044463 );
			AddCraft( 628, typeof( StuddedLegs ), 1015300, 1025082, 91.2, 116.2, typeof( Leather ), 1044462, 12, 1044463 );
			AddCraft( 629, typeof( StuddedChest ), 1015300, 1025083, 94.0, 119.0, typeof( Leather ), 1044462, 14, 1044463 );

			craft = AddCraft( 630, typeof( StuddedMempo ), 1015300, 1030216, 80.0, 105.0, typeof( Leather ), 1044462, 8, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 631, typeof( StuddedDo ), 1015300, 1030183, 95.0, 120.0, typeof( Leather ), 1044462, 14, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 632, typeof( StuddedHiroSode ), 1015300, 1030186, 85.0, 110.0, typeof( Leather ), 1044462, 8, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 633, typeof( StuddedSuneate ), 1015300, 1030194, 92.0, 117.0, typeof( Leather ), 1044462, 14, 1044463 );
			craft.RequiresSE = true;
			craft = AddCraft( 634, typeof( StuddedHaidate ), 1015300, 1030198, 92.0, 117.0, typeof( Leather ), 1044462, 14, 1044463 );
			craft.RequiresSE = true;

			craft = AddCraft( 653, typeof( HidePauldrons ), 1015300, 1031127, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 654, typeof( HideGloves ), 1015300, 1031125, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 655, typeof( HideGorget ), 1015300, 1031126, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 656, typeof( HidePants ), 1015300, 1031128, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			craft = AddCraft( 657, typeof( HideTunic ), 1015300, 1031124, 80.0, 105.0, typeof( Leather ), 1044462, 15, 1044463 );
			craft.RequiresML = true;
			#endregion

			#region Female Armor
			AddCraft( 635, typeof( LeatherShorts ), 1015306, 1027168, 62.2, 87.2, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( 636, typeof( LeatherSkirt ), 1015306, 1027176, 58.0, 83.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( 637, typeof( LeatherBustierArms ), 1015306, 1027178, 58.0, 83.0, typeof( Leather ), 1044462, 6, 1044463 );
			AddCraft( 638, typeof( StuddedBustierArms ), 1015306, 1027180, 82.9, 107.9, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( 639, typeof( FemaleLeatherChest ), 1015306, 1027174, 62.2, 87.2, typeof( Leather ), 1044462, 8, 1044463 );
			AddCraft( 640, typeof( FemaleStuddedChest ), 1015306, 1027170, 87.1, 112.1, typeof( Leather ), 1044462, 10, 1044463 );
			#endregion

			#region Bone Armor
			craft = AddCraft( 641, typeof( BoneHelm ), 1049149, 1025206, 85.0, 110.0, typeof( Leather ), 1044462, 4, 1044463 );
			craft.AddRes( typeof( Bone ), 1049064, 2, 1049063 );
			craft = AddCraft( 642, typeof( BoneGloves ), 1049149, 1025205, 89.0, 114.0, typeof( Leather ), 1044462, 6, 1044463 );
			craft.AddRes( typeof( Bone ), 1049064, 2, 1049063 );
			craft = AddCraft( 643, typeof( BoneArms ), 1049149, 1025203, 92.0, 117.0, typeof( Leather ), 1044462, 8, 1044463 );
			craft.AddRes( typeof( Bone ), 1049064, 4, 1049063 );
			craft = AddCraft( 644, typeof( BoneLegs ), 1049149, 1025202, 95.0, 120.0, typeof( Leather ), 1044462, 10, 1044463 );
			craft.AddRes( typeof( Bone ), 1049064, 6, 1049063 );
			craft = AddCraft( 645, typeof( BoneChest ), 1049149, 1025199, 96.0, 121.0, typeof( Leather ), 1044462, 12, 1044463 );
			craft.AddRes( typeof( Bone ), 1049064, 10, 1049063 );
			#endregion

			// Set the overidable material
			SetSubRes( typeof( Leather ), 1049150 );

			// Add every material you want the player to be able to chose from
			// This will overide the overidable material
			AddSubRes( typeof( Leather ), 1049150, 00.0, 1044462, 1049311 );
			AddSubRes( typeof( SpinedLeather ), 1049151, 65.0, 1044462, 1049311 );
			AddSubRes( typeof( HornedLeather ), 1049152, 80.0, 1044462, 1049311 );
			AddSubRes( typeof( BarbedLeather ), 1049153, 99.0, 1044462, 1049311 );

			MarkOption = true;
			Repair = true;
			CanEnhance = true;
			Alter = true;
		}
	}
}