using System;
using System.Collections;
using System.Collections.Generic;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Engines.Craft
{
	public class CraftGump : Gump
	{
		public override int TypeID { get { return 0x1CC; } }

		private Mobile m_From;
		private CraftSystem m_CraftSystem;
		private BaseTool m_Tool;
		private Timer m_MakeTimer;

		private CraftPage m_Page;

		private const int LabelHue = 0x480;
		private const int LabelColor = 0x7FFF;
		private const int FontColor = 0xFFFFFF;

		private enum CraftPage
		{
			None = 0,
			PickResource = 9,
			PickResource2 = 10
		}

		public CraftGump( Mobile from, CraftSystem craftSystem, BaseTool tool, object notice )
			: this( from, craftSystem, tool, notice, CraftPage.None )
		{
		}

		private CraftGump( Mobile from, CraftSystem craftSystem, BaseTool tool, object notice, CraftPage page )
			: base( 50, 50 )
		{
			m_From = from;
			m_CraftSystem = craftSystem;
			m_Tool = tool;
			m_Page = page;

			CraftContext context = craftSystem.GetContext( from );

			from.CloseGump( typeof( CraftGump ) );
			from.CloseGump( typeof( CraftGumpItem ) );

			AddPage( (int) CraftPage.None );

			AddBackground( 0, 0, 530, 497, 5054 );
			AddImageTiled( 10, 10, 510, 22, 2624 );
			AddImageTiled( 10, 292, 150, 45, 2624 );
			AddImageTiled( 165, 292, 355, 45, 2624 );
			AddImageTiled( 10, 342, 510, 145, 2624 );
			AddImageTiled( 10, 37, 200, 250, 2624 );
			AddImageTiled( 215, 37, 305, 250, 2624 );
			AddAlphaRegion( 10, 10, 510, 497 );

			if ( craftSystem.GumpTitleNumber > 0 )
				AddHtmlLocalized( 10, 12, 510, 20, craftSystem.GumpTitleNumber, FontColor, false, false );
			else
				AddHtml( 10, 12, 510, 20, craftSystem.GumpTitleString, false, false );

			AddHtmlLocalized( 10, 302, 150, 25, 1044012, FontColor, false, false ); // <CENTER>NOTICES</CENTER>

			AddButton( 15, 442, 4017, 4019, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 445, 150, 18, 1011441, FontColor, false, false ); // EXIT

			AddButton( 270, 442, 4005, 4007, 1999, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 445, 150, 18, 1044013, FontColor, false, false ); // MAKE LAST

			AddButton( 115, 442, 4017, 4019, 12000, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 150, 445, 150, 18, 1112698, FontColor, false, false ); // CANCEL MAKE

			// Resmelt option
			if ( craftSystem.Resmelt )
			{
				AddButton( 15, 342, 4005, 4007, 7000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 50, 345, 150, 18, 1044259, FontColor, false, false ); // SMELT ITEM
			}
			// ****************************************

			// Repair option
			if ( craftSystem.Repair )
			{
				AddButton( 270, 342, 4005, 4007, 8000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 305, 345, 150, 18, 1044260, FontColor, false, false ); // REPAIR ITEM
			}
			// ****************************************

			// Alter option
			if ( craftSystem.Alter )
			{
				AddButton( 270, 402, 4005, 4007, 9000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 305, 405, 300, 18, 1094726, FontColor, false, false ); // ALTER ITEM (GARGOYLE)
			}
			// ****************************************

			AddHtmlLocalized( 10, 37, 200, 22, 1044010, FontColor, false, false ); // <CENTER>CATEGORIES</CENTER>

			CreateGroupList();

			if ( craftSystem.CraftSubRes.Init )
				CreateResList( false );

			if ( craftSystem.CraftSubRes2.Init )
				CreateResList( true );

			AddPage( 0 );

			// Enhance option
			if ( craftSystem.CanEnhance )
			{
				AddButton( 270, 382, 4005, 4007, 2000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 305, 385, 150, 18, 1061001, FontColor, false, false ); // ENHANCE ITEM
			}
			// ****************************************

			if ( craftSystem.CraftSubRes2.Init )
				AddButton( 15, 382, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 10 );

			if ( craftSystem.CraftSubRes.Init )
				AddButton( 15, 362, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 9 );

			if ( context != null && context.LastGroupIndex > -1 )
				CreateItemList( context.LastGroupIndex );

			// If the system has more than one resource
			if ( craftSystem.CraftSubRes.Init )
			{
				AddPage( 0 );

				string nameString = craftSystem.CraftSubRes.NameString;
				int nameNumber = craftSystem.CraftSubRes.NameNumber;
				int amount;

				int resIndex = ( context == null ? -1 : context.LastResourceIndex );

				if ( resIndex > -1 )
				{
					CraftSubRes subResource = craftSystem.CraftSubRes.GetAt( resIndex );

					nameString = subResource.NameString;
					nameNumber = subResource.NameNumber;
					amount = GetAmount( subResource.ItemType );
				}
				else
					amount = GetAmount( craftSystem.CraftSubRes.ResType );

				if ( nameNumber > 0 )
				{
					AddHtmlLocalized( 50, 365, 170, 18, nameNumber, amount.ToString(), LabelColor, false, false );
					AddKRHtmlLocalized( 50, 365, 170, 18, nameNumber, LabelColor, false, false );
				}
				else
					AddLabel( 50, 362, LabelHue, nameString );

				AddPage( 9 );

				AddButton( 220, 260, 4005, 4007, 3000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 255, 263, 200, 18, ( context == null || !context.DoNotColor ) ? 1061591 : 1061590, LabelColor, false, false );
			}
			// ****************************************

			// For dragon scales
			if ( craftSystem.CraftSubRes2.Init )
			{
				AddPage( 0 );

				string nameString = craftSystem.CraftSubRes2.NameString;
				int nameNumber = craftSystem.CraftSubRes2.NameNumber;
				int amount;

				int resIndex = ( context == null ? -1 : context.LastResourceIndex2 );

				if ( resIndex > -1 )
				{
					CraftSubRes subResource = craftSystem.CraftSubRes2.GetAt( resIndex );

					nameString = subResource.NameString;
					nameNumber = subResource.NameNumber;
					amount = GetAmount( subResource.ItemType );
				}
				else
					amount = GetAmount( craftSystem.CraftSubRes2.ResType );

				if ( nameNumber > 0 )
				{
					AddHtmlLocalized( 50, 385, 150, 18, nameNumber, amount.ToString(), LabelColor, false, false );
					AddKRHtmlLocalized( 50, 385, 150, 18, nameNumber, LabelColor, false, false );
				}
				else
					AddLabel( 50, 385, LabelHue, nameString );

				AddPage( 10 );

				AddButton( 220, 260, 4005, 4007, 3001, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 255, 263, 200, 18, ( context == null || !context.DoNotColor ) ? 1061591 : 1061590, LabelColor, false, false );
			}
			// ****************************************

			// Notice
			if ( notice is int && (int) notice > 0 )
			{
				AddPage( 0 );
				AddHtmlLocalized( 170, 295, 350, 40, (int) notice, LabelColor, false, false );
			}
			else if ( notice is string )
			{
				AddPage( 0 );
				AddHtml( 170, 295, 350, 40, String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", FontColor, notice ), false, false );
			}
			// ****************************************

			// Mark option
			if ( craftSystem.MarkOption )
			{
				AddPage( 0 );

				AddButton( 270, 362, 4005, 4007, 6000, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 305, 365, 150, 18, 1044017 + ( context == null ? 0 : (int) context.MarkOption ), LabelColor, false, false ); // MARK ITEM

				if ( context != null && context.MarkOption == CraftMarkOption.MarkItem )
					AddKRButton( 0, 0, 0, 0, 6001, GumpButtonType.Page, 0 );
			}
			// ****************************************

			// Quest Item option
			bool questItem = ( context != null && context.QuestItem );

			AddPage( 0 );

			AddButton( 270, 422, 0xFA5, 0xFA7, 6002, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 425, 150, 18, questItem ? 1112534 : 1112533, LabelColor, false, false );
			// ****************************************

			if ( context != null )
			{
				if ( context.MakeLast )
					AddKRButton( 0, 0, 0, 0, 6003, GumpButtonType.Page, 0 );

				if ( context.MakeNumber )
					AddKRButton( 0, 0, 0, 0, context.Total + 6100, GumpButtonType.Page, 0 );

				if ( context.MakeMax )
					AddKRButton( 0, 0, 0, 0, 6005, GumpButtonType.Page, 0 );
			}

			if ( context != null )
			{
				if ( context.Success )
					context.Done++;

				int done = 0, total = 1;

				done = context.Done;
				total = context.Total;

				AddHtmlLocalized( 270, 465, 200, 18, 1079443, String.Format( "{0}@{1}", done, total ), FontColor, false, false ); // ~1_DONE~/~2_TOTAL~ COMPLETED

				if ( context.Making != null && context.Success )
					m_MakeTimer = Timer.DelayCall( TimeSpan.FromSeconds( 1.25 ), new TimerCallback( DoMake ) );

				context.Success = false;
			}
		}

		public void CreateResList( bool opt )
		{
			CraftSubResCol res = ( opt ? m_CraftSystem.CraftSubRes2 : m_CraftSystem.CraftSubRes );

			AddPage( (int) ( opt ? CraftPage.PickResource2 : CraftPage.PickResource ) );

			int residx = opt ? 4000 : 5000;

			for ( int i = 0; i < res.Count; ++i )
			{
				int index = i % 10;

				CraftSubRes subResource = res.GetAt( i );

				if ( index == 0 )
				{
					if ( i > 0 )
					{
						AddButton( 485, 260, 4005, 4007, 0, GumpButtonType.Page, ( i / 10 ) + 1 );
						AddButton( 455, 260, 4014, 4015, 0, GumpButtonType.Page, i / 10 );
					}

					CraftContext context = m_CraftSystem.GetContext( m_From );
				}

				AddButton( 220, 60 + ( index * 20 ), 4005, 4007, residx + i, GumpButtonType.Reply, 0 );

				if ( subResource.NameNumber > 0 )
					AddHtmlLocalized( 255, 63 + ( index * 20 ), 150, 18, subResource.NameNumber, GetAmount( subResource.ItemType ).ToString(), FontColor, false, false );
				else
					AddLabel( 255, 60 + ( index * 20 ), LabelHue, subResource.NameString );
			}
		}

		private int GetAmount( Type type )
		{
			if ( m_From.Backpack == null )
			{
				return 0;
			}
			else
			{
				Type[] types = null;

				for ( int i = 0; types == null && i < Craft.CraftItem.TypesTable.Length; ++i )
				{
					if ( Craft.CraftItem.TypesTable[i][0] == type )
						types = Craft.CraftItem.TypesTable[i];
				}

				if ( types == null )
					types = new Type[] { type };

				int amount = 0;

				for ( int i = 0; i < types.Length; i++ )
				{
					Item[] items = m_From.Backpack.FindItemsByType( types[i] );

					for ( int j = 0; j < items.Length; j++ )
						amount += items[j].Amount;
				}

				return amount;
			}
		}

		public void CreateMakeLastList()
		{
			CraftContext context = m_CraftSystem.GetContext( m_From );

			if ( context == null )
				return;

			List<CraftItem> items = context.Items;

			if ( items.Count > 0 )
			{
				for ( int i = 0; i < items.Count; ++i )
				{
					int index = i % 10;

					CraftItem craftItem = items[i];

					if ( index == 0 )
					{
						if ( i > 0 )
						{
							AddButton( 370, 260, 4005, 4007, 0, GumpButtonType.Page, ( i / 10 ) + 1 );
							AddHtmlLocalized( 405, 263, 100, 18, 1044045, LabelColor, false, false ); // NEXT PAGE
						}

						AddPage( ( i / 10 ) + 1 );

						if ( i > 0 )
						{
							AddButton( 220, 260, 4014, 4015, 0, GumpButtonType.Page, i / 10 );
							AddHtmlLocalized( 255, 263, 100, 18, 1044044, LabelColor, false, false ); // PREV PAGE
						}
					}

					AddButton( 220, 60 + ( index * 20 ), 4005, 4007, craftItem.CraftId, GumpButtonType.Reply, 0 );

					if ( craftItem.NameNumber > 0 )
						AddHtmlLocalized( 255, 63 + ( index * 20 ), 220, 18, craftItem.NameNumber, LabelColor, false, false );
					else
						AddLabel( 255, 60 + ( index * 20 ), LabelHue, craftItem.NameString );

					AddButton( 480, 60 + ( index * 20 ), 4011, 4012, craftItem.CraftId + 1000, GumpButtonType.Reply, 0 );
				}
			}
			else // OSI checked
				AddPage( 1 );
		}

		public void CreateItemList( int selectedGroup )
		{
			if ( selectedGroup == 501 ) // 501 : Last 10
			{
				CreateMakeLastList();
				return;
			}

			CraftGroupCol craftGroupCol = m_CraftSystem.CraftGroups;
			CraftGroup craftGroup = craftGroupCol.GetAt( selectedGroup );
			CraftItemCol craftItemCol = craftGroup.CraftItems;

			int i = 0;

			List<CraftItem> items = new List<CraftItem>( craftItemCol.Values );
			items.Sort();

			foreach ( CraftItem craftItem in items )
			{
				int index = i % 10;

				// ********** Next and Previous Buttons **********
				if ( index == 0 )
				{
					if ( i > 0 )
					{
						AddButton( 370, 260, 4005, 4007, 0, GumpButtonType.Page, ( i / 10 ) + 1 );
						AddHtmlLocalized( 405, 263, 100, 18, 1044045, FontColor, false, false ); // NEXT PAGE
					}

					AddPage( ( i / 10 ) + 1 );

					if ( i > 0 )
					{
						AddHtmlLocalized( 255, 263, 100, 18, 1044044, FontColor, false, false ); // PREV PAGE
						AddButton( 220, 260, 4014, 4015, 0, GumpButtonType.Page, i / 10 );
					}
				}
				// ********** End Next and Previous Buttons **********

				AddButton( 220, 60 + ( index * 20 ), 4005, 4007, craftItem.CraftId, GumpButtonType.Reply, 0 );

				if ( craftItem.NameNumber > 0 )
					AddHtmlLocalized( 255, 63 + ( index * 20 ), 220, 18, craftItem.NameNumber, FontColor, false, false );
				else
					AddLabel( 255, 60 + ( index * 20 ), LabelHue, craftItem.NameString );

				AddButton( 480, 60 + ( index * 20 ), 4011, 4012, craftItem.CraftId + 1000, GumpButtonType.Reply, 0 );

				i++;
			}
		}

		public int CreateGroupList()
		{
			CraftGroupCol craftGroupCol = m_CraftSystem.CraftGroups;

			AddButton( 15, 60, 0xFAB, 0xFAC, 9099, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 60, 150, 18, 1044014, FontColor, false, false ); // LAST TEN

			AddHtmlLocalized( 215, 37, 305, 22, 1044011, FontColor, false, false ); // <CENTER>SELECTIONS</CENTER>

			for ( int i = 0; i < craftGroupCol.Count; i++ )
			{
				CraftGroup craftGroup = craftGroupCol.GetAt( i );

				AddButton( 15, 80 + ( i * 20 ), 4005, 4007, 9001 + i, GumpButtonType.Reply, 0 );

				if ( craftGroup.NameNumber > 0 )
					AddHtmlLocalized( 50, 83 + ( i * 20 ), 150, 18, craftGroup.NameNumber, FontColor, false, false );
				else
					AddLabel( 50, 80 + ( i * 20 ), LabelHue, craftGroup.NameString );
			}

			return craftGroupCol.Count;
		}

		public static void CraftItem( CraftItem item, CraftSystem system, Mobile from, BaseTool tool )
		{
			int num = system.CanCraft( from, tool, item.ItemType );
			CraftContext context = system.GetContext( from );

			if ( context == null )
				return;

			if ( num > 0 )
			{
				context.Making = null;

				from.SendGump( new CraftGump( from, system, tool, num ) );
			}
			else
			{
				context.Making = item;

				Type type = null;

				CraftSubResCol res = ( item.UseSubRes2 ? system.CraftSubRes2 : system.CraftSubRes );
				int resIndex = ( item.UseSubRes2 ? context.LastResourceIndex2 : context.LastResourceIndex );

				if ( resIndex >= 0 && resIndex < res.Count )
					type = res.GetAt( resIndex ).ItemType;

				system.CreateItem( from, item.ItemType, type, tool, item );
			}
		}

		public void CraftItem( CraftItem item )
		{
			CraftItem( item, m_CraftSystem, m_From, m_Tool );
		}

		public void DoMake()
		{
			CraftContext context = m_CraftSystem.GetContext( m_From );
			CraftItem item = context.Making;

			if ( item != null )
			{
				m_From.CloseGump( typeof( CraftGump ) );
				m_From.CloseGump( typeof( CraftGumpItem ) );

				CraftItem( item );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_MakeTimer != null )
				m_MakeTimer.Stop();

			if ( info.ButtonID <= 0 )
				return; // Canceled

			int type = info.ButtonID / 1000;
			int index = info.ButtonID % 1000;

			CraftSystem system = m_CraftSystem;
			CraftGroupCol groups = system.CraftGroups;
			CraftContext context = system.GetContext( m_From );

			switch ( type )
			{
				case 0: // Create Item
					{
						if ( context == null )
							break;

						CraftItem item = system.CraftItems.GetAt( index );

						if ( item != null )
							CraftItem( item );

						break;
					}
				case 1: // Item Details && Make Last
					{
						if ( context == null )
							break;

						switch ( index )
						{
							case 999: // Make Last
								{
									CraftItem item = context.LastMade;

									if ( item != null )
										CraftItem( item );
									else
									{
										// You haven't made anything yet.
										m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, 1044165, m_Page ) );
									}

									break;
								}
							default: // Item Details
								{
									CraftItem item = system.CraftItems.GetAt( index );

									if ( item != null )
										m_From.SendGump( new CraftGumpItem( m_From, system, item, m_Tool ) );

									break;
								}
						}

						break;
					}
				case 2: // Enhance Item
					{
						if ( system.CanEnhance )
							Enhance.BeginTarget( m_From, system, m_Tool );

						break;
					}
				case 3: // Toggle use resource hue
					{
						if ( context == null )
							break;

						context.DoNotColor = !context.DoNotColor;

						m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

						break;
					}
				case 4: // 2nd resource selected
					{
						if ( index >= 0 && index < system.CraftSubRes2.Count )
						{
							CraftSubRes res = system.CraftSubRes2.GetAt( index );

							if ( m_From.Skills[system.MainSkill].Base < res.RequiredSkill )
							{
								m_From.SendGump( new CraftGump( m_From, system, m_Tool, res.Message ) );
							}
							else
							{
								if ( context != null )
									context.LastResourceIndex2 = index;

								m_From.SendGump( new CraftGump( m_From, system, m_Tool, null ) );
							}
						}

						break;
					}
				case 5: // Resource selected
					{
						if ( index >= 0 && index < system.CraftSubRes.Count )
						{
							CraftSubRes res = system.CraftSubRes.GetAt( index );

							if ( m_From.Skills[system.MainSkill].Base < res.RequiredSkill )
							{
								m_From.SendGump( new CraftGump( m_From, system, m_Tool, res.Message ) );
							}
							else
							{
								if ( context != null )
									context.LastResourceIndex = index;

								m_From.SendGump( new CraftGump( m_From, system, m_Tool, null ) );
							}
						}

						break;
					}
				case 6: // Craft system features
					{
						if ( context == null )
							break;

						switch ( index )
						{
							case 0: // Toggle mark option
								{
									if ( !system.MarkOption )
										break;

									switch ( context.MarkOption )
									{
										case CraftMarkOption.MarkItem:
											context.MarkOption = CraftMarkOption.DoNotMark;
											break;
										case CraftMarkOption.DoNotMark:
											context.MarkOption = CraftMarkOption.PromptForMark;
											break;
										case CraftMarkOption.PromptForMark:
											context.MarkOption = CraftMarkOption.MarkItem;
											break;
									}

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							case 1: // Toggle mark option (KR Version)
								{
									if ( !system.MarkOption )
										break;

									switch ( context.MarkOption )
									{
										case CraftMarkOption.PromptForMark:
										case CraftMarkOption.MarkItem:
											context.MarkOption = CraftMarkOption.DoNotMark;
											break;
										case CraftMarkOption.DoNotMark:
											context.MarkOption = CraftMarkOption.MarkItem;
											break;
									}

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							case 2: // Toggle quest item option
								{
									context.QuestItem = !context.QuestItem;

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							case 3: // Make last
								{
									context.MakeLast = !context.MakeLast;

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							case 4: // Reset Make number
								{
									context.Total = 1;

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							case 5: // Make Max
								{
									if ( !context.MakeMax )
										context.Total = 9999;
									else
									{
										context.Total = 1;
										context.Making = null;
									}

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
							default: // Make number
								{
									int amount = index - 100;
									Utility.FixMinMax( ref amount, 1, 100 );

									context.Total = amount;

									m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

									break;
								}
						}

						break;
					}
				case 7: // Smelt item
					{
						if ( system.Resmelt )
							Resmelt.Do( m_From, system, m_Tool );

						break;
					}
				case 8: // Repair item
					{
						if ( system.Repair )
							Repair.Do( m_From, system, m_Tool );

						break;
					}
				case 9: // Show group && Last 10
					{
						if ( context == null )
							break;

						switch ( index )
						{
							case 0: // Alter Item (Gargoyle)
								{
									if ( system.Alter )
										Alter.Do( m_From, system, m_Tool );

									break;
								}
							case 99: // Last 10
								{
									context.LastGroupIndex = 501;
									m_From.SendGump( new CraftGump( m_From, system, m_Tool, null ) );

									break;
								}
							default: // Show group
								{
									index--;

									if ( index >= 0 && index < groups.Count )
									{
										context.LastGroupIndex = index;
										m_From.SendGump( new CraftGump( m_From, system, m_Tool, null ) );
									}

									break;
								}
						}

						break;
					}
				case 12: // Cancel Make
					{
						context.Total = 1;

						m_From.SendLocalizedMessage( 501806 ); // Request cancelled.
						m_From.SendGump( new CraftGump( m_From, m_CraftSystem, m_Tool, null, m_Page ) );

						break;
					}
			}
		}
	}
}
