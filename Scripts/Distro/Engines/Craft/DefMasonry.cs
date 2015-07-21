using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefMasonry : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefMasonry();
		}

		public override SkillName MainSkill { get { return SkillName.Carpentry; } }

		public override int GumpTitleNumber
		{
			get { return 1044500; } // <CENTER>MASONRY MENU</CENTER> 
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		public override double DefaultChanceAtMin
		{
			get { return 0.0; }
		}

		private DefMasonry()
			: base( 1, 1, 1.25 )
		{
		}

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			return true;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !( from is PlayerMobile && ( (PlayerMobile) from ).Masonry && from.Skills[SkillName.Carpentry].Base >= 100.0 ) )
				return 1044633; // You havent learned stonecraft.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
		}

		// Delay to synchronize the sound with the hit on the anvil 
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from )
				: base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x23D );
			}
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

			#region Decorations
			AddCraft( 1, typeof( Vase ), 1044501, 1022888, 52.5, 102.5, typeof( Granite ), 1044514, 1, 1044513 );
			AddCraft( 2, typeof( LargeVase ), 1044501, 1022887, 52.5, 102.5, typeof( Granite ), 1044514, 3, 1044513 );

			craft = AddCraft( 3, typeof( SmallUrn ), 1044501, 1029244, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
			craft.RequiresSE = true;
			craft = AddCraft( 4, typeof( SmallTowerSculpture ), 1044501, 1029242, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );
			craft.RequiresSE = true;

			craft = AddCraft( 609, typeof( GargishPainting ), 1044501, 1095317, 83.0, 133.0, typeof( Granite ), 1044514, 3, 1044513 );
			craft.RequiresSA = true;
			craft = AddCraft( 610, typeof( GargishSculpture ), 1044501, 1095319, 82.0, 132.0, typeof( Granite ), 1044514, 4, 1044513 );
			craft.RequiresSA = true;
			craft = AddCraft( 611, typeof( GargishVase ), 1044501, 1095322, 80.0, 130.0, typeof( Granite ), 1044514, 4, 1044513 );
			craft.RequiresSA = true;

			#endregion

			#region Furniture
			AddCraft( 5, typeof( StoneChair ), 1044502, 1024635, 55.0, 105.0, typeof( Granite ), 1044514, 4, 1044513 );

			AddCraft( 500, typeof( MediumStoneTableEastDeed ), 1044502, 1044508, 65.0, 115.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( 501, typeof( MediumStoneTableSouthDeed ), 1044502, 1044509, 65.0, 115.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( 502, typeof( LargeStoneTableEastDeed ), 1044502, 1044511, 75.0, 125.0, typeof( Granite ), 1044514, 9, 1044513 );
			AddCraft( 503, typeof( LargeStoneTableSouthDeed ), 1044502, 1044512, 75.0, 125.0, typeof( Granite ), 1044514, 9, 1044513 );
			#endregion

			#region Statues
			AddCraft( 6, typeof( StatueSouth ), 1044503, 1044505, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( 7, typeof( StatueNorth ), 1044503, 1044506, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( 8, typeof( StatueEast ), 1044503, 1044507, 60.0, 120.0, typeof( Granite ), 1044514, 3, 1044513 );
			AddCraft( 9, typeof( StatuePegasus ), 1044503, 1044510, 70.0, 130.0, typeof( Granite ), 1044514, 4, 1044513 );
			#endregion

			#region Misc. Add-Ons
			craft = AddCraft( 504, typeof( StoneAnvilSouthDeed ), 1044290, 1072876, 78.0, 128.0, typeof( Granite ), 1044514, 20, 1044513 );
			craft.AddRecipe( (int) CarpRecipe.StoneAnvilSouth, this );
			craft.RequiresML = true;

			craft = AddCraft( 505, typeof( StoneAnvilEastDeed ), 1044290, 1073392, 78.0, 128.0, typeof( Granite ), 1044514, 20, 1044513 );
			craft.AddRecipe( (int) CarpRecipe.StoneAnvilEast, this );
			craft.RequiresML = true;

			craft = AddCraft( 506, typeof( LargeGargishBedSouthAddonDeed ), 1044290, 1111761, 94.7, 103.5, typeof( Granite ), 1044514, 100, 1044513 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 75.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresSA = true;

			craft = AddCraft( 507, typeof( LargeGargishBedEastAddonDeed ), 1044290, 1111762, 94.7, 103.5, typeof( Granite ), 1044514, 100, 1044513 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 75.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresSA = true;

			craft = AddCraft( 508, typeof( GargishCotSouthAddonDeed ), 1044290, 1111920, 94.7, 103.5, typeof( Granite ), 1044514, 100, 1044513 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 75.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresSA = true;

			craft = AddCraft( 509, typeof( GargishCotEastAddonDeed ), 1044290, 1111921, 94.7, 103.5, typeof( Granite ), 1044514, 100, 1044513 );
			craft.AddSkill( SkillName.Tailoring, 75.0, 75.0 );
			craft.AddRes( typeof( Cloth ), 1044286, 100, 1044287 );
			craft.RequiresSA = true;
			#endregion

			#region Stone Armor
			AddCraft( 600, typeof( FemaleGargishStoneArms ), 1111705, 1095343, 56.3, 106.3, typeof( Granite ), 1044514, 8, 1044513 );
			AddCraft( 601, typeof( FemaleGargishStoneChest ), 1111705, 1095345, 55.0, 105.0, typeof( Granite ), 1044514, 12, 1044513 );
			AddCraft( 602, typeof( FemaleGargishStoneLeggings ), 1111705, 1095349, 58.8, 108.8, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( 603, typeof( FemaleGargishStoneKilt ), 1111705, 1095347, 48.9, 98.9, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( 604, typeof( FemaleGargishStoneArms ), 1111705, 1095344, 56.3, 106.3, typeof( Granite ), 1044514, 8, 1044513 );
			AddCraft( 605, typeof( FemaleGargishStoneChest ), 1111705, 1095346, 55.0, 105.0, typeof( Granite ), 1044514, 12, 1044513 );
			AddCraft( 606, typeof( FemaleGargishStoneLeggings ), 1111705, 1095350, 58.8, 108.8, typeof( Granite ), 1044514, 10, 1044513 );
			AddCraft( 607, typeof( FemaleGargishStoneKilt ), 1111705, 1095348, 48.9, 98.9, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( 612, typeof( LargeStoneShield ), 1111705, 1095773, 55.0, 105.0, typeof( Granite ), 1044514, 16, 1044513 );
			#endregion

			#region Stone Weapons
			AddCraft( 608, typeof( StoneWarSword ), 1111719, 1095369, 55.0, 105.0, typeof( Granite ), 1044514, 18, 1044513 );
			#endregion

			// Set the overidable material
			SetSubRes( typeof( Granite ), 1044525 );

			// Add every material you want the player to be able to chose from
			// This will overide the overidable material
			AddSubRes( typeof( Granite ), 1044525, 00.0, 1044514, 1044526 );
			AddSubRes( typeof( DullCopperGranite ), 1044023, 65.0, 1044514, 1044527 );
			AddSubRes( typeof( ShadowIronGranite ), 1044024, 70.0, 1044514, 1044527 );
			AddSubRes( typeof( CopperGranite ), 1044025, 75.0, 1044514, 1044527 );
			AddSubRes( typeof( BronzeGranite ), 1044026, 80.0, 1044514, 1044527 );
			AddSubRes( typeof( GoldGranite ), 1044027, 85.0, 1044514, 1044527 );
			AddSubRes( typeof( AgapiteGranite ), 1044028, 90.0, 1044514, 1044527 );
			AddSubRes( typeof( VeriteGranite ), 1044029, 95.0, 1044514, 1044527 );
			AddSubRes( typeof( ValoriteGranite ), 1044030, 99.0, 1044514, 1044527 );
		}
	}
}