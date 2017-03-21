using System.Collections;
using Server.Engines.Guilds.Prompts;
using Server.Events;
using Server.Factions;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class MyGuildGump : Gump
	{
		public override int TypeID { get { return 0x2DD; } }

		public static void Initialize()
		{
			EventSink.GuildGumpRequest += new GuildGumpRequestHandler( EventSink_GuildGumpRequest );
		}

		private static void EventSink_GuildGumpRequest( GuildGumpRequestArgs e )
		{
			Mobile beholder = e.Mobile;

			beholder.CloseGump( typeof( AdvancedSearchGump ) );
			beholder.CloseGump( typeof( DiplomacyGump ) );
			beholder.CloseGump( typeof( DiplomacyMiscGump ) );
			beholder.CloseGump( typeof( JoinGuildGump ) );
			beholder.CloseGump( typeof( InviteGump ) );
			beholder.CloseGump( typeof( RosterGump ) );
			beholder.CloseGump( typeof( RosterMiscGump ) );
			beholder.CloseGump( typeof( MyGuildGump ) );

			Guild guild = beholder.Guild as Guild;

			if ( beholder.Map == beholder.Map && beholder.InRange( beholder, 12 ) )
			{
				if ( guild == null || guild.Disbanded )
				{
					beholder.SendGump( new JoinGuildGump( beholder, guild ) );
				}
				else if ( guild.Accepted.Contains( beholder ) )
				{
					#region Factions
					PlayerState guildState = PlayerState.Find( guild.Leader );
					PlayerState targetState = PlayerState.Find( beholder );

					Faction guildFaction = ( guildState == null ? null : guildState.Faction );
					Faction targetFaction = ( targetState == null ? null : targetState.Faction );

					if ( guildFaction != targetFaction || ( targetState != null && targetState.IsLeaving ) )
						return;

					if ( guildState != null && targetState != null )
						targetState.Leaving = guildState.Leaving;
					#endregion

					guild.Accepted.Remove( beholder );
					guild.AddMember( beholder );

					( beholder as PlayerMobile ).GuildRank = 1;

					beholder.SendGump( new MyGuildGump( beholder, guild ) );
				}
				else
				{
					beholder.SendGump( new MyGuildGump( beholder, guild ) );
				}
			}
		}

		protected const string DefaultWebsite = "Guild website is not yet set";
		protected const string DefaultCharter = "The guild leader has not yet set the guild charter.";

		protected Mobile m_Mobile;
		protected Guild m_Guild;
		protected Faction m_Faction;

		public MyGuildGump( Mobile from, Guild guild )
			: this( from, guild, false )
		{
		}

		public MyGuildGump( Mobile from, Guild guild, bool resign )
			: base( 10, 10 )
		{
			m_Mobile = from;
			m_Guild = guild;

			Guild g = from.Guild as Guild;
			Mobile leader = g.Leader;
			string gname = g.Name;

			AddPage( 0 );

			AddBackground( 0, 0, 600, 440, 0x24AE );
			AddBackground( 66, 40, 150, 26, 0x2486 );

			AddButton( 71, 45, 0x845, 0x846, 100, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 96, 43, 110, 26, 1063014, 0xF, false, false ); // My Guild

			AddBackground( 236, 40, 150, 26, 0x2486 );

			AddButton( 241, 45, 0x845, 0x846, 110, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 266, 43, 110, 26, 1062974, false, false ); // Guild Roster

			AddBackground( 401, 40, 150, 26, 0x2486 );

			AddButton( 406, 45, 0x845, 0x846, 120, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 431, 43, 110, 26, 1062978, false, false ); // Diplomacy

			AddImageTiled( 65, 80, 160, 26, 0xA40 );
			AddImageTiled( 67, 82, 156, 22, 0xBBC );

			AddHtmlLocalized( 70, 83, 150, 20, 1062954, false, false ); // <i>Guild Name</i>
			AddHtml( 233, 84, 320, 26, gname, false, false );

			AddImageTiled( 65, 114, 160, 26, 0xA40 );
			AddImageTiled( 67, 116, 156, 22, 0xBBC );

			AddHtmlLocalized( 70, 117, 150, 20, 1063025, false, false ); // <i>Alliance</i>
			AddHtml( 233, 118, 320, 26, g.AllianceName, false, false );

			AddImageTiled( 65, 148, 160, 26, 0xA40 );
			AddImageTiled( 67, 150, 156, 22, 0xBBC );

			AddHtmlLocalized( 70, 151, 150, 20, 1063084, false, false ); // Guild Faction

			Faction faction = Faction.Find( m_Guild.Leader as Mobile );

			if ( faction != null )
				AddHtml( 233, 152, 320, 26, faction.ToString(), false, false );
			else
				AddHtml( 233, 152, 320, 26, "", false, false );

			if ( g.AllianceName.Length > 0 )
				AddButton( 40, 120, 0x4B9, 0x4BA, 200, GumpButtonType.Reply, 0 ); // Button on Alliance Roster

			AddImageTiled( 65, 196, 480, 4, 0x238D );

			string charter;

			if ( ( charter = g.Charter ) == null || ( charter = charter.Trim() ).Length <= 0 )
				charter = DefaultCharter;

			AddHtml( 65, 216, 480, 80, charter, true, true );

			if ( from != null && from == leader )
				AddButton( 40, 251, 0x4B9, 0x4BA, 210, GumpButtonType.Reply, 0 );

			string website;

			if ( ( website = g.Website ) == null || ( website = website.Trim() ).Length <= 0 )
				website = DefaultWebsite;

			AddHtml( 65, 306, 480, 30, website, true, false );

			if ( from != null && from == leader )
				AddButton( 40, 313, 0x4B9, 0x4BA, 215, GumpButtonType.Reply, 0 );

			AddCheck( 65, 370, 0xD2, 0xD3, from.DisplayGuildTitle, 400 );
			AddHtmlLocalized( 95, 370, 150, 26, 1063085, false, false ); // // Show Guild Title

			AddBackground( 450, 370, 100, 26, 0x2486 );

			AddButton( 455, 375, 0x845, 0x846, resign ? 501 : 500, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 480, 373, 60, 26, 3006115, resign ? 0x3C00 : 0x0, false, false ); // Resign

			Design();
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Guild.BadMember( m_Mobile ) )
				return;

			sender.Mobile.DisplayGuildTitle = info.IsSwitched( 400 );

			switch ( info.ButtonID )
			{
				case 100: // -- Open My Guild Gump
					{
						m_Mobile.SendGump( new MyGuildGump( m_Mobile, m_Guild ) );
						break;
					}
				case 110: // -- Open Guild Member List
					{
						m_Mobile.SendGump( new RosterGump( m_Mobile, m_Guild, 2, "" ) );
						break;
					}
				case 120: // -- Open Diplomancy Gump
					{
						BaseGuild[] guilds = Guild.Search( "" );

						if ( guilds.Length > 0 )
							m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );

						break;
					}
				case 200: // -- Show Alliance
					{
						m_Mobile.CloseGump( typeof( DiplomacyGump ) );

						if ( m_Guild.Allies.Count > 0 )
						{
							ArrayList guilds = new ArrayList( m_Guild.Allies );
							guilds.Add( m_Guild );
							m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );
						}
						break;
					}
				case 210: // -- Set Guild Charter
					{
						m_Mobile.Prompt = new GuildCharterPrompt( m_Mobile, m_Guild );

						break;
					}
				case 215: // -- Set Guild Website
					{
						m_Mobile.Prompt = new GuildWebsitePrompt( m_Mobile, m_Guild );

						break;
					}
				case 500: // -- Resign from Guild
					{
						m_Mobile.SendGump( new MyGuildGump( m_Mobile, m_Guild, true ) );
						m_Mobile.SendLocalizedMessage( 1063332 ); // Are you sure you wish to resign from your guild?
						break;
					}
				case 501: // Resign from Guild (Kingdom Reborn)
					{
						m_Guild.RemoveMember( m_Mobile );
						break;
					}
			}
		}

		protected virtual void Design()
		{
		}
	}
}