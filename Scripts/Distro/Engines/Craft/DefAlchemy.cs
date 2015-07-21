using System;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefAlchemy : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefAlchemy();
		}

		public override SkillName MainSkill { get { return SkillName.Alchemy; } }

		public override int GumpTitleNumber
		{
			get { return 1044001; } // <CENTER>ALCHEMY MENU</CENTER>
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

		private DefAlchemy()
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
			from.PlaySound( 0x242 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			Type type = item.ItemType;

			if ( failed )
			{
				if ( type != typeof( SmokeBomb ) )
					from.AddToBackpack( new Bottle() );

				return 1044043; // You failed to create the item, and some of your materials are lost.
			}
			else
			{
				if ( type != typeof( SmokeBomb ) )
				{
					from.PlaySound( 0x240 ); // Sound of a filling bottle

					if ( exceptional )
						return 1048136; // You create the potion and pour it into a keg.
				}

				return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region HealingAndCurative

			craft = AddCraft( 10, typeof( RefreshPotion ), 1116348, 1044538, -25.0, 25.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( BlackPearl ), 1044353, 1, 1042081 );

			craft = AddCraft( 11, typeof( GreaterRefreshPotion ), 1116348, 1044539, 25.0, 75.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( BlackPearl ), 1044353, 5, 1042081 );

			craft = AddCraft( 40, typeof( LesserHealPotion ), 1116348, 1044543, -25.0, 25.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Ginseng ), 1044356, 1, 1042081 );

			craft = AddCraft( 41, typeof( HealPotion ), 1116348, 1044544, 15.0, 65.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Ginseng ), 1044356, 3, 1042081 );

			craft = AddCraft( 42, typeof( GreaterHealPotion ), 1116348, 1044545, 55.0, 105.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Ginseng ), 1044356, 7, 1042081 );

			craft = AddCraft( 70, typeof( LesserCurePotion ), 1116348, 1044552, -10.0, 40.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Garlic ), 1044355, 1, 1042081 );

			craft = AddCraft( 71, typeof( CurePotion ), 1116348, 1044553, 25.0, 75.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Garlic ), 1044355, 3, 1042081 );

			craft = AddCraft( 72, typeof( GreaterCurePotion ), 1116348, 1044554, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Garlic ), 1044355, 6, 1042081 );

			craft = AddCraft( 157, typeof( ElixirOfRebirth ), 1116348, 1112762, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( MedusaBlood ), 1031702, 1, 1044253 );
			craft.AddRes( typeof( SpidersSilk ), 1044360, 3, 1042081 );
			craft.RequiresSA = true;

			#endregion

			#region Enhancement

			craft = AddCraft( 20, typeof( AgilityPotion ), 1116349, 1044540, 15.0, 65.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Bloodmoss ), 1044354, 1, 1042081 );

			craft = AddCraft( 21, typeof( GreaterAgilityPotion ), 1116349, 1044541, 35.0, 85.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Bloodmoss ), 1044354, 3, 1042081 );

			craft = AddCraft( 30, typeof( NightSightPotion ), 1116349, 1044542, -25.0, 25.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( SpidersSilk ), 1044360, 1, 1042081 );

			craft = AddCraft( 50, typeof( StrengthPotion ), 1116349, 1044546, 25.0, 75.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( MandrakeRoot ), 1044357, 2, 1042081 );

			craft = AddCraft( 51, typeof( GreaterStrengthPotion ), 1116349, 1044547, 45.0, 95.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( MandrakeRoot ), 1044357, 5, 1042081 );

			craft = AddCraft( 103, typeof( InvisibilityPotion ), 1116349, 1074860, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Bloodmoss ), 1044354, 4, 1042081 );
			craft.AddRes( typeof( Nightshade ), 1044358, 3, 1042081 );
			craft.RequiresML = true;
			craft.AddRecipe( (int) TinkerRecipe.InvisibilityPotion, this );

			#endregion

			#region Toxic

			craft = AddCraft( 60, typeof( LesserPoisonPotion ), 1116350, 1044548, -5.0, 45.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Nightshade ), 1044358, 1, 1042081 );

			craft = AddCraft( 61, typeof( PoisonPotion ), 1116350, 1044549, 15.0, 65.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Nightshade ), 1044358, 2, 1042081 );

			craft = AddCraft( 62, typeof( GreaterPoisonPotion ), 1116350, 1044550, 55.0, 105.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Nightshade ), 1044358, 4, 1042081 );

			craft = AddCraft( 63, typeof( DeadlyPoisonPotion ), 1116350, 1044551, 90.0, 140.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( Nightshade ), 1044358, 8, 1042081 );

			craft = AddCraft( 104, typeof( ParasiticPotion ), 1116350, 1072942, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( ParasiticPlant ), 1073474, 5, 1042081 );
			craft.RequiresML = true;
			craft.AddRecipe( (int) TinkerRecipe.ParasiticPotion, this );

			craft = AddCraft( 105, typeof( DarkglowPotion ), 1116350, 1072943, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( LuminescentFungi ), 1073475, 5, 1042081 );
			craft.RequiresML = true;
			craft.AddRecipe( (int) TinkerRecipe.DarkglowPotion, this );

			craft = AddCraft( 155, typeof( ScouringToxin ), 1116350, 1112292, 75.0, 100.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( ToxicVenomSac ), 1112291, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			#endregion

			#region Explosive

			craft = AddCraft( 80, typeof( LesserExplosionPotion ), 1116351, 1044555, 5.0, 55.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( SulfurousAsh ), 1044359, 3, 1042081 );

			craft = AddCraft( 81, typeof( ExplosionPotion ), 1116351, 1044556, 35.0, 85.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( SulfurousAsh ), 1044359, 5, 1042081 );

			craft = AddCraft( 82, typeof( GreaterExplosionPotion ), 1116351, 1044557, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( SulfurousAsh ), 1044359, 10, 1042081 );

			craft = AddCraft( 101, typeof( ConflagrationPotion ), 1116351, 1072096, 55.0, 105.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( GraveDust ), 1023983, 5, 1042081 );
			craft.RequiresSE = true;

			craft = AddCraft( 102, typeof( GreaterConflagrationPotion ), 1116351, 1072099, 70.0, 120.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( GraveDust ), 1023983, 10, 1042081 );
			craft.RequiresSE = true;

			craft = AddCraft( 141, typeof( ConfusionBlastPotion ), 1116351, 1072106, 50.0, 100.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( PigIron ), 1023978, 5, 1042081 );
			craft.RequiresSE = true;

			craft = AddCraft( 142, typeof( GreaterConfusionBlastPotion ), 1116351, 1072109, 65.0, 115.0, typeof( Bottle ), 1044529, 1, 1044558 );
			craft.AddRes( typeof( PigIron ), 1023978, 10, 1042081 );
			craft.RequiresSE = true;

			// TODO (HS)
			// 161 1095826 black powder
			// 162 1116304 match cord
			// 163 1116305 fuse cord

			#endregion

			#region Strange Brew

			craft = AddCraft( 83, typeof( SmokeBomb ), 1116353, 1030248, 90.0, 120.0, typeof( Eggs ), 1044477, 1, 1044253 );
			craft.AddRes( typeof( Ginseng ), 1044356, 3, 1044364 );
			craft.RequiresSE = true;

			craft = AddCraft( 106, typeof( HoveringWisp ), 1116353, 1072881, 75.0, 125.0, typeof( CapturedEssence ), 1032686, 4, 1042081 );
			craft.RequiresML = true;
			craft.AddRecipe( (int) TinkerRecipeGreater.HoveringWisp, this );

			craft = AddCraft( 151, typeof( NaturalDye ), 1116353, 1112136, 75.0, 100.0, typeof( PlantPigment ), 1112132, 1, 1044253 );
			craft.AddRes( typeof( ColorFixative ), 1112135, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			#endregion

			#region Ingredients

			craft = AddCraft( 150, typeof( PlantPigment ), 1044495, 1112132, 75.0, 100.0, typeof( PlantClippings ), 1112131, 1, 1044253 );
			craft.AddRes( typeof( Bottle ), 1044250, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 152, typeof( ColorFixative ), 1044495, 1112135, 75.0, 100.0, typeof( SilverSerpentVenom ), 1112173, 1, 1044253 );
			craft.AddRes( typeof( BaseBeverage ), 1022503, 1, 1044253 );
			craft.BeverageType = BeverageType.Wine;
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 153, typeof( CrystalGranules ), 1044495, 1112329, 75.0, 100.0, typeof( ShimmeringCrystals ), 1075095, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 154, typeof( CrystalDust ), 1044495, 1112328, 75.0, 100.0, typeof( ICrystal ), 1074261, 4, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 156, typeof( SoftenedReeds ), 1044495, 1112249, 75.0, 100.0, typeof( DryReeds ), 1112248, 2, 1112250 );
			craft.AddRes( typeof( ScouringToxin ), 1112292, 1, 1112326 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 158, typeof( VialOfVitriol ), 1044495, 1113331, 90.0, 115.0, typeof( ParasiticPotion ), 1072942, 1, 1044253 );
			craft.AddRes( typeof( Nightshade ), 1044358, 30, 1044366 );
			craft.AddSkill( SkillName.Magery, 75.0, 80.0 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 159, typeof( BottleOfIchor ), 1044495, 1113361, 90.0, 115.0, typeof( DarkglowPotion ), 1072943, 1, 1044253 );
			craft.AddRes( typeof( SpidersSilk ), 1044360, 30, 1044368 );
			craft.AddSkill( SkillName.Magery, 75.0, 80.0 );
			craft.ChanceAtMin = 0.2;
			craft.RequiresSA = true;

			if ( Expansion.HS )
			{
				craft = AddCraft( 160, typeof( Potash ), 1044495, 1116319, 0.0, 50.0, typeof( Board ), 1044041, 1, 1044253 );
				craft.AddRes( typeof( BaseBeverage ), 1046458, 1, 1044253 );
				craft.BeverageType = BeverageType.Water;
				craft.RequiresHS = true;
			}

			#endregion
		}
	}
}