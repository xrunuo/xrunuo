using System.Collections;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class AllianceLeaderGump : Gump
	{
		public override int TypeID { get { return 0x2DA; } }

		private Mobile m_Mobile;
		private Guild m_Guild, t_Guild;

		public AllianceLeaderGump( Mobile from, Guild guild )
			: base( 10, 40 )
		{
			m_Mobile = from;
			m_Guild = from.Guild as Guild;
			t_Guild = guild;

			/*0*/
			Intern( "Unknown" );
			/*1*/
			Intern( "" );
			/*2*/
			Intern( "" );
			/*3*/
			Intern( "0/0" );
			/*4*/
			Intern( "0/0" );
			/*5*/
			Intern( "00:00" );
			/*6*/
			Intern( guild.Name );
			/*7*/
			Intern( "<basefont color=#black>" + guild.AllianceName + "</basefont>" );
			/*8*/
			Intern( guild.Abbreviation );
			/*9*/
			Intern( "<basefont color=#black>0/0</basefont>" );
			/*10*/
			Intern( "<basefont color=#black>00:00</basefont>" );
			/*11*/
			Intern( "<basefont color=#black>0/0</basefont>" );

			AddPage( 0 );

			AddBackground( 0, 0, 520, 335, 0x242C );
			AddHtmlLocalized( 20, 15, 480, 26, 1062975, 0x0, false, false ); // Guild Relationship
			AddImageTiled( 20, 40, 480, 2, 0x2711 );

			AddHtmlLocalized( 20, 50, 120, 26, 1062954, 0x0, true, false ); // Guild Name
			AddHtmlIntern( 150, 53, 360, 26, 6, false, false );

			AddHtmlLocalized( 20, 80, 120, 26, 1063025, 0x0, true, false ); // Alliance
			AddHtmlIntern( 150, 83, 360, 26, 7, false, false );

			AddHtmlLocalized( 20, 110, 120, 26, 1063139, 0x0, true, false ); // Abbreviation
			AddHtmlIntern( 150, 113, 120, 26, 8, false, false );

			AddHtmlLocalized( 280, 110, 120, 26, 1062966, 0x0, true, false ); // Your Kills
			AddHtmlIntern( 410, 113, 120, 26, 9, false, false );

			AddHtmlLocalized( 20, 140, 120, 26, 1062968, 0x0, true, false ); // Time Remaining
			AddHtmlIntern( 150, 143, 120, 26, 10, false, false );

			AddHtmlLocalized( 280, 140, 120, 26, 1062967, 0x0, true, false ); // Their Kills
			AddHtmlIntern( 410, 143, 120, 26, 11, false, false );

			AddImageTiled( 20, 172, 480, 2, 0x2711 );

			AddBackground( 275, 290, 225, 26, 0x2486 );
			AddButton( 280, 295, 0x845, 0x846, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 293, 185, 26, 3000091, 0x0, false, false ); // Cancel

			AddBackground( 20, 290, 225, 26, 0x2486 );
			AddButton( 25, 295, 0x845, 0x846, 301, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 293, 185, 26, 1062985, 0x0, false, false ); // Leave Alliance

			AddBackground( 20, 260, 225, 26, 0x2486 );
			AddButton( 25, 265, 0x845, 0x846, 300, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 263, 185, 26, 1062984, 0x0, false, false ); // Remove Guild from Alliance

			AddBackground( 275, 260, 225, 26, 0x2486 );
			AddButton( 280, 265, 0x845, 0x846, 306, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 263, 185, 26, 1063433, 0x0, false, false ); // Promote to Alliance Leader

			AddBackground( 148, 215, 225, 26, 0x2486 );
			AddButton( 153, 220, 0x845, 0x846, 310, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 178, 218, 185, 26, 1063164, 0x0, false, false ); // Show Alliance Roster

			AddHtmlLocalized( 20, 180, 480, 30, 1062970, 0x0, true, false ); // You are allied with this guild.
			AddImageTiled( 20, 245, 480, 2, 0x2711 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int m_Rank = ( m_Mobile as PlayerMobile ).GuildRank;

			if ( m_Guild.BadMember( m_Mobile ) )
			{
				return;
			}

			switch ( info.ButtonID )
			{
				case 0: // Cancel
					{
						m_Mobile.CloseGump<AllianceLeaderGump>();

						BaseGuild[] guilds = Guild.Search( "" );

						m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );

						break;
					}
				case 301: // Leave Alliance
					{
						if ( m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063436 ); // You don't have permission to negotiate an alliance. 

							return;
						}

						if ( m_Guild.AllianceLeader )
						{
							m_Guild.AllianceLeader = false;

							m_Guild.AllianceName = "";

							ArrayList alliance = new ArrayList( m_Guild.Allies );

							if ( alliance.Count > 0 )
							{
								( alliance[Utility.Random( alliance.Count )] as Guild ).AllianceLeader = true;
							}
						}

						Guild a_Guild;

						if ( m_Guild.Allies.Count > 0 )
						{
							for ( int i = 0; i < m_Guild.Allies.Count; i++ )
							{
								a_Guild = m_Guild.Allies[i] as Guild;

								a_Guild.RemoveAlly( m_Guild );

								m_Guild.RemoveAlly( a_Guild );

								if ( a_Guild.Allies.Count <= 0 )
								{
									a_Guild.AllianceName = "";
								}
							}
						}

						m_Mobile.CloseGump<AllianceLeaderGump>();

						break;
					}
				case 300: // Remove Guild from Alliance
					{
						if ( m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063436 ); // You don't have permission to negotiate an alliance. 

							return;
						}

						if ( ( t_Guild != null ) && t_Guild.IsAlly( m_Guild ) && m_Guild.AllianceLeader )
						{
							t_Guild.AllianceName = "";

							m_Guild.RemoveAlly( t_Guild );

							if ( m_Guild.AllianceLeader && ( m_Guild.Allies.Count <= 0 ) )
							{
								m_Guild.AllianceLeader = false;

								m_Guild.AllianceName = "";

								m_Mobile.SendLocalizedMessage( 1070762 ); // Your Alliance has dissolved.
							}
						}
						else
						{
							m_Mobile.SendLocalizedMessage( 1063447 ); // Failed to remove guild from alliance.
						}

						m_Mobile.CloseGump<AllianceLeaderGump>();

						break;
					}
				case 306: // Promote to Alliance Leader
					{
						if ( m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063436 ); // You don't have permission to negotiate an alliance. 
							return;

						}

						if ( ( t_Guild != null ) && t_Guild.IsAlly( m_Guild ) )
						{
							t_Guild.AllianceLeader = true;

							m_Guild.AllianceLeader = false;

							m_Mobile.CloseGump<AllianceLeaderGump>();
						}

						m_Mobile.SendLocalizedMessage( 1063435 ); // Failed to promote guild to alliance leader.

						break;
					}
				case 310: // Show Alliance Roster
					{
						if ( m_Guild.Allies.Count > 0 )
						{
							ArrayList guilds = new ArrayList( m_Guild.Allies );

							guilds.Add( m_Guild );

							m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );
						}

						m_Mobile.CloseGump<AllianceLeaderGump>();

						break;
					}
			}
		}
	}
}