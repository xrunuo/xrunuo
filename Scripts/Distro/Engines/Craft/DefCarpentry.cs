using System;
using Server.Items;
using Server.Spells.Second;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum CarpRecipe
	{
		WarriorStatueSouth = 88,
		WarriorStatueEast = 87,
		SquirrelStatueSouth = 86,
		SquirrelStatueEast = 85,
		AcidProofRope = 76,
		OrnateElvenChair = 77,
		ArcaneBookshelfSouth = 79,
		ArcaneBookshelfEast = 78,
		OrnateElvenChestSouth = 82,
		ElvenDresserSouth = 109,
		ElvenDresserEast = 110,
		FancyElvenArmoire = 80,
		ArcanistsWildStaff = 58,
		AncientWildStaff = 61,
		ThornedWildStaff = 60,
		HardenedWildStaff = 59,
		TallElvenBedSouth = 84,
		TallElvenBedEast = 83,
		StoneAnvilSouth = 118,
		StoneAnvilEast = 119,
		OrnateElvenChestEast = 81
	}

	public enum CarpRecipeGreater
	{
		PhantomStaff = 57,
		IronwoodCrown = 56,
		BrambleCoat = 55
	}
	#endregion

	public class DefCarpentry : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefCarpentry();
		}

		public override SkillName MainSkill { get { return SkillName.Carpentry; } }

		public override int GumpTitleNumber
		{
			get { return 1044004; } // <CENTER>CARPENTRY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		public override double DefaultChanceAtMin
		{
			get { return 0.5; }
		}

		private DefCarpentry()
			: base( 1, 1, 1.25 )
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
			from.PlaySound( 0x23D );
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

			#region Other
			//craft = AddCraft( 67, typeof( Board ), 1044294, 1027127, 0.0, 0.0, typeof( Log ), 1044466, 1, 1044465 );
			//craft.UseAllRes = true;

			AddCraft( 68, typeof( BarrelStaves ), 1044294, 1027857, 00.0, 25.0, typeof( Log ), 1044041, 5, 1044351 );
			AddCraft( 69, typeof( BarrelLid ), 1044294, 1027608, 11.0, 36.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( 70, typeof( ShortMusicStand ), 1044294, 1044313, 78.9, 103.9, typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( 71, typeof( ShortMusicStandRight ), 1044294, 1044314, 78.9, 103.9, typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( 72, typeof( TallMusicStand ), 1044294, 1044315, 81.5, 106.5, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( 73, typeof( TallMusicStandRight ), 1044294, 1044316, 81.5, 106.5, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( 74, typeof( Easle ), 1044294, 1044317, 86.8, 111.8, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( 75, typeof( EasleEast ), 1044294, 1044318, 86.8, 111.8, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( 76, typeof( EasleNorth ), 1044294, 1044319, 86.8, 111.8, typeof( Log ), 1044041, 20, 1044351 );

			craft = AddCraft( 77, typeof( RedHangingLantern ), 1044294, 1029412, 65.0, 90.0, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddRes( typeof( BlankScroll ), 1044377, 10, 1044378 );
			craft.RequiresSE = true;

			craft = AddCraft( 78, typeof( WhiteHangingLantern ), 1044294, 1029416, 65.0, 90.0, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddRes( typeof( BlankScroll ), 1044377, 10, 1044378 );
			craft.RequiresSE = true;

			craft = AddCraft( 79, typeof( ShojiScreen ), 1044294, 1029423, 80.0, 105.0, typeof( Log ), 1044041, 75, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 80, typeof( BambooScreen ), 1044294, 1029428, 80.0, 105.0, typeof( Log ), 1044041, 75, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );
			craft.RequiresSE = true;

			craft = AddCraft( 81, typeof( FishingPole ), 1044294, 1023519, 68.4, 93.4, typeof( Log ), 1044041, 5, 1044351 ); //This is in the categor of Other during AoS
			craft.AddSkill( SkillName.Tailoring, 40.0, 45.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 5, 1044287 );

			craft = AddCraft( 136, typeof( WoodenContainerEngravingTool ), 1044294, 1072153, 75.0, 100.0, typeof( Log ), 1044041, 4, 1044351 );
			craft.AddRes( typeof( IronIngot ), 1027151, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 138, typeof( RunedSwitch ), 1044294, 1032129, 70.0, 95.0, typeof( RunedPrism ), 1044041, 1, 1044253 );
			craft.AddRes( typeof( JeweledFiligree ), 1032126, 1, 1044253 );
			craft.AddRes( typeof( EnchantedSwitch ), 1032124, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 226, typeof( ArcanistStatueSoutDeed ), 1044294, 1072885, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 227, typeof( ArcanistStatueEastDeed ), 1044294, 1072886, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 228, typeof( WarriorStatueSoutDeed ), 1044294, 1072887, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.WarriorStatueSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 229, typeof( WarriorStatueEastDeed ), 1044294, 1072888, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.WarriorStatueEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 230, typeof( SquirrelStatueSoutDeed ), 1044294, 1072884, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.SquirrelStatueSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 231, typeof( SquirrelStatueEastDeed ), 1044294, 1073398, 0.0, 25.0, typeof( Log ), 1044041, 250, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.SquirrelStatueEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 306, typeof( GiantReplicaAcorn ), 1044294, 1072889, 80.0, 105.0, typeof( Log ), 1044041, 35, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 310, typeof( MountedDreadHorn ), 1044294, 1074464, 90.0, 115.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.AddRes( typeof( PristineDreadHorn ), 1032634, 1, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 311, typeof( AcidProofRope ), 1044294, 1074886, 80.0, 105.0, typeof( GreaterStrengthPotion ), 1044547, 2, 1044253 );
			craft.AddRes( typeof( SwitchItem ), 1024241, 2, 1044253 );
			craft.AddRes( typeof( ProtectionScroll ), 1044395, 1, 1044253 );
			craft.AddRecipe( (int) CarpRecipe.AcidProofRope, this );
			craft.RequiresML = true;

			craft = AddCraft( 314, typeof( GargishBanner ), 1044294, 1095311, 94.7, 119.7, typeof( Log ), 1044547, 50 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 75.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 50, 1044287 );
			craft.RequiresSA = true;

			// TODO (SA):
			// 519	1112479	an incubator
			// 520	1112570	a chicken coop
			#endregion

			#region Furniture
			AddCraft( 82, typeof( FootStool ), 1044291, 1022910, 11.0, 36.0, typeof( Log ), 1044041, 9, 1044351 );
			AddCraft( 83, typeof( Stool ), 1044291, 1022602, 11.0, 36.0, typeof( Log ), 1044041, 9, 1044351 );
			AddCraft( 84, typeof( BambooChair ), 1044291, 1044300, 21.0, 46.0, typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( 85, typeof( WoodenChair ), 1044291, 1044301, 21.0, 46.0, typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( 86, typeof( FancyWoodenChairCushion ), 1044291, 1044302, 42.1, 67.1, typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( 87, typeof( WoodenChairCushion ), 1044291, 1044303, 42.1, 67.1, typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( 88, typeof( WoodenBench ), 1044291, 1022860, 52.6, 77.6, typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( 89, typeof( WoodenThrone ), 1044291, 1044304, 52.6, 77.6, typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( 90, typeof( Throne ), 1044291, 1044305, 73.6, 98.6, typeof( Log ), 1044041, 19, 1044351 );
			AddCraft( 91, typeof( Nightstand ), 1044291, 1044306, 42.1, 67.1, typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( 92, typeof( WritingTable ), 1044291, 1022890, 63.1, 88.1, typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( 93, typeof( LargeTable ), 1044291, 1044308, 84.2, 109.2, typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( 94, typeof( YewWoodTable ), 1044291, 1044307, 63.1, 88.1, typeof( Log ), 1044041, 23, 1044351 );

			craft = AddCraft( 95, typeof( ElegantLowTable ), 1044291, 1030265, 80.0, 105.0, typeof( Log ), 1044041, 35, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 96, typeof( PlainLowTable ), 1044291, 1030266, 80.0, 105.0, typeof( Log ), 1044041, 35, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 209, typeof( OrnateTableSouthDeed ), 1044291, 1072869, 85.0, 110.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 210, typeof( OrnateTableEastDeed ), 1044291, 1073384, 85.0, 110.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 211, typeof( HardwoodTableSouthDeed ), 1044291, 1073385, 80.0, 105.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 212, typeof( HardwoodTableEastDeed ), 1044291, 1073386, 80.0, 105.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 300, typeof( ElvenPodium ), 1044291, 1031741, 80.0, 105.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 304, typeof( OrnateElvenChair ), 1044291, 1072870, 80.0, 105.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.OrnateElvenChair, this );
			craft.RequiresML = true;

			craft = AddCraft( 305, typeof( CozyElvenChair ), 1044291, 1031755, 85.0, 110.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 307, typeof( ReadingChair ), 1044291, 1072160, 85.0, 110.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 315, typeof( TerMurStyleChair ), 1044291, 1095291, 85.0, 110.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 316, typeof( TerMurStyleTable ), 1044291, 1095321, 75.0, 100.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresSA = true;
			#endregion

			#region Containers
			AddCraft( 97, typeof( WoodenBox ), 1044292, 1023709, 21.0, 46.0, typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( 98, typeof( SmallCrate ), 1044292, 1044309, 10.0, 35.0, typeof( Log ), 1044041, 8, 1044351 );
			AddCraft( 99, typeof( MediumCrate ), 1044292, 1044310, 31.0, 56.0, typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( 100, typeof( LargeCrate ), 1044292, 1044311, 47.3, 72.3, typeof( Log ), 1044041, 18, 1044351 );
			AddCraft( 101, typeof( WoodenChest ), 1044292, 1023650, 73.6, 98.6, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( 102, typeof( EmptyBookcase ), 1044292, 1022718, 31.5, 56.5, typeof( Log ), 1044041, 25, 1044351 );
			AddCraft( 103, typeof( FancyArmoire ), 1044292, 1044312, 84.2, 109.2, typeof( Log ), 1044041, 35, 1044351 );
			AddCraft( 104, typeof( Armoire ), 1044292, 1022643, 84.2, 109.2, typeof( Log ), 1044041, 35, 1044351 );

			craft = AddCraft( 105, typeof( PlainWoodenChest ), 1044292, 1030251, 90.0, 115.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 106, typeof( OrnateWoodenChest ), 1044292, 1030253, 90.0, 115.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 107, typeof( GildedWoodenChest ), 1044292, 1030255, 90.0, 115.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 108, typeof( WoodenFootLocker ), 1044292, 1030257, 90.0, 115.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 109, typeof( FinishedWoodenChest ), 1044292, 1030259, 90.0, 115.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 110, typeof( TallCabinet ), 1044292, 1030261, 90.0, 115.0, typeof( Log ), 1044041, 35, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 111, typeof( ShortCabinet ), 1044292, 1030263, 90.0, 115.0, typeof( Log ), 1044041, 35, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 112, typeof( RedArmoire ), 1044292, 1030328, 90.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 113, typeof( ElegantArmoire ), 1044292, 1030330, 90.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 114, typeof( MapleArmoire ), 1044292, 1030328, 90.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 115, typeof( CherryArmoire ), 1044292, 1030328, 90.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 116, typeof( Keg ), 1044292, 1023711, 57.8, 82.8, typeof( BarrelStaves ), 1044288, 3, 1044253 );
			craft.AddRes( typeof( BarrelHoops ), 1044289, 1, 1044253 );
			craft.AddRes( typeof( BarrelLid ), 1044251, 1, 1044253 );

			craft = AddCraft( 205, typeof( ArcaneBookshelfSouthDeed ), 1044292, 1073371, 94.7, 119.7, typeof( Log ), 1044041, 80, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.ArcaneBookshelfSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 206, typeof( ArcaneBookshelfEastDeed ), 1044292, 1072871, 94.7, 119.7, typeof( Log ), 1044041, 80, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.ArcaneBookshelfEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 213, typeof( OrnateElvenChestSouthDeed ), 1044292, 1073383, 94.7, 119.7, typeof( Log ), 1044041, 40, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.OrnateElvenChestSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 214, typeof( OrnateElvenChestEastDeed ), 1044292, 1072862, 94.7, 119.7, typeof( Log ), 1044041, 40, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.OrnateElvenChestEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 215, typeof( ElvenWashBasinSouthDeed ), 1044292, 1072865, 70.0, 95.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 216, typeof( ElvenWashBasinEastDeed ), 1044292, 1073387, 70.0, 95.0, typeof( Log ), 1044041, 40, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 217, typeof( ElvenDresserSouthDeed ), 1044292, 1072864, 75.0, 100.0, typeof( Log ), 1044041, 45, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.ElvenDresserSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 218, typeof( ElvenDresserEastDeed ), 1044292, 1073388, 75.0, 100.0, typeof( Log ), 1044041, 45, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.ElvenDresserEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 302, typeof( FancyElvenArmoire ), 1044292, 1072866, 80.0, 105.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.AddRecipe( (int) CarpRecipe.FancyElvenArmoire, this );
			craft.RequiresML = true;

			craft = AddCraft( 303, typeof( SimpleElvenArmoire ), 1044292, 1073401, 80.0, 105.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 308, typeof( RarewoodChest ), 1044292, 1031761, 80.0, 105.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 309, typeof( DecorativeBox ), 1044292, 1031763, 80.0, 105.0, typeof( Log ), 1044041, 25, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 317, typeof( GargishChest ), 1044292, 1095293, 80.0, 105.0, typeof( Log ), 1044041, 30, 1044351 );
			craft.RequiresSA = true;
			#endregion

			#region Weapons
			AddCraft( 117, typeof( ShepherdsCrook ), 1044566, 1023713, 78.9, 103.9, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( 118, typeof( QuarterStaff ), 1044566, 1023721, 73.6, 98.6, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( 119, typeof( GnarledStaff ), 1044566, 1025112, 78.9, 103.9, typeof( Log ), 1044041, 7, 1044351 );

			craft = AddCraft( 121, typeof( Bokuto ), 1044566, 1030227, 70.0, 95.0, typeof( Log ), 1044041, 6, 1044351 );
			craft.RequiresSE = true;
			craft = AddCraft( 122, typeof( Fukiya ), 1044566, 1030229, 60.0, 85.0, typeof( Log ), 1044041, 6, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 123, typeof( Tetsubo ), 1044566, 1030225, 85.0, 110.0, typeof( Log ), 1044041, 8, 1044351 );
			craft.AddSkill( SkillName.Tinkering, 40.0, 45.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.RequiresSE = true;

			craft = AddCraft( 135, typeof( WildStaff ), 1044566, 1031557, 63.8, 88.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 137, typeof( PhantomStaff ), 1044566, 1072919, 90.0, 130.0, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( DiseasedBark ), 1032683, 1, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.ChanceAtMin = 0.2;
			craft.ForceNonExceptional = true;
			craft.AddRecipe( (int) CarpRecipeGreater.PhantomStaff, this );
			craft.RequiresML = true;

			craft = AddCraft( 157, typeof( ArcanistsWildStaff ), 1044566, 1073549, 63.8, 88.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.AddRecipe( (int) CarpRecipe.ArcanistsWildStaff, this );
			craft.RequiresML = true;

			craft = AddCraft( 158, typeof( AncientWildStaff ), 1044566, 1073550, 63.8, 88.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) CarpRecipe.AncientWildStaff, this );
			craft.RequiresML = true;

			craft = AddCraft( 159, typeof( ThornedWildStaff ), 1044566, 1073551, 63.8, 88.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( FireRuby ), 1026254, 1, 1044253 );
			craft.AddRecipe( (int) CarpRecipe.ThornedWildStaff, this );
			craft.RequiresML = true;

			craft = AddCraft( 160, typeof( HardenedWildStaff ), 1044566, 1073552, 63.8, 88.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) CarpRecipe.HardenedWildStaff, this );
			craft.RequiresML = true;

			craft = AddCraft( 161, typeof( SerpentstoneStaff ), 1044566, 1095367, 63.8, 113.8, typeof( Log ), 1044041, 16, 1044351 );
			craft.AddRes( typeof( EcruCitrine ), 1032693, 1, 1044253 );
			craft.ChanceAtMin = 0.0;
			craft.RequiresSA = true;

			craft = AddCraft( 164, typeof( GargishGnarledStaff ), 1044566, 1097488, 78.9, 103.9, typeof( Log ), 1044041, 7, 1044351 );
			craft.RequiresSA = true;

			AddCraft( 312, typeof( Club ), 1044566, 1025043, 65.0, 90.0, typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( 906, typeof( BlackStaff ), 1044566, 1023568, 81.5, 106.5, typeof( Log ), 1044041, 9, 1044351 );

			#endregion

			#region Armor
			AddCraft( 120, typeof( WoodenShield ), 1062760, 1027034, 52.6, 77.6, typeof( Log ), 1044041, 9, 1044351 );

			craft = AddCraft( 140, typeof( WoodlandChest ), 1062760, 1031111, 85.0, 110.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 6, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 141, typeof( WoodlandArms ), 1062760, 1031116, 85.0, 110.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 142, typeof( WoodlandGauntlets ), 1062760, 1031114, 85.0, 110.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 143, typeof( WoodlandLeggings ), 1062760, 1031115, 85.0, 110.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 144, typeof( WoodlandGorget ), 1062760, 1031113, 85.0, 110.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 145, typeof( RavenHelm ), 1062760, 1031121, 65.0, 90.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.AddRes( typeof( Feather ), 1027121, 25, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 146, typeof( VultureHelm ), 1062760, 1031122, 63.9, 88.9, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.AddRes( typeof( Feather ), 1027121, 25, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 147, typeof( WingedHelm ), 1062760, 1031123, 58.4, 83.4, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( BarkFragment ), 1032687, 4, 1044253 );
			craft.AddRes( typeof( Feather ), 1027121, 60, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 148, typeof( IronwoodCrown ), 1062760, 1072924, 85.0, 120.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( DiseasedBark ), 1032683, 1, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.AddRecipe( (int) CarpRecipeGreater.IronwoodCrown, this );
			craft.RequiresML = true;

			craft = AddCraft( 149, typeof( BrambleCoat ), 1062760, 1072925, 85.0, 120.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( DiseasedBark ), 1032683, 1, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.AddRecipe( (int) CarpRecipeGreater.BrambleCoat, this );
			craft.RequiresML = true;

			craft = AddCraft( 150, typeof( DarkwoodCrown ), 1062760, 1073481, 85.0, 120.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( LardOfParoxysmus ), 1032681, 1, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 151, typeof( DarkwoodChest ), 1062760, 1073482, 85.0, 120.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( DreadHornMane ), 1032682, 1, 1044253 );
			craft.AddRes( typeof( Muculent ), 1072139, 10, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 152, typeof( DarkwoodGorget ), 1062760, 1073483, 85.0, 120.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRes( typeof( DiseasedBark ), 1032683, 1, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 153, typeof( DarkwoodLeggings ), 1062760, 1073484, 85.0, 120.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( GrizzledBones ), 1032684, 1, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 154, typeof( DarkwoodPauldrons ), 1062760, 1073485, 85.0, 120.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( EyeOfTheTravesty ), 1032685, 1, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 155, typeof( DarkwoodGauntlets ), 1062760, 1073486, 85.0, 120.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( CapturedEssence ), 1032686, 1, 1044253 );
			craft.AddRes( typeof( Muculent ), 1072139, 10, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.ChanceAtMin = 0.3;
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 163, typeof( GargishWoodenShield ), 1062760, 1095768, 52.6, 77.6, typeof( Log ), 1044041, 9, 1044351 );
			craft.RequiresSA = true;
			#endregion

			#region Instruments
			craft = AddCraft( 124, typeof( LapHarp ), 1044293, 1023762, 63.1, 88.1, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 125, typeof( Harp ), 1044293, 1023761, 78.9, 103.9, typeof( Log ), 1044041, 35, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 15, 1044287 );

			craft = AddCraft( 126, typeof( Drums ), 1044293, 1023740, 57.8, 82.8, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 127, typeof( Lute ), 1044293, 1023763, 68.4, 93.4, typeof( Log ), 1044041, 25, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 128, typeof( Tambourine ), 1044293, 1023741, 57.8, 82.8, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 129, typeof( TambourineTassel ), 1044293, 1044320, 57.8, 82.8, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 15, 1044287 );

			craft = AddCraft( 130, typeof( BambooFlute ), 1044293, 1030247, 80.0, 105.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.RequiresSE = true;

			craft = AddCraft( 162, typeof( AudChar ), 1044293, 1095315, 78.9, 103.9, typeof( Log ), 1044041, 35, 1044351 );
			craft.AddRes( typeof( Granite ), 1044514, 15, 1044513 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.RequiresSA = true;

			craft = AddCraft( 518, typeof( SnakeCharmerFlute ), 1044293, 1112174, 80.0, 105.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( LuminescentFungi ), 1032689, 3, 1044513 );
			craft.AddSkill( SkillName.Musicianship, 45.0, 50.0 );
			craft.RequiresSA = true;
			#endregion

			#region Misc Addons
			AddCraft( 131, typeof( PlayerBBEast ), 1044290, 1062420, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );
			AddCraft( 132, typeof( PlayerBBSouth ), 1044290, 1062421, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );

			craft = AddCraft( 156, typeof( Mobiles.ParrotPerchDeed ), 1044290, 1072617, 50.0, 75.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 200, typeof( ArcaneCircleDeed ), 1044290, 1072703, 94.7, 119.7, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 2, 1044253 );
			craft.AddRes( typeof( FireRuby ), 1026254, 2, 1044253 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 2, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 201, typeof( LargeElvenBedSouthDeed ), 1044290, 1072858, 94.7, 119.7, typeof( Log ), 1044041, 200, 1044351 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.AddRecipe( (int) CarpRecipe.TallElvenBedSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 202, typeof( LargeElvenBedEastDeed ), 1044290, 1072859, 94.7, 119.7, typeof( Log ), 1044041, 200, 1044351 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.AddRecipe( (int) CarpRecipe.TallElvenBedEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 203, typeof( SmallElvenBedSouthDeed ), 1044290, 1072860, 94.7, 119.7, typeof( Log ), 1044041, 70, 1044351 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresML = true;

			craft = AddCraft( 204, typeof( SmallElvenBedEastDeed ), 1044290, 1072861, 94.7, 119.7, typeof( Log ), 1044041, 70, 1044351 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresML = true;

			craft = AddCraft( 207, typeof( ElvenLoveseatSouthDeed ), 1044290, 1074898, 80.0, 105.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 208, typeof( ElvenLoveseatEastDeed ), 1044290, 1074897, 80.0, 105.0, typeof( Log ), 1044041, 50, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 224, typeof( AlchemistTableSouthDeed ), 1044290, 1073396, 85.0, 110.0, typeof( Log ), 1044041, 70, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 225, typeof( AlchemistTableEastDeed ), 1044290, 1073397, 85.0, 110.0, typeof( Log ), 1044041, 70, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 500, typeof( SmallBedSouthDeed ), 1044290, 1044321, 94.7, 113.1, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 80.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );

			craft = AddCraft( 501, typeof( SmallBedEastDeed ), 1044290, 1044322, 94.7, 113.1, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 80.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );

			craft = AddCraft( 502, typeof( LargeBedSouthDeed ), 1044290, 1044323, 94.7, 113.1, typeof( Log ), 1044041, 150, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 80.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 150, 1044287 );

			craft = AddCraft( 503, typeof( LargeBedEastDeed ), 1044290, 1044324, 94.7, 113.1, typeof( Log ), 1044041, 150, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 80.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 150, 1044287 );

			AddCraft( 504, typeof( DartBoardSouthDeed ), 1044290, 1044325, 15.7, 40.7, typeof( Log ), 1044041, 5, 1044351 );

			AddCraft( 505, typeof( DartBoardEastDeed ), 1044290, 1044326, 15.7, 40.7, typeof( Log ), 1044041, 5, 1044351 );

			AddCraft( 506, typeof( BallotBoxDeed ), 1044290, 1044327, 47.3, 72.3, typeof( Log ), 1044041, 5, 1044351 );

			craft = AddCraft( 507, typeof( PentagramDeed ), 1044290, 1044328, 100.0, 125.0, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Magery, 75.0, 80.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 40, 1044037 );

			craft = AddCraft( 508, typeof( AbbatoirDeed ), 1044290, 1044329, 100.0, 125.0, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Magery, 50.0, 55.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 40, 1044037 );

			craft = AddCraft( 510, typeof( GargishCouchSouthDeed ), 1044290, 1111775, 90.0, 115.0, typeof( Log ), 1044041, 75, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 511, typeof( GargishCouchEastDeed ), 1044290, 1111776, 90.0, 115.0, typeof( Log ), 1044041, 75, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 512, typeof( LongTableSouthDeed ), 1044290, 1111781, 90.0, 115.0, typeof( Log ), 1044041, 80, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 513, typeof( LongTableEastDeed ), 1044290, 1111782, 90.0, 115.0, typeof( Log ), 1044041, 80, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 514, typeof( ShortTableDeed ), 1044290, 1095307, 80.0, 105.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 515, typeof( TerMurStyleDresserSouthAddonDeed ), 1044290, 1111783, 90.0, 115.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresSA = true;

			craft = AddCraft( 516, typeof( TerMurStyleDresserEastAddonDeed ), 1044290, 1111784, 90.0, 115.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.RequiresSA = true;
			#endregion

			#region Tailoring and Cooking
			craft = AddCraft( 133, typeof( Dressform ), 1044298, 1044339, 63.1, 88.1, typeof( Log ), 1044041, 25, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 134, typeof( DressformSide ), 1044298, 1044340, 63.1, 88.1, typeof( Log ), 1044041, 25, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 10, 1044287 );

			craft = AddCraft( 220, typeof( ElvenSpinningWheelEastDeed ), 1044298, 1073393, 75.0, 100.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 40, 1044287 );
			craft.RequiresML = true;

			craft = AddCraft( 221, typeof( ElvenSpinningWheelSouthDeed ), 1044298, 1072878, 75.0, 100.0, typeof( Log ), 1044041, 60, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 40, 1044287 );
			craft.RequiresML = true;

			craft = AddCraft( 222, typeof( ElvenOvenSouthDeed ), 1044298, 1073394, 80.0, 110.0, typeof( Log ), 1044041, 80, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 223, typeof( ElvenOvenEastDeed ), 1044298, 1073395, 80.0, 110.0, typeof( Log ), 1044041, 80, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 800, typeof( SpinningwheelEastDeed ), 1044298, 1044341, 73.6, 98.6, typeof( Log ), 1044041, 75, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 25, 1044287 );

			craft = AddCraft( 801, typeof( SpinningwheelSouthDeed ), 1044298, 1044342, 73.6, 98.6, typeof( Log ), 1044041, 75, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 25, 1044287 );

			craft = AddCraft( 802, typeof( LoomEastDeed ), 1044298, 1044343, 84.2, 109.2, typeof( Log ), 1044041, 85, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 25, 1044287 );

			craft = AddCraft( 803, typeof( LoomSouthDeed ), 1044298, 1044344, 84.2, 109.2, typeof( Log ), 1044041, 85, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 65.0, 70.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 25, 1044287 );

			craft = AddCraft( 900, typeof( StoneOvenEastDeed ), 1044298, 1044345, 68.4, 93.4, typeof( Log ), 1044041, 85, 1044351 );
			craft.AddSkill( SkillName.Tinkering, 50.0, 55.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 125, 1044037 );

			craft = AddCraft( 901, typeof( StoneOvenSouthDeed ), 1044298, 1044346, 68.4, 93.4, typeof( Log ), 1044041, 85, 1044351 );
			craft.AddSkill( SkillName.Tinkering, 50.0, 55.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 125, 1044037 );

			craft = AddCraft( 902, typeof( FlourMillEastDeed ), 1044298, 1044347, 94.7, 119.7, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Tinkering, 50.0, 55.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 50, 1044037 );

			craft = AddCraft( 903, typeof( FlourMillSouthDeed ), 1044298, 1044348, 94.7, 119.7, typeof( Log ), 1044041, 100, 1044351 );
			craft.AddSkill( SkillName.Tinkering, 50.0, 55.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 50, 1044037 );

			AddCraft( 904, typeof( WaterTroughEastDeed ), 1044298, 1044349, 94.7, 119.7, typeof( Log ), 1044041, 150, 1044351 );

			AddCraft( 905, typeof( WaterTroughSouthDeed ), 1044298, 1044350, 94.7, 119.7, typeof( Log ), 1044041, 150, 1044351 );

			#endregion

			#region Anvils and Forges
			craft = AddCraft( 219, typeof( ElvenForgeDeed ), 1111809, 1072875, 94.7, 119.7, typeof( Log ), 1044041, 200, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 509, typeof( SoulforgeDeed ), 1111809, 1095867, 100.0, 125.0, typeof( Log ), 1044041, 150, 1044351 );
			craft.AddRes( typeof( IronIngot ), 1044036, 150, 1044037 );
			craft.AddRes( typeof( RelicFragment ), 1031699, 1, 1044253 );
			craft.AddSkill( SkillName.Imbuing, 75.0, 100.0 );
			craft.RequiresSA = true;

			craft = AddCraft( 600, typeof( SmallForgeDeed ), 1111809, 1044330, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddSkill( SkillName.Blacksmith, 75.0, 80.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 75, 1044037 );

			craft = AddCraft( 601, typeof( LargeForgeEastDeed ), 1111809, 1044331, 78.9, 103.9, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddSkill( SkillName.Blacksmith, 80.0, 85.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 100, 1044037 );

			craft = AddCraft( 602, typeof( LargeForgeSouthDeed ), 1111809, 1044332, 78.9, 103.9, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddSkill( SkillName.Blacksmith, 80.0, 85.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 100, 1044037 );

			craft = AddCraft( 603, typeof( AnvilEastDeed ), 1111809, 1044333, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddSkill( SkillName.Blacksmith, 75.0, 80.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 150, 1044037 );

			craft = AddCraft( 604, typeof( AnvilSouthDeed ), 1111809, 1044334, 73.6, 98.6, typeof( Log ), 1044041, 5, 1044351 );
			craft.AddSkill( SkillName.Blacksmith, 75.0, 80.0 );
			craft.AddRes( typeof( IronIngot ), 1044036, 150, 1044037 );
			#endregion

			#region Training
			craft = AddCraft( 700, typeof( TrainingDummyEastDeed ), 1044297, 1044335, 68.4, 93.4, typeof( Log ), 1044041, 55, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );

			craft = AddCraft( 701, typeof( TrainingDummySouthDeed ), 1044297, 1044336, 68.4, 93.4, typeof( Log ), 1044041, 55, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );

			craft = AddCraft( 702, typeof( PickpocketDipEastDeed ), 1044297, 1044337, 73.6, 98.6, typeof( Log ), 1044041, 65, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );

			craft = AddCraft( 703, typeof( PickpocketDipSouthDeed ), 1044297, 1044338, 73.6, 98.6, typeof( Log ), 1044041, 65, 1044351 );
			craft.AddSkill( SkillName.Tailoring, 50.0, 55.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 60, 1044287 );
			#endregion

			// Set the overidable material
			SetSubRes( typeof( Log ), 1072643 );

			AddSubRes( typeof( Log ), 1072643, 0.0, 1044041, 11072652 );
			AddSubRes( typeof( OakLog ), 1072644, 65.0, 1044041, 1072652 );
			AddSubRes( typeof( AshLog ), 1072645, 80.0, 1044041, 1072652 );
			AddSubRes( typeof( YewLog ), 1072646, 95.0, 1044041, 1072652 );
			AddSubRes( typeof( HeartwoodLog ), 1072647, 100.0, 1044041, 1072652 );
			AddSubRes( typeof( BloodwoodLog ), 1072648, 100.0, 1044041, 1072652 );
			AddSubRes( typeof( FrostwoodLog ), 1072649, 100.0, 1044041, 1072652 );

			MarkOption = true;
			Repair = true;
			CanEnhance = true;
			Alter = true;
		}
	}
}