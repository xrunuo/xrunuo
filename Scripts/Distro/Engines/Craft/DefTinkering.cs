using System;
using Server;
using Server.Items;
using Server.Factions;
using Server.Targeting;

namespace Server.Engines.Craft
{
	#region Recipes
	public enum TinkerRecipe
	{
		InvisibilityPotion = 89,
		DarkglowPotion = 91,
		ParasiticPotion = 90
	}

	public enum TinkerRecipeGreater
	{
		EssenceOfBattle = 74,
		PendantOfTheMagi = 75,
		ResilientBracer = 73,
		ScrappersCompendium = 64,
		HoveringWisp = 92
	}
	#endregion

	public class DefTinkering : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefTinkering();
		}

		public override SkillName MainSkill { get { return SkillName.Tinkering; } }

		public override int GumpTitleNumber
		{
			get { return 1044007; } // <CENTER>TINKERING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		private DefTinkering()
			: base( 1, 1, 1.25 )
		{
		}

		public override double DefaultChanceAtMin
		{
			get { return 0.0; }
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.
			else if ( itemType != null && ( itemType.IsSubclassOf( typeof( BaseFactionTrapDeed ) ) || itemType == typeof( FactionTrapRemovalKit ) ) && Faction.Find( from ) == null )
				return 1044573; // You have to be in a faction to do that.

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
		}

		private static Type[] m_TinkerColorables = new Type[]
			{
				typeof( ForkLeft ),			typeof( ForkRight ),
				typeof( SpoonLeft ),		typeof( SpoonRight ),
				typeof( KnifeLeft ),		typeof( KnifeRight ),
				typeof( Plate ),			typeof( Goblet ),
				typeof( PewterMug ),		typeof( KeyRing ),
				typeof( Candelabra ),		typeof( Scales ),
				typeof( IronKey ),			typeof( Globe ),
				typeof( Spyglass ),			typeof( Lantern ),
				typeof( HeatingStand ),		typeof( SoftenedReeds ),
				typeof( WeavableBasket ),	typeof( TerMurStyleCandelabra ),
				typeof( GorgonLens )
			};

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			if ( type.IsSubclassOf( typeof( BaseIngot ) ) )
				return true;

			if ( type == typeof( CrystalDust ) )
				return false;

			type = item.ItemType;

			bool contains = false;

			for ( int i = 0; !contains && i < m_TinkerColorables.Length; ++i )
				contains = ( m_TinkerColorables[i].IsAssignableFrom( type ) );

			return contains;
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

		public override bool ConsumeOnFailure( Mobile from, Type resourceType, CraftItem craftItem )
		{
			if ( resourceType == typeof( Silver ) )
				return false;

			return base.ConsumeOnFailure( from, resourceType, craftItem );
		}

		public void AddJewelrySet( int index, GemType gemType, Type itemType )
		{
			int offset = (int) gemType - 1;
			CraftItem craft;

			craft = AddCraft( index++, typeof( GoldRing ), 1044049, 1044176 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );

			craft = AddCraft( index++, typeof( SilverBeadNecklace ), 1044049, 1044185 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );

			craft = AddCraft( index++, typeof( GoldNecklace ), 1044049, 1044194 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );

			craft = AddCraft( index++, typeof( GoldEarrings ), 1044049, 1044203 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );

			craft = AddCraft( index++, typeof( GoldBeadNecklace ), 1044049, 1044212 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );

			craft = AddCraft( index++, typeof( GoldBracelet ), 1044049, 1044221 + offset, 40.0, 90.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( itemType, 1044231 + offset, 1, 1044240 );
		}

		public void AddBasket( int craftId, Type type, int name, int shafts, int reeds )
		{
			CraftItem craft = AddCraft( craftId, type, 1044042, name, 75.0, 100.0, typeof( Shaft ), 1112247, shafts, 1112246 );
			craft.AddRes( typeof( SoftenedReeds ), 1112249, reeds, 1112251 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;
		}

		public override void InitCraftList()
		{
			CraftItem craft = null;

			#region Wooden Items
			AddCraft( 1, typeof( JointingPlane ), 1044042, 1024144, 0.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( 2, typeof( MouldingPlane ), 1044042, 1024140, 0.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( 3, typeof( SmoothingPlane ), 1044042, 1024146, 0.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( 4, typeof( ClockFrame ), 1044042, 1024173, 0.0, 50.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( 5, typeof( Axle ), 1044042, 1024187, -25.0, 25.0, typeof( Log ), 1044041, 2, 1044351 );
			AddCraft( 6, typeof( RollingPin ), 1044042, 1024163, 0.0, 50.0, typeof( Log ), 1044041, 5, 1044351 );

			craft = AddCraft( 7, typeof( Nunchaku ), 1044042, 1030158, 70.0, 120.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft.AddRes( typeof( Log ), 1044041, 8, 1044351 );
			craft.RequiresSE = true;

			// Basketweaving
			craft = AddCraft( 350, typeof( SoftenedReeds ), 1044042, 1112249, 75.0, 100.0, typeof( DryReeds ), 1112248, 2, 1112250 );
			craft.AddRes( typeof( ScouringToxin ), 1112292, 1, 1112326 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			AddBasket( 351, typeof( RoundBasket ), 1112293, 3, 2 );
			AddBasket( 352, typeof( Bushel ), 1112357, 3, 2 );
			AddBasket( 353, typeof( SmallBushel ), 1112337, 2, 1 );
			AddBasket( 354, typeof( PicnicBasketW ), 1112356, 2, 1 );
			AddBasket( 355, typeof( WinnowingBasket ), 1112355, 3, 2 );
			AddBasket( 356, typeof( SquareBasket ), 1112295, 3, 2 );
			AddBasket( 357, typeof( BasketW ), 1112294, 3, 2 );
			AddBasket( 358, typeof( TallRoundBasket ), 1112297, 4, 3 );
			AddBasket( 359, typeof( SmallSquareBasket ), 1112296, 2, 1 );
			AddBasket( 360, typeof( TallBasket ), 1112299, 3, 2 );
			AddBasket( 361, typeof( SmallRoundBasket ), 1112298, 2, 1 );
			#endregion

			#region Tools
			AddCraft( 8, typeof( Scissors ), 1044046, 1023998, 5.0, 55.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 9, typeof( MortarPestle ), 1044046, 1023739, 20.0, 70.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 10, typeof( Scorp ), 1044046, 1024327, 30.0, 80.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 11, typeof( TinkerTools ), 1044046, 1044164, 10.0, 60.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 12, typeof( Hatchet ), 1044046, 1023907, 30.0, 80.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 13, typeof( DrawKnife ), 1044046, 1024324, 30.0, 80.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 14, typeof( SewingKit ), 1044046, 1023997, 10.0, 70.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 15, typeof( Saw ), 1044046, 1024148, 30.0, 80.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 16, typeof( DovetailSaw ), 1044046, 1024136, 30.0, 80.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 17, typeof( Froe ), 1044046, 1024325, 30.0, 80.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 18, typeof( Shovel ), 1044046, 1023898, 40.0, 90.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 19, typeof( Hammer ), 1044046, 1024138, 30.0, 80.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 20, typeof( Tongs ), 1044046, 1024028, 35.0, 85.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 21, typeof( SmithHammer ), 1044046, 1025091, 40.0, 90.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 22, typeof( SledgeHammer ), 1044046, 1024021, 40.0, 90.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 23, typeof( Inshave ), 1044046, 1024326, 30.0, 80.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 24, typeof( Pickaxe ), 1044046, 1023718, 40.0, 90.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 25, typeof( Lockpick ), 1044046, 1025371, 45.0, 95.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 26, typeof( Skillet ), 1044046, 1044567, 30.0, 80.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 27, typeof( FlourSifter ), 1044046, 1024158, 50.0, 100.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 28, typeof( FletcherTools ), 1044046, 1044166, 35.0, 85.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 29, typeof( MapmakersPen ), 1044046, 1044167, 25.0, 75.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 30, typeof( ScribesPen ), 1044046, 1044168, 25.0, 75.0, typeof( IronIngot ), 1044036, 1, 1044037 );

			craft = AddCraft( 506, typeof( Clippers ), 1044046, 1112117, 50.0, 75.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 708, typeof( MetalContainerEngravingTool ), 1044046, 1072154, 75.0, 100.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			craft.AddRes( typeof( Diamond ), 1023878, 1, 1044253 );
			craft.AddRes( typeof( Gears ), 1044254, 2, 1044253 );
			craft.AddRes( typeof( Springs ), 1011201, 1, 1044253 );
			craft.RequiresML = true;
			#endregion

			#region Parts
			AddCraft( 31, typeof( Gears ), 1044047, 1024179, 5.0, 55.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 32, typeof( ClockParts ), 1044047, 1024175, 25.0, 75.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 33, typeof( BarrelTap ), 1044047, 1024100, 35.0, 85.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 34, typeof( Springs ), 1044047, 1024189, 5.0, 55.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 35, typeof( SextantParts ), 1044047, 1024185, 30.0, 80.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 36, typeof( BarrelHoops ), 1044047, 1024321, -15.0, 35.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			AddCraft( 37, typeof( Hinge ), 1044047, 1024181, 5.0, 55.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 38, typeof( BolaBall ), 1044047, 1023699, 45.0, 95.0, typeof( IronIngot ), 1044036, 10, 1044037 );

			craft = AddCraft( 709, typeof( JeweledFiligree ), 1044047, 1032126, 70.0, 115.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( typeof( Ruby ), 1023859, 1, 1044253 );
			craft.AddRes( typeof( StarSapphire ), 1023855, 1, 1044253 );
			craft.RequiresML = true;
			#endregion

			#region Utensils
			AddCraft( 39, typeof( ButcherKnife ), 1044048, 1025110, 25.0, 75.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 40, typeof( SpoonLeft ), 1044048, 1044158, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 41, typeof( SpoonRight ), 1044048, 1044159, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 42, typeof( Plate ), 1044048, 1022519, 0.0, 50.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 43, typeof( ForkLeft ), 1044048, 1044160, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 44, typeof( ForkRight ), 1044048, 1044161, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 45, typeof( Cleaver ), 1044048, 1023778, 20.0, 70.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 46, typeof( KnifeLeft ), 1044048, 1044162, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 47, typeof( KnifeRight ), 1044048, 1044163, 0.0, 50.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			AddCraft( 48, typeof( Goblet ), 1044048, 1022458, 10.0, 60.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 49, typeof( PewterMug ), 1044048, 1024097, 10.0, 60.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 50, typeof( SkinningKnife ), 1044048, 1023781, 25.0, 75.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			#endregion

			#region Misc
			AddCraft( 51, typeof( KeyRing ), 1044050, 1024113, 10.0, 60.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 52, typeof( Candelabra ), 1044050, 1022599, 55.0, 105.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 53, typeof( Scales ), 1044050, 1026225, 60.0, 110.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 54, typeof( IronKey ), 1044050, 1024112, 20.0, 70.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( 55, typeof( Globe ), 1044050, 1024167, 55.0, 105.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 56, typeof( Spyglass ), 1044050, 1025365, 60.0, 110.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( 57, typeof( Lantern ), 1044050, 1022597, 30.0, 80.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( 58, typeof( HeatingStand ), 1044050, 1026217, 60.0, 110.0, typeof( IronIngot ), 1044036, 4, 1044037 );

			craft = AddCraft( 59, typeof( ShojiLantern ), 1044050, 1029404, 65.0, 115.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( Log ), 1044041, 5, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 60, typeof( PaperLantern ), 1044050, 1029406, 65.0, 115.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( Log ), 1044041, 5, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 61, typeof( RoundPaperLantern ), 1044050, 1029418, 65.0, 115.0, typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( Log ), 1044041, 5, 1044351 );
			craft.RequiresSE = true;

			craft = AddCraft( 62, typeof( WindChimes ), 1044050, 1030290, 80.0, 130.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresSE = true;

			craft = AddCraft( 63, typeof( FancyWindChimes ), 1044050, 1030291, 80.0, 130.0, typeof( IronIngot ), 1044036, 15, 1044037 );
			craft.RequiresSE = true;

			craft = AddCraft( 64, typeof( TerMurStyleCandelabra ), 1044050, 1095313, 55.0, 105.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			craft.RequiresSA = true;

			craft = AddCraft( 505, typeof( GorgonLens ), 1044050, 1112625, 90.0, 115.0, typeof( IScales ), 1113463, 2, 1053097 );
			craft.AddRes( typeof( CrystalDust ), 1112328, 3, 1044253 );
			craft.ChanceAtMin = 0.5;

			// TODO (SA):
			// 507	1112480	a scale collar
			#endregion

			#region Jewelry
			craft = AddCraft( 65, typeof( GoldRing ), 1044049, 1011207, 65.0, 115.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft = AddCraft( 66, typeof( GoldBracelet ), 1044049, 1011219, 55.0, 105.0, typeof( IronIngot ), 1044036, 3, 1044037 );

			craft = AddCraft( 67, typeof( GargishNecklace ), 1044049, 1095784, 60.0, 110.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft.ForceCannotEnhance = true;
			craft = AddCraft( 68, typeof( GargishBracelet ), 1044049, 1095785, 55.0, 105.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft = AddCraft( 69, typeof( GargishRing ), 1044049, 1095786, 65.0, 115.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft = AddCraft( 70, typeof( GargishEarrings ), 1044049, 1095787, 55.0, 105.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			craft.ForceCannotEnhance = true;

			AddJewelrySet( 100, GemType.StarSapphire, typeof( StarSapphire ) );
			AddJewelrySet( 106, GemType.Emerald, typeof( Emerald ) );
			AddJewelrySet( 112, GemType.Sapphire, typeof( Sapphire ) );
			AddJewelrySet( 118, GemType.Ruby, typeof( Ruby ) );
			AddJewelrySet( 124, GemType.Citrine, typeof( Citrine ) );
			AddJewelrySet( 130, GemType.Amethyst, typeof( Amethyst ) );
			AddJewelrySet( 136, GemType.Tourmaline, typeof( Tourmaline ) );
			AddJewelrySet( 142, GemType.Amber, typeof( Amber ) );
			AddJewelrySet( 148, GemType.Diamond, typeof( Diamond ) );
			#endregion

			#region Assemblies
			craft = AddCraft( 154, typeof( AxleGears ), 1044051, 1024177, 0.0, 0.0, typeof( Axle ), 1044169, 1, 1044253 );
			craft.AddRes( typeof( Gears ), 1044254, 1, 1044253 );

			craft = AddCraft( 155, typeof( ClockParts ), 1044051, 1024175, 0.0, 0.0, typeof( AxleGears ), 1044170, 1, 1044253 );
			craft.AddRes( typeof( Springs ), 1044171, 1, 1044253 );

			craft = AddCraft( 156, typeof( SextantParts ), 1044051, 1024185, 0.0, 0.0, typeof( AxleGears ), 1044170, 1, 1044253 );
			craft.AddRes( typeof( Hinge ), 1044172, 1, 1044253 );

			craft = AddCraft( 157, typeof( ClockRight ), 1044051, 1044257, 0.0, 0.0, typeof( ClockFrame ), 1044174, 1, 1044253 );
			craft.AddRes( typeof( ClockParts ), 1044173, 1, 1044253 );

			craft = AddCraft( 158, typeof( ClockLeft ), 1044051, 1044256, 0.0, 0.0, typeof( ClockFrame ), 1044174, 1, 1044253 );
			craft.AddRes( typeof( ClockParts ), 1044173, 1, 1044253 );

			AddCraft( 159, typeof( Sextant ), 1044051, 1024183, 0.0, 0.0, typeof( SextantParts ), 1044175, 1, 1044253 );

			craft = AddCraft( 207, typeof( Bola ), 1044051, 1046441, 60.0, 110.0, typeof( BolaBall ), 1046440, 4, 1042613 );
			craft.AddRes( typeof( Leather ), 1044462, 3, 1044463 );

			craft = AddCraft( 300, typeof( PotionKeg ), 1044051, 1044258, 75.0, 100.0, typeof( Keg ), 1044255, 1, 1044253 );
			craft.AddRes( typeof( BarrelTap ), 1044252, 1, 1044253 );
			craft.AddRes( typeof( BarrelLid ), 1044251, 1, 1044253 );
			craft.AddRes( typeof( Bottle ), 1044250, 10, 1044253 );
			craft.ChanceAtMin = 0.5;

			craft = AddCraft( 713, typeof( HitchingRope ), 1044051, 1071124, 60.0, 120.0, typeof( Rope ), 1020934, 1, 1044253 );
			craft.AddSkill( SkillName.AnimalLore, 15.0, 100.0 );
			craft.AddRes( typeof( ResolvesBridle ), 1074761, 1, 1044253 );

			craft = AddCraft( 714, typeof( HitchingPost ), 1044051, 1071127, 90.0, 160.0, typeof( IronIngot ), 1044036, 50, 1044253 );
			craft.AddRes( typeof( AnimalPheromone ), 1071200, 1, 1044253 );
			craft.AddRes( typeof( HitchingRope ), 1071124, 2, 1044253 );
			craft.AddRes( typeof( PhillipsWoodenSteed ), 1063488, 1, 1044253 );

			craft = AddCraft( 508, typeof( LeatherWolfAssembly ), 1044051, 1113031, 90.0, 115.0, typeof( ClockworkAssembly ), 1073426, 1, 1044253 );
			craft.AddRes( typeof( PowerCrystal ), 1112811, 1, 1044253 );
			craft.AddRes( typeof( VoidEssence ), 1112327, 2, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 509, typeof( ClockworkScorpionAssembly ), 1044051, 1113032, 90.0, 115.0, typeof( ClockworkAssembly ), 1073426, 1, 1044253 );
			craft.AddRes( typeof( PowerCrystal ), 1112811, 1, 1044253 );
			craft.AddRes( typeof( VoidEssence ), 1112327, 1, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;

			craft = AddCraft( 510, typeof( VollemAssembly ), 1044051, 1113033, 90.0, 115.0, typeof( ClockworkAssembly ), 1073426, 1, 1044253 );
			craft.AddRes( typeof( PowerCrystal ), 1112811, 1, 1044253 );
			craft.AddRes( typeof( VoidEssence ), 1112327, 3, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.RequiresSA = true;
			#endregion

			#region Traps
			craft = AddCraft( 400, typeof( DartTrapCraft ), 1044052, 1024396, 30.0, 80.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			craft.AddRes( typeof( Bolt ), 1044570, 1, 1044253 );

			craft = AddCraft( 401, typeof( PoisonTrapCraft ), 1044052, 1044593, 30.0, 80.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			craft.AddRes( typeof( BasePoisonPotion ), 1044571, 1, 1044253 );

			craft = AddCraft( 402, typeof( ExplosionTrapCraft ), 1044052, 1044597, 55.0, 110.0, typeof( IronIngot ), 1044036, 1, 1044037 );
			craft.AddRes( typeof( BaseExplosionPotion ), 1044569, 1, 1044253 );

			craft = AddCraft( 500, typeof( FactionGasTrapDeed ), 1044052, 1044598, 65.0, 115.0, typeof( Silver ), 1044572, 250, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( BasePoisonPotion ), 1044571, 1, 1044253 );

			craft = AddCraft( 501, typeof( FactionExplosionTrapDeed ), 1044052, 1044599, 65.0, 115.0, typeof( Silver ), 1044572, 250, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( BaseExplosionPotion ), 1044569, 1, 1044253 );

			craft = AddCraft( 502, typeof( FactionSawTrapDeed ), 1044052, 1044600, 65.0, 115.0, typeof( Silver ), 1044572, 250, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( Gears ), 1044254, 1, 1044253 );

			craft = AddCraft( 503, typeof( FactionSpikeTrapDeed ), 1044052, 1044601, 65.0, 115.0, typeof( Silver ), 1044572, 250, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.AddRes( typeof( Springs ), 1044171, 1, 1044253 );

			craft = AddCraft( 504, typeof( FactionTrapRemovalKit ), 1044052, 1046445, 90.0, 115.0, typeof( Silver ), 1044572, 500, 1044253 );
			craft.AddRes( typeof( IronIngot ), 1044036, 10, 1044037 );
			craft.ChanceAtMin = 0.5;
			#endregion

			#region Magic Jewelry
			craft = AddCraft( 700, typeof( BrilliantAmberBracelet ), 1073107, 1073453, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( Amber ), 1062607, 20, 1044253 );
			craft.AddRes( typeof( BrilliantAmber ), 1026256, 10, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 701, typeof( FireRubyBracelet ), 1073107, 1073454, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( FireRuby ), 1026254, 10, 1044253 );
			craft.AddRes( typeof( Ruby ), 1023859, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 702, typeof( DarkSapphireBracelet ), 1073107, 1073455, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( DarkSapphire ), 1026249, 10, 1044253 );
			craft.AddRes( typeof( Sapphire ), 1023857, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 703, typeof( WhitePearlBracelet ), 1073107, 1073456, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 10, 1044253 );
			craft.AddRes( typeof( Tourmaline ), 1023864, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 704, typeof( EcruCitrineRing ), 1073107, 1073457, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( EcruCitrine ), 1026252, 10, 1044253 );
			craft.AddRes( typeof( Citrine ), 1023861, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 705, typeof( BlueDiamondRing ), 1073107, 1073458, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 10, 1044253 );
			craft.AddRes( typeof( Diamond ), 1023878, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 706, typeof( PerfectEmeraldRing ), 1073107, 1073459, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( Emerald ), 1023856, 20, 1044253 );
			craft.AddRes( typeof( PerfectEmerald ), 1026251, 10, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 707, typeof( TurquoiseRing ), 1073107, 1073460, 75.0, 125.0, typeof( IronIngot ), 1044036, 5, 1044037 );
			craft.AddRes( typeof( Turquoise ), 1026250, 10, 1044253 );
			craft.AddRes( typeof( Amethyst ), 1023862, 20, 1044253 );
			craft.RequiresML = true;

			craft = AddCraft( 710, typeof( ResillientBracer ), 1073107, 1072933, 100.0, 150, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( typeof( CapturedEssence ), 1032686, 1, 1044253 );
			craft.AddRes( typeof( BlueDiamond ), 1026255, 5, 1044253 );
			craft.AddRes( typeof( Diamond ), 1023878, 50, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.AddRecipe( (int) TinkerRecipeGreater.ResilientBracer, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 711, typeof( EssenceOfBattle ), 1073107, 1072935, 100.0, 1500, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( typeof( CapturedEssence ), 1032686, 1, 1044253 );
			craft.AddRes( typeof( FireRuby ), 1026254, 5, 1044253 );
			craft.AddRes( typeof( Ruby ), 1023859, 50, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.AddRecipe( (int) TinkerRecipeGreater.EssenceOfBattle, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;

			craft = AddCraft( 712, typeof( PendantOfTheMagi ), 1073107, 1072937, 100.0, 150, typeof( IronIngot ), 1044036, 2, 1044037 );
			craft.AddRes( typeof( EyeOfTheTravesty ), 1032685, 1, 1044253 );
			craft.AddRes( typeof( WhitePearl ), 1026253, 5, 1044253 );
			craft.AddRes( typeof( StarSapphire ), 1023855, 50, 1044253 );
			craft.ChanceAtMin = 0.5;
			craft.AddRecipe( (int) TinkerRecipeGreater.PendantOfTheMagi, this );
			craft.ForceNonExceptional = true;
			craft.RequiresML = true;
			#endregion

			// Set the overidable material
			SetSubRes( typeof( IronIngot ), 1044022 );

			// Add every material you want the player to be able to chose from
			// This will overide the overidable material
			AddSubRes( typeof( IronIngot ), 1044022, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( DullCopperIngot ), 1044023, 65.0, 1044036, 1044268 );
			AddSubRes( typeof( ShadowIronIngot ), 1044024, 70.0, 1044036, 1044268 );
			AddSubRes( typeof( CopperIngot ), 1044025, 75.0, 1044036, 1044268 );
			AddSubRes( typeof( BronzeIngot ), 1044026, 80.0, 1044036, 1044268 );
			AddSubRes( typeof( GoldIngot ), 1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( AgapiteIngot ), 1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( VeriteIngot ), 1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( ValoriteIngot ), 1044030, 99.0, 1044036, 1044268 );

			MarkOption = true;
			Repair = true;
			CanEnhance = true;
			Alter = true;
		}
	}

	public abstract class TrapCraft : CustomCraft
	{
		private LockableContainer m_Container;

		public LockableContainer Container { get { return m_Container; } }

		public abstract TrapType TrapType { get; }

		public TrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool exceptional )
			: base( from, craftItem, craftSystem, typeRes, tool, exceptional )
		{
		}

		private int Verify( LockableContainer container )
		{
			if ( container == null || container.KeyValue == 0 )
				return 1005638; // You can only trap lockable chests.
			if ( From.Map != container.Map || !From.InRange( container.GetWorldLocation(), 2 ) )
				return 500446; // That is too far away.
			if ( !container.Movable )
				return 502944; // You cannot trap this item because it is locked down.
			if ( !container.IsAccessibleTo( From ) )
				return 502946; // That belongs to someone else.
			if ( container.Locked )
				return 502943; // You can only trap an unlocked object.
			if ( container.TrapType != TrapType.None )
				return 502945; // You can only place one trap on an object at a time.

			return 0;
		}

		private bool Acquire( object target, out int message )
		{
			LockableContainer container = target as LockableContainer;

			message = Verify( container );

			if ( message > 0 )
			{
				return false;
			}
			else
			{
				m_Container = container;
				return true;
			}
		}

		public override void EndCraftAction()
		{
			From.SendLocalizedMessage( 502921 ); // What would you like to set a trap on?
			From.Target = new ContainerTarget( this );
		}

		private class ContainerTarget : Target
		{
			private TrapCraft m_TrapCraft;

			public ContainerTarget( TrapCraft trapCraft )
				: base( -1, false, TargetFlags.None )
			{
				m_TrapCraft = trapCraft;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				int message;

				if ( m_TrapCraft.Acquire( targeted, out message ) )
					m_TrapCraft.CraftItem.CompleteCraft( m_TrapCraft.Exceptional, false, false, m_TrapCraft.From, m_TrapCraft.CraftSystem, m_TrapCraft.TypeRes, m_TrapCraft.Tool, m_TrapCraft );
				else
					Failure( message );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				if ( cancelType == TargetCancelType.Canceled )
					Failure( 0 );
			}

			private void Failure( int message )
			{
				Mobile from = m_TrapCraft.From;
				BaseTool tool = m_TrapCraft.Tool;

				if ( tool != null && !tool.Deleted && tool.UsesRemaining > 0 )
					from.SendGump( new CraftGump( from, m_TrapCraft.CraftSystem, tool, message ) );
				else if ( message > 0 )
					from.SendLocalizedMessage( message );
			}
		}

		public override Item CompleteCraft( out int message )
		{
			message = Verify( this.Container );

			if ( message == 0 )
			{
				int trapLevel = (int) ( From.Skills.Tinkering.Value / 10 );

				Container.TrapType = this.TrapType;
				Container.TrapPower = trapLevel * 9;
				Container.TrapLevel = trapLevel;
				Container.TrapEnabled = false;

				message = 1005639; // Trap is disabled until you lock the chest.
			}

			return null;
		}
	}

	[CraftItemID( 0x1BFC )]
	public class DartTrapCraft : TrapCraft
	{
		public override TrapType TrapType { get { return TrapType.DartTrap; } }

		public DartTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool exceptional )
			: base( from, craftItem, craftSystem, typeRes, tool, exceptional )
		{
		}
	}

	[CraftItemID( 0x113E )]
	public class PoisonTrapCraft : TrapCraft
	{
		public override TrapType TrapType { get { return TrapType.PoisonTrap; } }

		public PoisonTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool exceptional )
			: base( from, craftItem, craftSystem, typeRes, tool, exceptional )
		{
		}
	}

	[CraftItemID( 0x370C )]
	public class ExplosionTrapCraft : TrapCraft
	{
		public override TrapType TrapType { get { return TrapType.ExplosionTrap; } }

		public ExplosionTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool exceptional )
			: base( from, craftItem, craftSystem, typeRes, tool, exceptional )
		{
		}
	}
}