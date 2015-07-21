using System;
using Server.Items;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum BowRecipe
	{
		BarbedLongbow = 47,
		SlayerLongbow = 48,
		FrozenLongbow = 49,
		LongbowOfMight = 50,
		RangersShortbow = 51,
		LightweightShortbow = 52,
		MysticalShortbow = 53,
		AssassinsShortbow = 54
	}

	public enum BowRecipeGreater
	{
		BlightGrippedLongbow = 42,
		FaerieFire = 43,
		SilvanisFeywoodBow = 44,
		MischiefMaker = 45,
		TheNightReaper = 46
	}
	#endregion

	public class DefBowFletching : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefBowFletching();
		}

		public override SkillName MainSkill { get { return SkillName.Fletching; } }

		public override int GumpTitleNumber
		{
			get { return 1044006; } // <CENTER>BOWCRAFT AND FLETCHING MENU</CENTER>
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

		private DefBowFletching()
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
			from.PlaySound( 0x55 );
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

		public override CraftECA ECA { get { return CraftECA.FiftyPercentChanceMinusTenPercent; } }

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region Materials
			AddCraft( 1, typeof( Kindling ), 1044457, 1023553, 0.0, 00.0, typeof( Log ), 1044041, 1, 1044351 );

			craft = AddCraft( 2, typeof( Shaft ), 1044457, 1027124, 0.0, 40.0, typeof( Log ), 1044041, 1, 1044351 );
			craft.UseAllRes = true;
			#endregion

			#region Ammunition
			craft = AddCraft( 3, typeof( Arrow ), 1044565, 1023903, 0.0, 40.0, typeof( Shaft ), 1044560, 1, 1044561 );
			craft.AddRes( typeof( Feather ), 1044562, 1, 1044563 );
			craft.UseAllRes = true;

			craft = AddCraft( 4, typeof( Bolt ), 1044565, 1027163, 0.0, 40.0, typeof( Shaft ), 1044560, 1, 1044561 );
			craft.AddRes( typeof( Feather ), 1044562, 1, 1044563 );
			craft.UseAllRes = true;

			craft = AddCraft( 5, typeof( FukiyaDart ), 1044565, 1030246, 50.0, 90.0, typeof( Log ), 1044041, 1, 1044351 );
			craft.UseAllRes = true;
			craft.RequiresSE = true;
			#endregion

			#region Weapons
			AddCraft( 6, typeof( Bow ), 1044566, 1025042, 30.0, 70.0, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( 7, typeof( Crossbow ), 1044566, 1023919, 60.0, 100.0, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( 8, typeof( HeavyCrossbow ), 1044566, 1025117, 80.0, 120.0, typeof( Log ), 1044041, 10, 1044351 );

			AddCraft( 9, typeof( CompositeBow ), 1044566, 1029922, 70.0, 110.0, typeof( Log ), 1044041, 7, 1044351 );
			AddCraft( 10, typeof( RepeatingCrossbow ), 1044566, 1029923, 90.0, 130.0, typeof( Log ), 1044041, 10, 1044351 );

			craft = AddCraft( 11, typeof( Yumi ), 1044566, 1030224, 90.0, 130.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 12, typeof( ElvenCompositeLongBow ), 1044566, 1031550, 95.0, 135.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 13, typeof( MagicalShortbow ), 1044566, 1031551, 85.0, 125.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.RequiresML = true;

			craft = AddCraft( 14, typeof( BlightGrippedLongbow ), 1044566, 1072907, 80.0, 130.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( LardOfParoxysmus ), 1032681, 1, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRecipe( (int) BowRecipeGreater.BlightGrippedLongbow, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 15, typeof( FaerieFire ), 1044566, 1072908, 80.0, 130.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( LardOfParoxysmus ), 1032681, 1, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRes( typeof( Taint ), 1032679, 10, 1044253 );
			craft.AddRecipe( (int) BowRecipeGreater.FaerieFire, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 16, typeof( SilvanisFeywoodBow ), 1044566, 1072955, 80.0, 130.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( LardOfParoxysmus ), 1032681, 1, 1044253 );
			craft.AddRes( typeof( Muculent ), 1032680, 10, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRecipe( (int) BowRecipeGreater.SilvanisFeywoodBow, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 17, typeof( MischiefMaker ), 1044566, 1072910, 80.0, 130.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( DreadHornMane ), 1032682, 1, 1044253 );
			craft.AddRes( typeof( Corruption ), 1032676, 10, 1044253 );
			craft.AddRes( typeof( Putrefaction ), 1032678, 10, 1044253 );
			craft.AddRecipe( (int) BowRecipeGreater.MischiefMaker, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 18, typeof( TheNightReaper ), 1044566, 1072912, 80.0, 130.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( DreadHornMane ), 1032682, 1, 1044253 );
			craft.AddRes( typeof( Blight ), 1032675, 10, 1044253 );
			craft.AddRes( typeof( Scourge ), 1032677, 10, 1044253 );
			craft.AddRecipe( (int) BowRecipeGreater.TheNightReaper, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 100, typeof( BarbedLongbow ), 1044566, 1073505, 75.0, 125.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( FireRuby ), 1026254, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.BarbedLongbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 101, typeof( SlayerLongbow ), 1044566, 1073506, 75.0, 125.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.SlayerLongbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 102, typeof( FrozenLongbow ), 1044566, 1073507, 75.0, 125.0, typeof( Log ), 1044041, 20, 1044351 );
			craft.AddRes( typeof( Turquoise ), 1026250, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.FrozenLongbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 103, typeof( LongbowOfMight ), 1044566, 1073508, 75.0, 125.0, typeof( Log ), 1044041, 10, 1044351 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.LongbowOfMight, this );
			craft.RequiresML = true;

			craft = AddCraft( 104, typeof( RangersShortbow ), 1044566, 1073509, 75.0, 125.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.RangersShortbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 105, typeof( LightweightShortbow ), 1044566, 1073510, 75.0, 125.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.LightweightShortbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 106, typeof( MysticalShortbow ), 1044566, 1073511, 75.0, 125.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.MysticalShortbow, this );
			craft.RequiresML = true;

			craft = AddCraft( 107, typeof( AssassinsShortbow ), 1044566, 1073512, 75.0, 125.0, typeof( Log ), 1044041, 15, 1044351 );
			craft.AddRes( typeof( DarkSapphire ), 1032690, 1, 1044253 );
			craft.AddRecipe( (int) BowRecipe.AssassinsShortbow, this );
			craft.RequiresML = true;
			#endregion

			// Set the overidable material
			SetSubRes( typeof( Log ), 1072643 );

			// Add every material you want the player to be able to chose from
			// This will overide the overidable material
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
		}
	}
}
