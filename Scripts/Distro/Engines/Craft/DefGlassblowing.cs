using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefGlassblowing : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefGlassblowing();
		}

		public override SkillName MainSkill { get { return SkillName.Alchemy; } }

		public override int GumpTitleNumber
		{
			get { return 1044622; } // <CENTER>Glassblowing MENU</CENTER>
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

		private DefGlassblowing()
			: base( 1, 1, 1.25 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !( from is PlayerMobile && ( (PlayerMobile) from ).Glassblowing && from.Skills[SkillName.Alchemy].Base >= 100.0 ) )
				return 1044634; // You havent learned glassblowing.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			bool anvil, forge;

			DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( forge )
				return 0;

			return 1044628; // You must be near a forge to blow glass.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x2B ); // bellows
		}

		/// <summary>
		/// Delay to synchronize the sound with the hit on the anvil
		/// </summary>
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
				m_From.PlaySound( 0x2A );
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
				from.PlaySound( 0x41 ); // glass breaking

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

			#region Miscellaneous
			craft = AddCraft( 1, typeof( Bottle ), 1044050, 1023854, 52.5, 102.5, typeof( Sand ), 1044625, 1, 1044627 );
			craft.UseAllRes = true;

			AddCraft( 2, typeof( SmallFlask ), 1044050, 1044610, 52.5, 102.5, typeof( Sand ), 1044625, 2, 1044627 );
			AddCraft( 3, typeof( MediumFlask ), 1044050, 1044611, 52.5, 102.5, typeof( Sand ), 1044625, 3, 1044627 );
			AddCraft( 4, typeof( CurvedFlask ), 1044050, 1044612, 55.0, 105.0, typeof( Sand ), 1044625, 2, 1044627 );
			AddCraft( 5, typeof( LongFlask ), 1044050, 1044613, 57.5, 107.5, typeof( Sand ), 1044625, 4, 1044627 );
			AddCraft( 6, typeof( LargeFlask ), 1044050, 1044623, 60.0, 110.0, typeof( Sand ), 1044625, 5, 1044627 );
			AddCraft( 7, typeof( AniSmallBlueFlask ), 1044050, 1044614, 60.0, 110.0, typeof( Sand ), 1044625, 5, 1044627 );
			AddCraft( 8, typeof( AniLargeVioletFlask ), 1044050, 1044615, 60.0, 110.0, typeof( Sand ), 1044625, 5, 1044627 );
			AddCraft( 9, typeof( AniRedRibbedFlask ), 1044050, 1044624, 60.0, 110.0, typeof( Sand ), 1044625, 7, 1044627 );
			AddCraft( 10, typeof( EmptyVialsWRack ), 1044050, 1044616, 65.0, 115.0, typeof( Sand ), 1044625, 8, 1044627 );
			AddCraft( 11, typeof( FullVialsWRack ), 1044050, 1044617, 65.0, 115.0, typeof( Sand ), 1044625, 9, 1044627 );
			AddCraft( 12, typeof( SpinningHourglass ), 1044050, 1044618, 75.0, 125.0, typeof( Sand ), 1044625, 10, 1044627 );
			AddCraft( 13, typeof( HollowPrism ), 1044050, 1032125, 100.0, 100.0, typeof( Sand ), 1044625, 8, 1044627 );

			craft = AddCraft( 16, typeof( TerMurStyleFloorMirror ), 1044050, 1095326, 75.0, 125.0, typeof( Sand ), 1044625, 20, 1044627 );
			craft.RequiresSA = true;

			craft = AddCraft( 17, typeof( TerMurStyleWallMirror ), 1044050, 1095324, 70.0, 120.0, typeof( Sand ), 1044625, 10, 1044627 );
			craft.RequiresSA = true;

			craft = AddCraft( 18, typeof( CraftableSoulstone ), 1111745, 1071000, 70.0, 120.0, typeof( CrystalGranules ), 1112329, 2, 1044253 );
			craft.AddRes( typeof( VoidEssence ), 1112327, 2, 1044253 );
			craft.RequiresSA = true;

			craft = AddCraft( 19, typeof( EmptyVenomVial ), 1044050, 1112215, 52.5, 102.5, typeof( Sand ), 1044625, 1, 1044627 );
			craft.RequiresSA = true;
			#endregion

			#region Glass Weapons
			craft = AddCraft( 14, typeof( GlassSword ), 1111745, 1095371, 55.0, 105.0, typeof( Sand ), 1044625, 14, 1044627 );
			craft.RequiresSA = true;

			craft = AddCraft( 15, typeof( GlassStaff ), 1111745, 1095368, 53.6, 103.6, typeof( Sand ), 1044625, 10, 1044627 );
			craft.RequiresSA = true;
			#endregion
		}
	}
}