using System;
using System.Collections;
using Server.Guilds;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class DiplomacyGump : Gump
	{
		public override int TypeID { get { return 0x2D9; } }

		protected Mobile m_Mobile;
		protected Guild m_Guild;
		protected ArrayList m_List;
		protected ArrayList m_SortAllyList;
		protected ArrayList m_SortWarList;
		protected bool updown;
		private int m_Page;
		private int m_SortType;
		private string m_Search;

		public DiplomacyGump( Mobile from, Guild guild, ArrayList list, int Sort_Type )
			: this( from, guild, list, Sort_Type, 0, "" )
		{
		}

		public DiplomacyGump( Mobile from, Guild guild, ArrayList list, int Sort_Type, string search )
			: this( from, guild, list, Sort_Type, 0, search )
		{
		}

		public DiplomacyGump( Mobile from, Guild guild, ArrayList list, int Sort_Type, int page, string search )
			: base( 10, 10 )
		{
			m_Mobile = from;
			m_Guild = guild;
			m_Page = page;
			m_Search = search;
			m_SortType = Sort_Type;

			if ( Sort_Type >= 10 )
			{
				updown = true;
				Sort_Type = Sort_Type - 10;
			}
			else
			{
				updown = false;
			}

			AddPage( 0 );

			AddBackground( 0, 0, 600, 440, 0x24AE );

			AddBackground( 66, 40, 150, 26, 0x2486 );
			AddButton( 71, 45, 0x845, 0x846, 100, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 96, 43, 110, 26, 1063014, false, false );

			AddBackground( 236, 40, 150, 26, 0x2486 );
			AddButton( 241, 45, 0x845, 0x846, 110, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 266, 43, 110, 26, 1062974, false, false );

			AddBackground( 401, 40, 150, 26, 0x2486 );
			AddButton( 406, 45, 0x845, 0x846, 120, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 431, 43, 110, 26, 1062978, 0xF, false, false );

			AddButton( 95, 80, 0x15E1, 0x15E5, 800, ( ( page + 1 ) * 8 ) >= list.Count ? GumpButtonType.Page : GumpButtonType.Reply, 0 ); // Next Page
			AddButton( 65, 80, 0x15E3, 0x15E7, 801, page == 0 ? GumpButtonType.Page : GumpButtonType.Reply, 0 ); // Previous Page

			AddBackground( 130, 75, 385, 30, 0xBB8 );
			AddTextEntry( 135, 80, 375, 30, 0x481, 3535, search );
			AddButton( 520, 75, 0x867, 0x868, 750, GumpButtonType.Reply, 0 );

			AddImageTiled( 65, 110, 290, 26, 0xA40 );
			AddImageTiled( 67, 112, 286, 22, 0xBBC );
			AddHtmlLocalized( 70, 113, 280, 20, 1062954, false, false );

			if ( Sort_Type == 0 ) // Sort by Name
			{
				if ( updown )
					AddButton( 339, 117, 0x983, 0x984, 401, GumpButtonType.Reply, 0 );
				else
					AddButton( 339, 117, 0x985, 0x986, 400, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 339, 117, 0x2716, 0x2716, 400, GumpButtonType.Reply, 0 );
			}

			AddImageTiled( 357, 110, 60, 26, 0xA40 );
			AddImageTiled( 359, 112, 56, 22, 0xBBC );
			AddHtmlLocalized( 362, 113, 50, 20, 1062957, false, false );

			if ( Sort_Type == 1 ) // Sort by Abbreviation
			{
				if ( updown )
					AddButton( 401, 117, 0x983, 0x984, 403, GumpButtonType.Reply, 0 );
				else
					AddButton( 401, 117, 0x985, 0x986, 402, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 401, 117, 0x2716, 0x2716, 402, GumpButtonType.Reply, 0 );
			}

			AddImageTiled( 419, 110, 130, 26, 0xA40 );
			AddImageTiled( 421, 112, 126, 22, 0xBBC );
			AddHtmlLocalized( 424, 113, 120, 20, 1062958, false, false );

			if ( Sort_Type == 2 ) // Sort by Status
			{
				if ( updown )
					AddButton( 534, 117, 0x983, 0x984, 405, GumpButtonType.Reply, 0 );
				else
					AddButton( 534, 117, 0x985, 0x986, 404, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 534, 117, 0x2716, 0x2716, 404, GumpButtonType.Reply, 0 );
			}

			m_List = new ArrayList( list.Count );
			m_List = list;

			switch ( Sort_Type )
			{
				case 0:
					{
						m_List.Sort( new ListNameSorter( updown ) );
						break;
					}
				case 1:
					{
						m_List.Sort( new ListAbbrSorter( updown ) );
						break;
					}
				case 2:
					{
						m_SortWarList = new ArrayList();
						m_SortAllyList = new ArrayList();

						int j = 0;

						while ( j < m_List.Count )
						{
							if ( ( (Guild) m_List[j] ).IsWar( m_Guild ) )
							{
								m_SortWarList.Add( m_List[j] );
								m_List.Remove( m_List[j] );
							}
							else
							{
								j++;
							}
						}

						j = 0;

						while ( j < m_List.Count )
						{
							if ( ( (Guild) m_List[j] ).IsAlly( m_Guild ) )
							{
								m_SortAllyList.Add( m_List[j] );
								m_List.Remove( m_List[j] );
							}
							else
							{
								j++;
							}
						}

						m_SortAllyList.Sort( new ListNameSorter( updown ) );
						m_SortWarList.Sort( new ListNameSorter( updown ) );
						m_List.Sort( new ListNameSorter( updown ) );

						for ( j = 0; j < m_SortAllyList.Count; j++ )
							m_SortWarList.Add( m_SortAllyList[j] );

						for ( j = 0; j < m_List.Count; j++ )
							m_SortWarList.Add( m_List[j] );

						m_List = m_SortWarList;
						break;
					}
				case 3:
					{
						m_List.Sort( new ListRelationshipSorter( updown ) );
						break;
					}
				case 4:
					{
						m_List.Sort( new ListAwaitingSorter( updown ) );
						break;
					}
			}

			for ( int i = ( m_Page * 8 ), n = 0; i < list.Count && i < 8 + ( m_Page * 8 ); ++i, ++n )
			{
				Guild g = (Guild) list[i] as Guild;

				if ( m_Guild.WarInvitations.Contains( g ) || m_Guild.AllyInvitations.Contains( g ) || g.AllyDeclarations.Contains( m_Guild ) || g.WarDeclarations.Contains( m_Guild ) || g.WarInvitations.Contains( m_Guild ) || g.AllyInvitations.Contains( m_Guild ) || m_Guild.AllyDeclarations.Contains( g ) || m_Guild.WarDeclarations.Contains( g ) )
					AddButton( 36, 143 + ( n * 28 ), 0x8AF, 0x8AF, i + 1000, GumpButtonType.Reply, 0 );
				else
					AddButton( 40, 143 + ( n * 28 ), 0x4B9, 0x4BA, i + 1000, GumpButtonType.Reply, 0 );

				AddImageTiled( 65, 138 + ( n * 28 ), 290, 26, 0xA40 );
				AddImageTiled( 67, 140 + ( n * 28 ), 286, 22, 0xBBC );

				if ( g.Name == m_Guild.Name )
					AddHtml( 70, 141 + ( n * 28 ), 280, 20, "<basefont color=#006600>" + g.Name + "</basefont>", false, false );
				else
					AddHtml( 70, 141 + ( n * 28 ), 280, 20, g.Name, false, false );

				AddImageTiled( 357, 138 + ( n * 28 ), 60, 26, 0xA40 );
				AddImageTiled( 359, 140 + ( n * 28 ), 56, 22, 0xBBC );

				AddHtml( 362, 141 + ( n * 28 ), 50, 20, g.Abbreviation, false, false );

				AddImageTiled( 419, 138 + ( n * 28 ), 130, 26, 0xA40 );
				AddImageTiled( 421, 140 + ( n * 28 ), 126, 22, 0xBBC );

				if ( g == m_Guild )
				{
					if ( g.Allies.Count > 0 )
					{
						if ( g.AllianceLeader )
							AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 1063237, false, false ); // Alliance Leader
						else
							AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 1062964, false, false ); // Ally
					}
					else
					{
						AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 3000085, false, false ); // Peace
					}
				}
				if ( m_Guild.IsWar( g ) )
				{
					AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 3000086, false, false ); // War
				}
				else if ( m_Guild.IsAlly( g ) )
				{
					if ( g.AllianceLeader )
						AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 1063237, false, false ); // Alliance Leader
					else
						AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 1062964, false, false ); // Ally
				}
				else if ( g != m_Guild )
				{
					AddHtmlLocalized( 424, 141 + ( n * 28 ), 120, 20, 3000085, false, false ); // Peace
				}

				if ( i == m_List.Count - 1 && m_Guild.Allies.Count + 1 >= m_List.Count )
					AddHtml( 66, 204 + ( ( n - 1 ) * 28 ), 300, 26, "<basefont color=#000066>" + g.AllianceName + "</basefont>", false, false );
				else
					AddKRLabel( 0, 0, 0, 0, -1, false, false );
			}

			if ( Sort_Type == 3 )
				AddHtmlLocalized( 66, 377, 280, 26, 1063138, 0xF, false, false );
			else if ( Sort_Type == 4 )
				AddHtmlLocalized( 66, 377, 280, 26, 1063137, 0xF, false, false );
			else
				AddHtmlLocalized( 66, 377, 280, 26, 1063136, 0xF, false, false );

			AddBackground( 350, 372, 200, 26, 0x2486 );
			AddButton( 355, 377, 0x845, 0x846, 975, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 380, 375, 160, 26, 1063083, false, false ); // Advanced Search
			AddKRHtmlLocalized( 0, 0, 0, 0, -2, false, false );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Guild.BadMember( m_Mobile ) )
				return;

			/* Select Guild */
			if ( info.ButtonID >= 1000 && info.ButtonID < ( 1000 + m_List.Count ) )
			{
				Guild g = (Guild) m_List[info.ButtonID - 1000];

				if ( g != null && !g.Disbanded )
				{
					if ( g == m_Guild )
					{
						m_Mobile.SendGump( new MyGuildGump( m_Mobile, m_Guild ) );
					}
					else if ( m_Guild.WarDeclarations.Contains( g ) )
					{
						m_Mobile.SendGump( new WarRequestGump( m_Mobile, g ) );
					}
					else if ( m_Guild.WarInvitations.Contains( g ) )
					{
						m_Mobile.SendGump( new ReceiveWarGump( m_Mobile, g ) );
					}
					else if ( m_Guild.IsWar( g ) )
					{
						m_Mobile.SendGump( new ProcessWarGump( m_Mobile, g ) );
					}
					else if ( m_Guild.AllyDeclarations.Contains( g ) )
					{
						m_Mobile.SendGump( new AllianceRequestGump( m_Mobile, g ) );
					}
					else if ( m_Guild.AllyInvitations.Contains( g ) )
					{
						m_Mobile.SendGump( new AcceptAllianceGump( m_Mobile, g ) );
					}
					else if ( m_Guild.IsAlly( g ) )
					{
						if ( m_Guild.AllianceLeader )
							m_Mobile.SendGump( new AllianceLeaderGump( m_Mobile, g ) );
						else
							m_Mobile.SendGump( new SlaveAllyGump( m_Mobile, g ) );
					}
					else
					{
						m_Mobile.SendGump( new DiplomacyMiscGump( m_Mobile, g ) );
					}
				}
			}

			/* Main Gump Buttons */
			else if ( info.ButtonID == 100 )
			{
				m_Mobile.SendGump( new MyGuildGump( m_Mobile, m_Guild ) );
			}
			else if ( info.ButtonID == 110 )
			{
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 2, "" ) );
			}
			else if ( info.ButtonID == 120 )
			{
				BaseGuild[] guilds = Guild.Search( "" );

				if ( guilds.Length > 0 )
					m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );
			}

			/* Advanced Search */
			else if ( info.ButtonID == 975 )
			{
				m_Mobile.SendGump( new AdvancedSearchGump( m_Mobile, m_Guild ) );
			}

			/* Sorters */
			else if ( info.ButtonID == 400 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 10 ) );
			}
			else if ( info.ButtonID == 401 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 0 ) );
			}
			else if ( info.ButtonID == 402 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 11 ) );
			}
			else if ( info.ButtonID == 403 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 1 ) );
			}
			else if ( info.ButtonID == 404 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 12 ) );
			}
			else if ( info.ButtonID == 405 )
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, 2 ) );
			}

			/* Search */
			if ( info.ButtonID == 750 )
			{
				string text = info.GetTextEntry( 3535 ).Text;
				text = text.Trim();

				if ( text.Length >= 3 )
				{
					BaseGuild[] guilds = Guild.Search( text );

					if ( guilds.Length > 0 )
						m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2, text ) );
					else
						m_Mobile.SendLocalizedMessage( 1018003 ); // No guilds found matching - try another name in the search
				}
				else
				{
					m_Mobile.SendMessage( "Search string must be at least three letters in length." );
				}
			}

			/* Pag Buttons */
			else if ( info.ButtonID == 800 ) // Av. Pag
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, m_SortType, m_Page + 1, m_Search ) );
			}
			else if ( info.ButtonID == 801 ) // Re. Page
			{
				m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, m_List, m_SortType, m_Page - 1, m_Search ) );
			}
		}

		#region Sorters
		private class ListNameSorter : IComparer
		{
			private bool Dsort;

			public ListNameSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Guild) x ).Name;
				ystr = ( (Guild) y ).Name;

				if ( Dsort )
				{
					return String.Compare( ystr, xstr, true );
				}
				else
				{
					return String.Compare( xstr, ystr, true );
				}
			}
		}

		private class ListAbbrSorter : IComparer
		{
			private bool Dsort;

			public ListAbbrSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Guild) x ).Abbreviation;
				ystr = ( (Guild) y ).Abbreviation;

				if ( Dsort )
				{
					return String.Compare( ystr, xstr, true );
				}
				else
				{
					return String.Compare( xstr, ystr, true );
				}
			}
		}

		private class ListAwaitingSorter : IComparer
		{
			private bool Dsort;

			public ListAwaitingSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Guild) x ).Name;
				ystr = ( (Guild) y ).Name;

				if ( Dsort )
				{
					return String.Compare( ystr, xstr, true );
				}
				else
				{
					return String.Compare( xstr, ystr, true );
				}
			}
		}

		private class ListRelationshipSorter : IComparer
		{
			private bool Dsort;

			public ListRelationshipSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Guild) x ).Name;
				ystr = ( (Guild) y ).Name;

				if ( Dsort )
				{
					return String.Compare( ystr, xstr, true );
				}
				else
				{
					return String.Compare( xstr, ystr, true );
				}
			}
		}
		#endregion
	}
}