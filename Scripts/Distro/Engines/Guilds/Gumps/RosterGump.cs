using System;
using System.Collections;
using Server.Engines.Guilds.Targets;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class RosterGump : Gump
	{
		public override int TypeID { get { return 0x2D7; } }

		protected Mobile m_Mobile;
		protected Guild m_Guild;
		protected ArrayList m_List;
		protected ArrayList m_SortList;
		protected bool updown;
		private int m_Page, m_SortType;
		private string m_FindText;

		public RosterGump( Mobile from, Guild guild, int Sort_Type, string FindText )
			: this( from, guild, guild.Members, Sort_Type, FindText, 0 )
		{
		}

		public RosterGump( Mobile from, Guild guild, int Sort_Type, string FindText, int page )
			: this( from, guild, guild.Members, Sort_Type, FindText, page )
		{
		}

		public RosterGump( Mobile from, Guild guild, ArrayList list, int Sort_Type, string FindText, int page )
			: base( 10, 10 )
		{
			m_Mobile = from;
			m_Guild = guild;
			m_Page = page;
			m_SortType = Sort_Type;
			m_FindText = FindText;

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
			AddHtmlLocalized( 266, 43, 110, 26, 1062974, 0xF, false, false );

			AddBackground( 401, 40, 150, 26, 0x2486 );
			AddButton( 406, 45, 0x845, 0x846, 120, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 431, 43, 110, 26, 1062978, false, false );

			AddButton( 95, 80, 0x15E1, 0x15E5, 800, ( ( page + 1 ) * 8 ) >= list.Count ? GumpButtonType.Page : GumpButtonType.Reply, 0 ); // Next Page
			AddButton( 65, 80, 0x15E3, 0x15E7, 801, page == 0 ? GumpButtonType.Page : GumpButtonType.Reply, 0 ); // Previous Page

			AddBackground( 130, 75, 385, 30, 0xBB8 );

			AddTextEntry( 135, 80, 375, 30, 0x481, 3535, "" );

			AddButton( 520, 75, 0x867, 0x868, 750, GumpButtonType.Reply, 0 );

			AddImageTiled( 65, 110, 140, 26, 0xA40 );
			AddImageTiled( 67, 112, 136, 22, 0xBBC );

			AddHtmlLocalized( 70, 113, 130, 20, 1062955, false, false );

			if ( Sort_Type == 0 ) // Sort by Name
			{
				if ( updown )
					AddButton( 189, 117, 0x983, 0x984, 401, GumpButtonType.Reply, 0 );
				else
					AddButton( 189, 117, 0x985, 0x986, 400, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 189, 117, 0x2716, 0x2716, 400, GumpButtonType.Reply, 0 );
			}

			AddImageTiled( 207, 110, 90, 26, 0xA40 );
			AddImageTiled( 209, 112, 86, 22, 0xBBC );
			AddHtmlLocalized( 212, 113, 80, 20, 1062956, false, false );

			if ( Sort_Type == 1 ) // Sort by Rank
			{
				if ( updown )
					AddButton( 281, 117, 0x983, 0x984, 403, GumpButtonType.Reply, 0 );
				else
					AddButton( 281, 117, 0x985, 0x986, 402, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 281, 117, 0x2716, 0x2716, 402, GumpButtonType.Reply, 0 );
			}

			AddImageTiled( 299, 110, 90, 26, 0xA40 );
			AddImageTiled( 301, 112, 86, 22, 0xBBC );
			AddHtmlLocalized( 304, 113, 80, 20, 1062952, false, false );

			if ( Sort_Type == 2 ) // Sort by LastOn
			{
				if ( updown )
					AddButton( 374, 117, 0x983, 0x984, 405, GumpButtonType.Reply, 0 );
				else
					AddButton( 374, 117, 0x985, 0x986, 404, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 374, 117, 0x2716, 0x2716, 404, GumpButtonType.Reply, 0 );
			}

			AddImageTiled( 391, 110, 160, 26, 0xA40 );
			AddImageTiled( 393, 112, 156, 22, 0xBBC );
			AddHtmlLocalized( 396, 113, 150, 20, 1062953, false, false );

			if ( Sort_Type == 3 ) // Sort by Guild Title
			{
				if ( updown )
					AddButton( 535, 117, 0x983, 0x984, 407, GumpButtonType.Reply, 0 );
				else
					AddButton( 535, 117, 0x985, 0x986, 406, GumpButtonType.Reply, 0 );
			}
			else
			{
				AddButton( 535, 117, 0x2716, 0x2716, 406, GumpButtonType.Reply, 0 );
			}

			m_List = new ArrayList();

			for ( int j1 = 0; j1 < list.Count; j1++ )
				m_List.Add( (Mobile) list[j1] );

			m_SortList = new ArrayList();

			switch ( Sort_Type )
			{
				case 0:
					{
						m_List.Sort( new ListNameSorter( updown ) );
						break;
					}
				case 1:
					{
						m_List.Sort( new ListGRSorter( updown ) );
						break;
					}
				case 2:
					{
						int j = 0;
						while ( j < m_List.Count )
						{
							if ( ( (Mobile) m_List[j] ).Client != null )
							{
								m_SortList.Add( m_List[j] );
								m_List.Remove( m_List[j] );
							}
							else
							{
								j++;
							}
						}

						m_List.Sort( new ListLastOnSorter( updown ) );

						for ( j = 0; j < m_List.Count; j++ )
						{
							m_SortList.Add( m_List[j] );
						}
						m_List = m_SortList;

						break;
					}
				case 3:
					{
						m_List.Sort( new ListGTSorter( updown ) );

						break;
					}

				case 4:
					{
						FindText = FindText.ToLower();
						int j2 = 0;
						while ( j2 < m_List.Count )
						{
							if ( ( (Mobile) m_List[j2] ).Name.ToLower().IndexOf( FindText ) < 0 )
								m_List.Remove( m_List[j2] );
							else
								j2++;
						}

						m_List.Sort( new ListNameSorter( updown ) );

						break;
					}
			}

			for ( int i = ( m_Page * 8 ), n = 0; i < m_List.Count && i < 8 + ( m_Page * 8 ); ++i, ++n )
			{
				Mobile m = (Mobile) m_List[i];

				AddButton( 40, 143 + ( n * 28 ), 0x4B9, 0x4BA, i + 1000, GumpButtonType.Reply, 0 );

				AddImageTiled( 65, 138 + ( n * 28 ), 140, 26, 0xA40 );
				AddImageTiled( 67, 140 + ( n * 28 ), 136, 22, 0xBBC );

				Mobile fealty = from.GuildFealty;

				if ( fealty == null || !guild.IsMember( fealty ) )
					fealty = guild.Leader;

				GameClient ns = ( (Mobile) m_List[i] ).Client;

				string name;
				if ( ( name = m.Name ) != null && ( name = name.Trim() ).Length <= 0 )
					name = "";

				if ( name == from.Name )
				{
					if ( fealty == from && ( from != guild.Leader ) )
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, "<basefont color=#006600>" + name + " *</basefont>", false, false );
					else
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, "<basefont color=#006600>" + name + "</basefont>", false, false );
				}
				if ( ns != null && ( m != from ) )
				{
					if ( m == fealty && ( m != guild.Leader ) )
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, "<basefont color=#0000CC>" + name + " *</basefont>", false, false );
					else
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, "<basefont color=#0000CC>" + name + "</basefont>", false, false );
				}
				if ( ns == null && ( m != from ) )
				{
					if ( m == fealty && ( m != guild.Leader ) )
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, "" + name + " *", false, false );
					else
						AddHtml( 70, 141 + ( n * 28 ), 130, 20, name, false, false );
				}

				AddImageTiled( 207, 138 + ( ( n % 8 ) * 28 ), 90, 26, 0xA40 );
				AddImageTiled( 209, 140 + ( ( n % 8 ) * 28 ), 86, 22, 0xBBC );

				int rank = 1062963;

				switch ( ( m as PlayerMobile ).GuildRank )
				{
					case 1:
						rank = 1062963; // Ronin
						break;
					case 2:
						rank = 1062962; // Member
						break;
					case 3:
						rank = 1062961; // Emissary
						break;
					case 4:
						rank = 1062960; // Warlord
						break;
					case 5:
						rank = 1062959; // Guild Leader
						break;
				}
				AddHtmlLocalized( 212, 141 + ( n * 28 ), 80, 20, rank, false, false );

				AddImageTiled( 299, 138 + ( n * 28 ), 90, 26, 0xA40 );
				AddImageTiled( 301, 140 + ( n * 28 ), 86, 22, 0xBBC );

				if ( ns != null )
				{
					AddHtmlLocalized( 304, 141 + ( n * 28 ), 80, 20, 1063015, false, false ); // Online or LastOn
				}
				else
				{
					string laston = null;
					laston = ( (Mobile) m as PlayerMobile ).m_LastLogin.ToString( "yyyy-MM-dd" );
					AddHtml( 304, 141 + ( n * 28 ), 80, 20, laston, false, false );
					//continue;
				}

				AddImageTiled( 391, 138 + ( n * 28 ), 160, 26, 0xA40 );
				AddImageTiled( 393, 140 + ( n * 28 ), 156, 22, 0xBBC );

				string title = m.GuildTitle;
				if ( title == null )
					title = "";

				AddHtml( 396, 141 + ( n * 28 ), 150, 20, title, false, false ); // Guild Title
			}

			AddBackground( 225, 372, 150, 26, 0x2486 );
			AddButton( 230, 377, 0x845, 0x846, 900, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 255, 375, 110, 26, 1062992, false, false ); // Invite Player

			AddKRHtmlLocalized( 0, 0, 0, 0, -2, false, false );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( m_Guild.BadMember( m_Mobile ) )
				return;

			if ( info.ButtonID >= 1000 && info.ButtonID < ( 1000 + m_List.Count ) )
			{
				Mobile m = (Mobile) m_List[info.ButtonID - 1000];

				if ( m != null && !m.Deleted )
					m_Mobile.SendGump( new RosterMiscGump( m_Mobile, m, m_Guild ) );
			}

			/* Main Gump Buttons */
			if ( info.ButtonID == 100 ) // My Guild
			{
				m_Mobile.SendGump( new MyGuildGump( m_Mobile, m_Guild ) );
			}
			else if ( info.ButtonID == 110 ) // Roster 
			{
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 2, "" ) );
			}
			else if ( info.ButtonID == 120 ) // Diplomacy
			{
				BaseGuild[] guilds = Guild.Search( "" );
				if ( guilds.Length > 0 )
				{
					m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );
				}
			}

			/* Search Button */
			else if ( info.ButtonID == 750 )
			{
				string text = info.GetTextEntry( 3535 ).Text;

				text = text.Trim();

				if ( text.Length >= 3 )
					m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 4, text ) );
				else
					m_Mobile.SendMessage( "Search string must be at least three letters in length." );
			}

			/* Sort buttons */
			else if ( info.ButtonID == 400 ) // Sort for Name
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 10, "" ) );
			else if ( info.ButtonID == 401 ) // Sort for Name
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 0, "" ) );

			else if ( info.ButtonID == 402 ) // Sort for Rank
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 11, "" ) );
			else if ( info.ButtonID == 403 ) // Sort for Rank
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 1, "" ) );

			else if ( info.ButtonID == 404 ) // Sort for LastOn
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 12, "" ) );
			else if ( info.ButtonID == 405 ) // Sort for LastOn
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 2, "" ) );

			else if ( info.ButtonID == 406 ) // Sort for Guild Title
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 13, "" ) );
			else if ( info.ButtonID == 407 ) // Sort for Guild Title
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 3, "" ) );

			/* Pag Buttons */
			else if ( info.ButtonID == 800 ) // Av. Pag
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, m_SortType, m_FindText, m_Page + 1 ) );
			else if ( info.ButtonID == 801 ) // Re. Page
				m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, m_SortType, m_FindText, m_Page - 1 ) );

			/* Invite Player Button */
			else if ( info.ButtonID == 900 )
			{
				if ( ( m_Guild.Leader == m_Mobile ) || ( ( m_Mobile as PlayerMobile ).GuildRank == 3 ) )
				{
					m_Mobile.Target = new InviteTarget( m_Mobile, m_Guild );
					m_Mobile.SendLocalizedMessage( 1063048 ); // Whom do you wish to invite into your guild?
				}
				else
				{
					m_Mobile.SendLocalizedMessage( 503301 ); //	 You don't have permission to do that.
				}
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

				xstr = ( (Mobile) x ).Name;
				ystr = ( (Mobile) y ).Name;

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

		private class ListLastOnSorter : IComparer
		{
			private bool Dsort;

			public ListLastOnSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Mobile) x as PlayerMobile ).m_LastLogin.ToString();
				ystr = ( (Mobile) y as PlayerMobile ).m_LastLogin.ToString();

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

		private class ListGTSorter : IComparer
		{
			private bool Dsort;

			public ListGTSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = null;
				string ystr = null;

				xstr = ( (Mobile) x ).GuildTitle;
				ystr = ( (Mobile) y ).GuildTitle;

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

		private class ListGRSorter : IComparer
		{
			private bool Dsort;

			public ListGRSorter( bool descend )
				: base()
			{
				Dsort = descend;
			}

			public int Compare( object x, object y )
			{
				string xstr = ( (Mobile) x as PlayerMobile ).GuildRank.ToString();
				string ystr = ( (Mobile) y as PlayerMobile ).GuildRank.ToString();

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