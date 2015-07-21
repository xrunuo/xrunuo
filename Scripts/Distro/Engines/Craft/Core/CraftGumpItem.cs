using System;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Engines.Craft
{
	public class CraftGumpItem : Gump
	{
		public override int TypeID { get { return 0x2AD; } }

		private Mobile m_From;
		private CraftSystem m_CraftSystem;
		private CraftItem m_CraftItem;
		private BaseTool m_Tool;

		private const int LabelHue = 0x481; // 0x384
		private const int RedLabelHue = 0x20;
		private const int LabelColor = 0x7FFF;
		private const int RedLabelColor = 0x6400;
		private const int GreyLabelColor = 0x3DEF;

		private int m_OtherCount;

		public CraftGumpItem( Mobile from, CraftSystem craftSystem, CraftItem craftItem, BaseTool tool )
			: base( 50, 50 )
		{
			m_From = from;
			m_CraftSystem = craftSystem;
			m_CraftItem = craftItem;
			m_Tool = tool;

			from.CloseGump( typeof( CraftGump ) );
			from.CloseGump( typeof( CraftGumpItem ) );

			AddPage( 0 );
			AddBackground( 0, 0, 530, 417, 5054 );
			AddImageTiled( 10, 10, 510, 22, 2624 );
			AddImageTiled( 10, 37, 150, 148, 2624 );
			AddImageTiled( 165, 37, 355, 90, 2624 );
			AddImageTiled( 10, 190, 155, 22, 2624 );
			AddImageTiled( 10, 237, 155, 53, 2624 );
			AddImageTiled( 165, 132, 355, 80, 2624 );
			AddImageTiled( 10, 275, 155, 22, 2624 );
			AddImageTiled( 10, 322, 155, 53, 2624 );
			AddImageTiled( 165, 217, 355, 80, 2624 );
			AddImageTiled( 10, 360, 155, 22, 2624 );
			AddImageTiled( 165, 302, 355, 80, 2624 );
			AddImageTiled( 10, 387, 510, 22, 2624 );
			AddAlphaRegion( 10, 10, 510, 397 );

			AddHtmlLocalized( 170, 40, 150, 20, 1044053, LabelColor, false, false ); // ITEM
			AddHtmlLocalized( 10, 215, 150, 22, 1044055, LabelColor, false, false ); // <CENTER>MATERIALS</CENTER>
			AddHtmlLocalized( 10, 300, 150, 22, 1044056, LabelColor, false, false ); // <CENTER>OTHER</CENTER>

			if ( craftSystem.GumpTitleNumber > 0 )
				AddHtmlLocalized( 10, 12, 510, 20, craftSystem.GumpTitleNumber, LabelColor, false, false );
			else
				AddHtml( 10, 12, 510, 20, craftSystem.GumpTitleString, false, false );

			AddButton( 15, 387, 4014, 4016, 9999, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 390, 150, 18, 1044150, LabelColor, false, false ); // BACK

			// Draw Item
			Type type = m_CraftItem.ItemType;

			if ( m_CraftItem.IsMarkable( type ) )
			{
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1044059, LabelColor, false, false ); // This item may hold its maker's mark
				m_ShowExceptionalChance = true;
			}

			if ( craftItem.NameNumber > 0 )
				AddHtmlLocalized( 330, 40, 180, 18, craftItem.NameNumber, LabelColor, false, false );
			else
				AddLabel( 330, 40, LabelHue, craftItem.NameString );

			AddItem( 20, 50, CraftItem.ItemIDOf( type ) );
			// ***********************************

			// Draw Skills
			for ( int i = 0; i < m_CraftItem.Skills.Count; i++ )
			{
				CraftSkill skill = m_CraftItem.Skills.GetAt( i );
				double minSkill = skill.MinSkill;

				if ( minSkill < 0 )
					minSkill = 0;

				AddHtmlLocalized( 170, 132 + ( i * 20 ), 200, 18, 1044060 + (int) skill.SkillToMake, LabelColor, false, false );
				AddLabel( 430, 132 + ( i * 20 ), LabelHue, String.Format( "{0:F1}", minSkill ).Replace( ",", "." ) );
			}
			// ***********************************

			// Draw Ressource
			bool retainedColor = false;

			CraftContext context = m_CraftSystem.GetContext( m_From );

			CraftSubResCol res = ( m_CraftItem.UseSubRes2 ? m_CraftSystem.CraftSubRes2 : m_CraftSystem.CraftSubRes );
			int resIndex = -1;

			if ( context != null )
				resIndex = ( m_CraftItem.UseSubRes2 ? context.LastResourceIndex2 : context.LastResourceIndex );

			bool cropScroll = ( m_CraftItem.Ressources.Count > 1 ) && m_CraftItem.Ressources.GetAt( m_CraftItem.Ressources.Count - 1 ).ItemType == typeofBlankScroll && typeofSpellScroll.IsAssignableFrom( m_CraftItem.ItemType );

			for ( int i = 0; i < m_CraftItem.Ressources.Count - ( cropScroll ? 1 : 0 ) && i < 4; i++ )
			{
				Type _type;
				string nameString;
				int nameNumber;

				CraftRes craftResource = m_CraftItem.Ressources.GetAt( i );

				_type = craftResource.ItemType;
				nameString = craftResource.NameString;
				nameNumber = craftResource.NameNumber;
				int amount = craftResource.Amount;

				// Resource Mutation
				if ( _type == res.ResType && resIndex > -1 )
				{
					CraftSubRes subResource = res.GetAt( resIndex );

					_type = subResource.ItemType;

					nameString = subResource.NameString;
					nameNumber = subResource.GenericNameNumber;

					if ( nameNumber <= 0 )
						nameNumber = subResource.NameNumber;
				}
				// ******************

				if ( m_CraftItem.NameNumber == 1044458 ) // cut-up cloth
				{
					amount = 0;

					AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 360, 18, 1044460, LabelColor, false, false ); // Cut bolts of cloth into pieces of ready cloth.
					AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1048176, LabelColor, false, false ); // Makes as many as possible at once
				}

				if ( m_CraftItem.NameNumber == 1044459 ) //combine cloth
				{
					amount = 0;

					AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 360, 18, 1044461, LabelColor, false, false ); // Combine available cloth into piles by color.
					AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1048176, LabelColor, false, false ); // Makes as many as possible at once
				}

				if ( !retainedColor && m_CraftItem.RetainsColorFrom( m_CraftSystem, _type ) )
				{
					retainedColor = true;
					AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1044152, LabelColor, false, false ); // * The item retains the color of this material
					AddLabel( 500, 219 + ( i * 20 ), LabelHue, "*" );
				}

				if ( nameNumber > 0 )
					AddHtmlLocalized( 170, 219 + ( i * 20 ), 310, 18, nameNumber, LabelColor, false, false );
				else
					AddLabel( 170, 219 + ( i * 20 ), LabelHue, nameString );

				AddHtml( 430, 219 + ( i * 20 ), 40, 20, String.Format( "<basefont color=#FFFFFF><div align=right>{0}</basefont></div>", amount.ToString() ), false, false );
			}

			if ( m_CraftItem.NameNumber == 1041267 ) // runebook
			{
				AddHtmlLocalized( 170, 219 + ( m_CraftItem.Ressources.Count * 20 ), 310, 18, 1044447, LabelColor, false, false );
				AddHtml( 430, 219 + ( m_CraftItem.Ressources.Count * 20 ), 40, 20, String.Format( "<basefont color=#FFFFFF><div align=right>{0}</basefont></div>", "1" ), false, false );
			}
			// ***********************************

			if ( craftItem.UseAllRes )
			{
				// Makes as many as possible at once
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1048176, LabelColor, false, false );
			}

			if ( craftItem.RequiresSE )
			{
				// * Requires the "Samurai Empire" expansion
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1063363, LabelColor, false, false );
			}

			if ( craftItem.RequiresML )
			{
				// * Requires the "Mondain's Legacy" expansion
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1072651, LabelColor, false, false );
			}

			if ( craftItem.RequiresSA )
			{
				// * Requires the "Stygian Abyss" expansion
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1094732, LabelColor, false, false );
			}

			if ( craftItem.RequiresHS )
			{
				// * Requires the "High Seas" booster
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1116296, LabelColor, false, false );
			}

			bool needsRecipe = ( craftItem.Recipe != null && from is PlayerMobile && !( (PlayerMobile) from ).HasRecipe( craftItem.Recipe ) );

			if ( needsRecipe )
			{
				// You have not learned this recipe.
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 310, 18, 1073620, RedLabelColor, false, false );
			}

			if ( cropScroll )
			{
				// Inscribing scrolls also requires a blank scroll and mana.
				AddHtmlLocalized( 170, 302 + ( m_OtherCount++ * 20 ), 360, 18, 1044379, LabelColor, false, false );
			}

			if ( needsRecipe )
			{
				AddButton( 135, 387, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 0 );
				AddHtmlLocalized( 180, 390, 150, 18, 1112624, 0x3DEF, false, false ); // MAKE MAX

				AddButton( 255, 387, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 0 );
				AddHtmlLocalized( 300, 390, 150, 18, 1112623, 0x3DEF, false, false ); // MAKE NUMBER

				AddButton( 405, 387, 4005, 4007, 0, GumpButtonType.Page, 0 );
				AddHtmlLocalized( 450, 390, 150, 18, 1044151, GreyLabelColor, false, false ); // MAKE NOW
			}
			else
			{
				AddButton( 135, 387, 0xFAE, 0xFB0, craftItem.CraftId + 11000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 180, 390, 150, 18, 1112624, 0x7FFF, false, false ); // MAKE MAX

				AddButton( 255, 387, 0xFAE, 0xFB0, craftItem.CraftId + 10000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 300, 390, 150, 18, 1112623, 0x7FFF, false, false ); // MAKE NUMBER

				AddButton( 405, 387, 4005, 4007, craftItem.CraftId, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 450, 390, 150, 18, 1044151, LabelColor, false, false ); // MAKE NOW
			}

			// Draw Chances
			DrawChances();
			// ***********************************
		}

		private bool m_ShowExceptionalChance;

		public void DrawChances()
		{
			CraftSubResCol res = ( m_CraftItem.UseSubRes2 ? m_CraftSystem.CraftSubRes2 : m_CraftSystem.CraftSubRes );
			int resIndex = -1;

			CraftContext context = m_CraftSystem.GetContext( m_From );

			if ( context != null )
				resIndex = ( m_CraftItem.UseSubRes2 ? context.LastResourceIndex2 : context.LastResourceIndex );

			bool allRequiredSkills = true;
			double chance = m_CraftItem.GetSuccessChance( m_From, resIndex > -1 ? res.GetAt( resIndex ).ItemType : null, m_CraftSystem, false, ref allRequiredSkills );
			double excepChance = m_CraftItem.GetExceptionalChance( m_CraftSystem, chance, m_From );

			if ( chance > 0.0 )
				chance += m_CraftItem.GetTalismanBonus( m_From, m_CraftSystem );

			if ( chance < 0.0 )
				chance = 0.0;
			else if ( chance > 1.0 )
				chance = 1.0;

			if ( excepChance < 0.0 )
				excepChance = 0.0;
			else if ( excepChance > 1.0 )
				excepChance = 1.0;

			AddHtmlLocalized( 170, 80, 250, 18, 1044057, LabelColor, false, false ); // Success Chance:
			AddLabel( 430, 80, LabelHue, String.Format( "{0:F1}%", chance * 100 ).Replace( ",", "." ) );

			if ( m_ShowExceptionalChance )
			{
				AddHtmlLocalized( 170, 100, 250, 18, 1044058, 32767, false, false ); // Exceptional Chance:
				AddLabel( 430, 100, LabelHue, String.Format( "{0:F1}%", excepChance * 100 ).Replace( ",", "." ) );
			}
		}

		private static Type typeofBlankScroll = typeof( BlankScroll );
		private static Type typeofSpellScroll = typeof( SpellScroll );

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			int typeId = info.ButtonID / 1000;
			int index = info.ButtonID % 1000;

			CraftContext context = m_CraftSystem.GetContext( m_From );

			if ( context == null )
				return;

			if ( info.ButtonID == 9999 ) // Back Button
			{
				CraftGump craftGump = new CraftGump( m_From, m_CraftSystem, m_Tool, null );
				m_From.SendGump( craftGump );
			}
			else if ( typeId == 10 ) // Make Number
			{
				m_From.Prompt = new MakeAmountPrompt( m_CraftSystem, m_CraftItem, m_Tool );
			}
			else
			{
				if ( typeId == 11 ) // Make Max
					context.Total = 9999;

				if ( info.ButtonID != 0 ) // Make Button
					CraftGump.CraftItem( m_CraftItem, m_CraftSystem, m_From, m_Tool );
			}
		}
	}

	public class MakeAmountPrompt : Prompt
	{
		// Please type the amount you wish to create(1 - 100): <Escape to cancel>
		public override int MessageCliloc { get { return 1112576; } }

		private CraftItem m_CraftItem;
		private CraftSystem m_CraftSystem;
		private BaseTool m_Tool;

		public MakeAmountPrompt( CraftSystem craftSystem, CraftItem craft, BaseTool tool )
			: base( tool )
		{
			m_CraftSystem = craftSystem;
			m_CraftItem = craft;
			m_Tool = tool;
		}

		public override void OnCancel( Mobile from )
		{
			from.SendLocalizedMessage( 501806 ); // Request cancelled.

			from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, null ) );
		}

		public override void OnResponse( Mobile from, string text )
		{
			int amount = Utility.ToInt32( text );
			CraftContext context = m_CraftSystem.GetContext( from );

			if ( amount < 1 || amount > 100 )
			{
				from.SendLocalizedMessage( 1112587 ); // Invalid entry.
				from.SendLocalizedMessage( 501806 ); // Request cancelled.

				context.Total = 1;

				from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, null ) );
			}
			else
			{
				context.Total = amount;

				context.Making = m_CraftItem;
				CraftGump.CraftItem( m_CraftItem, m_CraftSystem, from, m_Tool );
			}
		}
	}
}
