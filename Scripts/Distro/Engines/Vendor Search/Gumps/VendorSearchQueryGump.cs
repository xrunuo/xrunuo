using System;
using System.Linq;
using System.Threading.Tasks;
using Server;
using Server.Engines.VendorSearch;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
	public class VendorSearchQueryGump : Gump
	{
		private Mobile m_Mobile;
		private SearchCriteria m_Criteria;
		private SearchCriterion[] m_SelectedCriteria;

		public VendorSearchQueryGump( Mobile m )
			: this( m, SearchCriteria.GetSearchCriteriaForPlayer( m ) )
		{
		}

		public VendorSearchQueryGump( Mobile m, SearchCriteria criteria, int feedback = -1 )
			: base( 10, 10 )
		{
			m_Mobile = m;
			m_Criteria = criteria;
			m_SelectedCriteria = criteria.GetSelectedCriteria().ToArray();

			AddPage( 0 );

			AddBackground( 0, 0, 780, 550, 0x7752 );
			AddBackground( 10, 50, 246, 22, 0x2486 );

			AddHtmlLocalized( 10, 10, 760, 18, 1114513, "#1154508", 0x4BBD, false, false ); // <DIV ALIGN=CENTER><center>Vendor Search Query</center></DIV>

			AddHtmlLocalized( 522, 30, 246, 18, 1154546, 0x4BBD, false, false ); // Selected Search Criteria

			int yOffset = 0;

			if ( m_Criteria.HasName )
			{
				AddHtmlLocalized( 562, 50 + ( yOffset * 22 ), 246, 18, 1154510, 0x6B55, false, false ); // Item Name
				AddButton( 522, 50 + ( yOffset * 22 ), 0xFB1, 0xFB2, 4, GumpButtonType.Reply, 0 );
				AddTooltip( 1154694 ); // Remove Selected Search Criteria
				yOffset++;
			}

			if ( m_Criteria.HasPrice )
			{
				AddHtmlLocalized( 562, 50 + ( yOffset * 22 ), 246, 18, 1154512, string.Format( "{0}\t{1}", m_Criteria.MinPrice.ToString( "N0" ), m_Criteria.MaxPrice.ToString( "N0" ) ), 0x6B55, false, false ); // Price Range (~1_INT~ to ~2_INT~)
				AddButton( 522, 50 + ( yOffset * 22 ), 0xFB1, 0xFB2, 5, GumpButtonType.Reply, 0 );
				AddTooltip( 1154694 ); // Remove Selected Search Criteria
				yOffset++;
			}

			for ( int i = 0; i < m_SelectedCriteria.Length; i++ )
			{
				SearchCriterion sc = m_SelectedCriteria[i];

				string args = sc.LabelArgs;

				if ( args == null )
					AddHtmlLocalized( 562, 50 + ( yOffset * 22 ), 246, 18, sc.LabelNumber, 0x6B55, false, false );
				else
					AddHtmlLocalized( 562, 50 + ( yOffset * 22 ), 246, 18, sc.LabelNumber, args, 0x6B55, false, false );

				AddButton( 522, 50 + ( yOffset * 22 ), 0xFB1, 0xFB2, 50 + i, GumpButtonType.Reply, 0 );
				AddTooltip( 1154694 ); // Remove Selected Search Criteria

				yOffset++;
			}

			AddHtmlLocalized( 10, 30, 246, 18, 1154510, 0x4BBD, false, false ); // Item Name
			AddTextEntry( 12, 52, 242, 18, 0x9C2, 1, m_Criteria.Name, 25 );

			string priceArgs = String.Format( "{0}\t{1}", m_Criteria.MinPrice.ToString( "N0" ), m_Criteria.MaxPrice.ToString( "N0" ) );
			AddButton( 10, 74, 0x7745, 0x7745, 0, GumpButtonType.Page, 2 );
			AddHtmlLocalized( 50, 74, 206, 20, 1154512, priceArgs, 0x4BBD, false, false ); // Price Range (~1_INT~ to ~2_INT~)

			var categories = SearchCriteriaCategory.AllCategories;

			for ( int i = 0; i < categories.Length; i++ )
			{
				SearchCriteriaCategory category = categories[i];

				AddButton( 10, 96 + ( i * 22 ), 0x7745, 0x7745, 0, GumpButtonType.Page, i + 3 );
				AddHtmlLocalized( 50, 96 + ( i * 22 ), 206, 20, category.Cliloc, 0x4BBD, false, false );
			}

			if ( feedback != -1 )
				AddHtmlLocalized( 110, 520, 660, 20, feedback, 0x7C00, false, false );

			AddButton( 10, 520, 0x7747, 0x7747, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 520, 50, 20, 1150300, 0x4BBD, false, false ); // CANCEL

			AddButton( 740, 520, 0x7746, 0x7746, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 630, 520, 100, 20, 1114514, "#1154641", 0x4BBD, false, false ); // <DIV ALIGN=RIGHT>Search</DIV>

			AddButton( 740, 500, 0x7745, 0x7745, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 630, 500, 100, 20, 1114514, "#1154588", 0x4BBD, false, false ); // <DIV ALIGN=RIGHT>Clear Search Criteria</DIV>

			AddPage( 2 );

			AddHtmlLocalized( 266, 30, 246, 18, 1154532, 0x4BBD, false, false ); // Minimum Price
			AddBackground( 266, 50, 246, 22, 0x2486 );
			AddTextEntry( 268, 52, 242, 18, 0x9C2, 2, m_Criteria.MinPrice.ToString(), 10 );

			AddHtmlLocalized( 266, 74, 246, 18, 1154533, 0x4BBD, false, false ); // Maximum Price
			AddBackground( 266, 94, 246, 22, 0x2486 );
			AddTextEntry( 268, 96, 242, 18, 0x9C2, 3, m_Criteria.MaxPrice.ToString(), 10 );

			AddButton( 266, 118, 0xFAB, 0xFAC, 3, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 306, 118, 100, 272, 1154534, 0x4BBD, false, false ); // Add Search Criteria

			for ( int i = 0; i < categories.Length; i++ )
			{
				SearchCriteriaCategory category = categories[i];

				int categoryIdx = 100 * ( i + 1 );

				AddPage( i + 3 );
				AddHtmlLocalized( 266, 30, 246, 18, category.Cliloc, 0x4BBD, false, false );

				for ( int j = 0; j < category.Criteria.Length; j++ )
				{
					SearchCriterionEntry criterion = category.Criteria[j];
					int criterionIdx = categoryIdx + j;

					AddButton( 266, 50 + ( j * 22 ), 0x7745, 0x7745, criterionIdx, GumpButtonType.Reply, 0 );
					AddHtmlLocalized( 306, 50 + ( j * 22 ), 246, 18, criterion.Cliloc, 0x4BBD, false, false );

					string value = m_Criteria.GetSelectedValue( criterion.Criterion );

					if ( value != null )
					{
						AddBackground( 482, 50 + ( j * 22 ), 30, 20, 0x2486 );
						AddTextEntry( 484, 52 + ( j * 22 ), 26, 16, 0x9C2, criterionIdx, value, 3 );
					}
				}
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( info.ButtonID != 0 )
			{
				// Always update name
				m_Criteria.Name = info.GetTextEntry( 1 ).Text;
			}

			switch ( info.ButtonID )
			{
				case 0: // Cancel
					{
						break;
					}
				case 1: // Search
					{
						if ( m_Criteria.Count == 0 )
						{
							Resend( 1154586 ); // Please select some criteria to search for.
							break;
						}

						StartSearch( m_Mobile, m_Criteria, 0 );

						break;
					}
				case 2: // Clear search criteria
					{
						m_Criteria.Clear();
						Resend();

						break;
					}
				case 3: // Submit price criteria
					{
						m_Criteria.MinPrice = Utility.ToInt32( info.GetTextEntry( 2 ).Text );
						m_Criteria.MaxPrice = Utility.ToInt32( info.GetTextEntry( 3 ).Text );
						Resend();

						break;
					}
				case 4: // Reset Item Name
					{
						m_Criteria.Name = "";
						Resend();

						break;
					}
				case 5: // Reset Item Price
					{
						m_Criteria.MinPrice = SearchCriteria.DefaultMinPrice;
						m_Criteria.MaxPrice = SearchCriteria.DefaultMaxPrice;
						Resend();

						break;
					}
				default:
					{
						if ( info.ButtonID < 100 )
						{
							int criterionIdx = ( info.ButtonID - 50 );

							if ( criterionIdx >= 0 && criterionIdx < m_SelectedCriteria.Length )
							{
								SearchCriterion criterion = m_SelectedCriteria[criterionIdx];
								m_Criteria.Remove( criterion );
							}
						}
						else
						{
							if ( m_Criteria.Count >= SearchCriteria.MaxCount )
							{
								Resend( 1154681 ); // You may not add any more search criteria items.
								break;
							}

							int categoryIdx = ( info.ButtonID / 100 ) - 1;
							int criterionIdx = ( info.ButtonID % 100 );

							var categories = SearchCriteriaCategory.AllCategories;

							if ( categoryIdx >= 0 && categoryIdx < categories.Length )
							{
								SearchCriteriaCategory category = categories[categoryIdx];

								if ( criterionIdx >= 0 && criterionIdx < category.Criteria.Length )
								{
									SearchCriterionEntry entry = category.Criteria[criterionIdx];

									SearchCriterion sc = entry.Criterion.Clone();

									if ( sc is ValuedSearchCriterion )
										( (ValuedSearchCriterion) sc ).Value = Utility.ToInt32( info.GetTextEntry( info.ButtonID ).Text );

									m_Criteria.Add( sc );
								}
							}
						}

						Resend();

						break;
					}
			}
		}

		public static void StartSearch( Mobile from, SearchCriteria criteria, int page )
		{
			if ( !IsInValidRegion( from ) )
			{
				// Before using vendor search, you must be in a justice region or a safe log-out location (such as an inn or a house which has you on its Owner, Co-owner, or Friends list).
				from.SendLocalizedMessage( 1154680 );
				return;
			}

			var resultsTask = VendorItemFinder.Instance.FindVendorItemsAsync( criteria, page );

			var pollingTimer = new TaskPollingTimer<IVendorSearchItem[]>( resultsTask, ( results ) =>
			{
				from.CloseGump( typeof( VendorSearchQueryGump ) );
				from.CloseGump( typeof( VendorSearchResultsGump ) );
				from.CloseGump( typeof( VendorSearchWaitGump ) );

				if ( results.Any() )
				{
					from.SendGump( new VendorSearchQueryGump( from, criteria ) );
					from.SendGump( new VendorSearchResultsGump( results, criteria, page ) );
				}
				else
				{
					from.SendGump( new VendorSearchQueryGump( from, criteria, 1154587 ) ); // No items matched your search.
				}
			} );

			resultsTask.Start();
			pollingTimer.Start();

			from.SendGump( new VendorSearchWaitGump( pollingTimer ) );
		}

		private void Resend( int feedback = -1 )
		{
			m_Mobile.SendGump( new VendorSearchQueryGump( m_Mobile, m_Criteria, feedback ) );
		}

		public static bool IsInValidRegion( Mobile m )
		{
			if ( m.Region == null )
				return false;

			return m.Region.GetLogoutDelay( m ) == TimeSpan.Zero || m.Region.IsPartOf<GuardedRegion>();
		}
	}
}
