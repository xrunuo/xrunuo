using System;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefCartography : CraftSystem
	{
		public static void Initialize()
		{
			m_CraftSystem = new DefCartography();
		}

		public override SkillName MainSkill { get { return SkillName.Cartography; } }

		public override int GumpTitleNumber
		{
			get { return 1044008; } // <CENTER>CARTOGRAPHY MENU</CENTER>
		}

		public override double DefaultChanceAtMin
		{
			get { return 0.0; }
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get { return m_CraftSystem; }
		}

		private DefCartography()
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
			from.PlaySound( 0x249 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, bool exceptional, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 );  // You have worn out your tool

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
			#region Maps

			AddCraft( 1, typeof( LocalMap ), 1044448, 1015230, 10.0, 70.0, typeof( BlankMap ), 1044449, 1, 1044450 );
			AddCraft( 2, typeof( CityMap ), 1044448, 1015231, 25.0, 85.0, typeof( BlankMap ), 1044449, 1, 1044450 );
			AddCraft( 3, typeof( SeaChart ), 1044448, 1015232, 35.0, 95.0, typeof( BlankMap ), 1044449, 1, 1044450 );
			AddCraft( 4, typeof( WorldMap ), 1044448, 1015233, 50.0, 99.5, typeof( BlankMap ), 1044449, 1, 1044450 );

			// TODO: 
			// 5	1072891	tattered wall map (south)
			// 6	1072892	tattered wall map (east)
			#endregion
		}
	}
}