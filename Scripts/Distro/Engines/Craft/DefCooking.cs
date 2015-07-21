using System;
using Server.Items;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum CookingRecipe
	{
		GingerbreadCookie = 93,
		BowlOfRotwormStew = 94
	}
	#endregion

	public class DefCooking : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefCooking();
		}

		public override SkillName MainSkill { get { return SkillName.Cooking; } }

		public override int GumpTitleNumber
		{
			get { return 1044003; } // <CENTER>COOKING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		public override CraftECA ECA { get { return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double DefaultChanceAtMin
		{
			get { return 0.0; }
		}

		private DefCooking()
			: base( 1, 1, 1.25 ) // base( 1, 1, 1.5 )
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

			#region Ingredients
			craft = AddCraft( 1, typeof( SackFlour ), 1044495, 1024153, 0.0, 100.0, typeof( WheatSheaf ), 1044489, 1, 1044490 );

			craft = AddCraft( 2, typeof( Dough ), 1044495, 1024157, 0.0, 100.0, typeof( SackFlour ), 1044468, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );

			craft = AddCraft( 3, typeof( SweetDough ), 1044495, 1041340, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( JarHoney ), 1044472, 1, 1044253 );

			craft = AddCraft( 4, typeof( CakeMix ), 1044495, 1041002, 0.0, 100.0, typeof( SackFlour ), 1044468, 1, 1044253 );
			craft.AddRes( typeof( SweetDough ), 1044475, 1, 1044253 );

			craft = AddCraft( 5, typeof( CookieMix ), 1044495, 1024159, 0.0, 100.0, typeof( JarHoney ), 1044472, 1, 1044253 );
			craft.AddRes( typeof( SweetDough ), 1044475, 1, 1044253 );

			craft = AddCraft( 51, typeof( CocoaButter ), 1044495, 1079998, 0.0, 100.0, typeof( CocoaPulp ), 1080530, 1, 1044253 );
			craft.NeedHeat = true;
			craft.RequiresML = true;

			craft = AddCraft( 52, typeof( CocoaLiquor ), 1044495, 1079999, 0.0, 100.0, typeof( CocoaPulp ), 1080530, 1, 1044253 );
			craft.AddRes( typeof( PewterBowl ), 1025629, 1, 1044253 );
			craft.NeedHeat = true;
			craft.RequiresML = true;

			#endregion

			#region Preparations
			craft = AddCraft( 6, typeof( UnbakedQuiche ), 1044496, 1041339, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Eggs ), 1044477, 1, 1044253 );

			// TODO: This must also support chicken and lamb legs
			craft = AddCraft( 7, typeof( UnbakedMeatPie ), 1044496, 1041338, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( RawRibs ), 1044482, 1, 1044253 );

			craft = AddCraft( 8, typeof( UncookedSausagePizza ), 1044496, 1041337, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Sausage ), 1044483, 1, 1044253 );

			craft = AddCraft( 9, typeof( UncookedCheesePizza ), 1044496, 1041341, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( CheeseWheel ), 1044486, 1, 1044253 );

			craft = AddCraft( 10, typeof( UnbakedFruitPie ), 1044496, 1041334, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Pear ), 1044481, 1, 1044253 );

			craft = AddCraft( 11, typeof( UnbakedPeachCobbler ), 1044496, 1041335, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Peach ), 1044480, 1, 1044253 );

			craft = AddCraft( 12, typeof( UnbakedApplePie ), 1044496, 1041336, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Apple ), 1044479, 1, 1044253 );

			craft = AddCraft( 13, typeof( UnbakedPumpkinPie ), 1044496, 1041342, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( Pumpkin ), 1044484, 1, 1044253 );

			craft = AddCraft( 14, typeof( GreenTea ), 1044496, 1030315, 80.0, 130.0, typeof( GreenTeaBasket ), 1030316, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.RequiresSE = true;
			craft.NeedOven = true;

			craft = AddCraft( 15, typeof( WasabiClumps ), 1044496, 1029451, 70.0, 120.0, typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.AddRes( typeof( BowlOfPeas ), 1025633, 3, 1044253 );
			craft.RequiresSE = true;

			craft = AddCraft( 16, typeof( SushiRolls ), 1044496, 1030303, 90.0, 120.0, typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.AddRes( typeof( RawFishSteak ), 1044476, 10, 1044253 );
			craft.RequiresSE = true;

			craft = AddCraft( 17, typeof( SushiPlatter ), 1044496, 1030305, 90.0, 120.0, typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.AddRes( typeof( RawFishSteak ), 1044476, 10, 1044253 );
			craft.RequiresSE = true;

			craft = AddCraft( 18, typeof( TribalPaint ), 1044496, 1040000, 80.0, 80.0, typeof( SackFlour ), 1044468, 1, 1044253 );
			craft.AddRes( typeof( TribalBerry ), 1046460, 1, 1044253 );

			craft = AddCraft( 19, typeof( EggBomb ), 1044496, 1030249, 90.0, 120.0, typeof( Eggs ), 1044477, 1, 1044253 );
			craft.AddRes( typeof( SackFlour ), 1044468, 3, 1044253 );
			craft.RequiresSE = true;

			craft = AddCraft( 46, typeof( ParrotWafers ), 1044496, 1032246, 37.5, 77.5, typeof( Dough ), 1044469, 1, 1044253 );
			craft.AddRes( typeof( JarHoney ), 1044472, 1, 1044253 );
			craft.AddRes( typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 54, typeof( PlantPigment ), 1044496, 1112132, 75.0, 100.0, typeof( PlantClippings ), 1112131, 1, 1044253 );
			craft.AddRes( typeof( Bottle ), 1044250, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 55, typeof( NaturalDye ), 1044496, 1112136, 75.0, 100.0, typeof( PlantPigment ), 1112132, 1, 1044253 );
			craft.AddRes( typeof( ColorFixative ), 1112135, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 56, typeof( ColorFixative ), 1044496, 1112135, 75.0, 100.0, typeof( SilverSerpentVenom ), 1112173, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1022503, 1, 1044253 );
			craft.BeverageType = BeverageType.Wine;
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 57, typeof( WoodPulp ), 1044496, 1113136, 60.0, 85.0, typeof( BarkFragment ), 1032687, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.NeedHeat = true;
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;
			#endregion

			#region Baking
			craft = AddCraft( 20, typeof( BreadLoaf ), 1044497, 1024156, 0.0, 100.0, typeof( Dough ), 1044469, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 21, typeof( Cookies ), 1044497, 1025643, 0.0, 100.0, typeof( CookieMix ), 1044474, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 22, typeof( Cake ), 1044497, 1022537, 0.0, 100.0, typeof( CakeMix ), 1044471, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 23, typeof( Muffins ), 1044497, 1022539, 0.0, 100.0, typeof( SweetDough ), 1044475, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 24, typeof( Quiche ), 1044497, 1041345, 0.0, 100.0, typeof( UnbakedQuiche ), 1044518, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 25, typeof( MeatPie ), 1044497, 1041347, 0.0, 100.0, typeof( UnbakedMeatPie ), 1044519, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 26, typeof( SausagePizza ), 1044497, 1044517, 0.0, 100.0, typeof( UncookedSausagePizza ), 1044520, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 27, typeof( CheesePizza ), 1044497, 1044516, 0.0, 100.0, typeof( UncookedCheesePizza ), 1044521, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 28, typeof( FruitPie ), 1044497, 1041346, 0.0, 100.0, typeof( UnbakedFruitPie ), 1044522, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 29, typeof( PeachCobbler ), 1044497, 1041344, 0.0, 100.0, typeof( UnbakedPeachCobbler ), 1044523, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 30, typeof( ApplePie ), 1044497, 1041343, 0.0, 100.0, typeof( UnbakedApplePie ), 1044524, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 31, typeof( PumpkinPie ), 1044497, 1041348, 0.0, 100.0, typeof( UnbakedPumpkinPie ), 1046461, 1, 1044253 );
			craft.NeedOven = true;

			craft = AddCraft( 32, typeof( MisoSoup ), 1044497, 1030317, 60.0, 110.0, typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.RequiresSE = true;
			craft.NeedOven = true;

			craft = AddCraft( 33, typeof( WhiteMisoSoup ), 1044497, 1030318, 60.0, 110.0, typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.RequiresSE = true;
			craft.NeedOven = true;

			craft = AddCraft( 34, typeof( RedMisoSoup ), 1044497, 1030319, 60.0, 110.0, typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.RequiresSE = true;
			craft.NeedOven = true;

			craft = AddCraft( 35, typeof( AwaseMisoSoup ), 1044497, 1030320, 60.0, 110.0, typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
			craft.RequiresSE = true;
			craft.NeedOven = true;

			craft = AddCraft( 47, typeof( GingerbreadCookie ), 1044497, 1077414, 35.0, 85.0, typeof( CookieMix ), 1044474, 1, 1044253 );
			craft.AddRes( typeof( FreshGinger ), 1031235, 1, 1077413 );
			craft.RequiresML = true;
			craft.NeedOven = true;
			craft.AddRecipe( (int) CookingRecipe.GingerbreadCookie, this );
			#endregion

			#region Barbecue
			craft = AddCraft( 36, typeof( CookedBird ), 1044498, 1022487, 0.0, 100.0, typeof( RawBird ), 1044470, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 37, typeof( ChickenLeg ), 1044498, 1025640, 0.0, 100.0, typeof( RawChickenLeg ), 1044473, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 38, typeof( FishSteak ), 1044498, 1022427, 0.0, 100.0, typeof( RawFishSteak ), 1044476, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 39, typeof( FriedEggs ), 1044498, 1022486, 0.0, 100.0, typeof( Eggs ), 1044477, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 40, typeof( LambLeg ), 1044498, 1025642, 0.0, 100.0, typeof( RawLambLeg ), 1044478, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 41, typeof( Ribs ), 1044498, 1022546, 0.0, 100.0, typeof( RawRibs ), 1044485, 1, 1044253 );
			craft.NeedHeat = true;
			craft.UseAllRes = true;

			craft = AddCraft( 53, typeof( BowlOfRotwormStew ), 1044498, 1031706, 0.0, 100.0, typeof( RawRotwormMeat ), 1031705, 1, 1044253 );
			craft.RequiresSA = true;
			craft.AddRecipe( (int) CookingRecipe.BowlOfRotwormStew, this );
			#endregion

			#region Enchanted
			craft = AddCraft( 42, typeof( FoodDecorationTool ), 1073108, 1072951, 75.0, 100.0, typeof( Dough ), 1024157, 1, 1044253 );
			craft.AddRes( typeof( JarHoney ), 1044472, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 43, typeof( EnchantedApple ), 1073108, 1032248, 60.0, 85.0, typeof( Apple ), 1022512, 1, 1044253 );
			craft.AddRes( typeof( GreaterHealPotion ), 1073467, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresML = true;

			craft = AddCraft( 44, typeof( GrapesOfWrath ), 1073108, 1072953, 95.0, 120.0, typeof( Grapes ), 1073468, 1, 1044253 );
			craft.AddRes( typeof( GreaterStrengthPotion ), 1073466, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresML = true;

			craft = AddCraft( 45, typeof( FruitBowl ), 1073108, 1072950, 55.0, 105.0, typeof( WoodenBowl ), 1073472, 1, 1044253 );
			craft.AddRes( typeof( Pear ), 1044481, 3, 1044253 );
			craft.AddRes( typeof( Apple ), 1044479, 3, 1044253 );
			craft.AddRes( typeof( Banana ), 1073470, 3, 1044253 );
			craft.RequiresML = true;
			#endregion

			#region Chocolatiering

			craft = AddCraft( 48, typeof( DarkChocolate ), 1080001, 1079994, 15.0, 100.0, typeof( SackOfSugar ), 1079997, 1, 1080002 );
			craft.AddRes( typeof( CocoaButter ), 1079998, 1, 1080004 );
			craft.AddRes( typeof( CocoaLiquor ), 1079999, 1, 1080006 );
			craft.RequiresML = true;

			craft = AddCraft( 49, typeof( MilkChocolate ), 1080001, 1079995, 32.5, 100.0, typeof( SackOfSugar ), 1079997, 1, 1080002 );
			craft.AddRes( typeof( CocoaButter ), 1079998, 1, 1080004 );
			craft.AddRes( typeof( CocoaLiquor ), 1079999, 1, 1080006 );
			craft.AddRes( typeof( BaseBeverage ), 1048131, 1, 1044253 );
			craft.BeverageType = BeverageType.Milk;
			craft.RequiresML = true;

			craft = AddCraft( 50, typeof( WhiteChocolate ), 1080001, 1079996, 52.5, 100.0, typeof( SackOfSugar ), 1079997, 1, 1080002 );
			craft.AddRes( typeof( CocoaButter ), 1079998, 1, 1080004 );
			craft.AddRes( typeof( Vanilla ), 1080000, 1, 1080008 );
			craft.AddRes( typeof( BaseBeverage ), 1048131, 1, 1044253 );
			craft.BeverageType = BeverageType.Milk;
			craft.RequiresML = true;

			#endregion
		}
	}
}