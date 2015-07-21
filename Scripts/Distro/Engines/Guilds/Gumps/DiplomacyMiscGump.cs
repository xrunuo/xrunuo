using System.Collections;
using Server.Engines.Guilds.Prompts;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class DiplomacyMiscGump : Gump
	{
		public override int TypeID { get { return 0x2DA; } }

		private Mobile m_Mobile;
		private Guild m_Guild;
		private Guild t_Guild;

		public DiplomacyMiscGump( Mobile leader, Guild target )
			: base( 10, 40 )
		{
			m_Mobile = leader;
			t_Guild = target;
			m_Guild = m_Mobile.Guild as Guild;

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
			Intern( target.Name );
			/*7*/
			Intern( "<basefont color=#black>" + t_Guild.AllianceName + "</basefont>" );
			/*8*/
			Intern( target.Abbreviation );
			/*9*/
			Intern( "<basefont color=#black>0/0</basefont>" );
			/*10*/
			Intern( "<basefont color=#black>00:00</basefont>" );
			/*11*/
			Intern( "<basefont color=#black>0/0</basefont>" );

			AddPage( 0 );

			AddBackground( 0, 0, 520, 335, 0x242C );

			AddHtmlLocalized( 20, 15, 480, 26, 1062975, false, false ); // <div align=center><i>Guild Relationship</i></div>
			AddImageTiled( 20, 40, 480, 2, 0x2711 );

			AddHtmlLocalized( 20, 50, 120, 26, 1062954, true, false ); // <i>Guild Name</i>
			AddHtmlIntern( 150, 53, 360, 26, 6, false, false );

			AddHtmlLocalized( 20, 80, 120, 26, 1063025, true, false ); // <i>Alliance</i>
			AddHtmlIntern( 150, 83, 360, 26, 7, false, false );

			AddHtmlLocalized( 20, 110, 120, 26, 1063139, true, false ); // <i>Abbreviation</i>
			AddHtmlIntern( 150, 113, 120, 26, 8, false, false );

			AddHtmlLocalized( 280, 110, 120, 26, 1062966, true, false ); // <i>Your Kills</i>
			AddHtmlIntern( 410, 113, 120, 26, 9, false, false );

			AddHtmlLocalized( 20, 140, 120, 26, 1062968, true, false ); // <i>Time Remaining</i>
			AddHtmlIntern( 150, 143, 120, 26, 10, false, false );

			AddHtmlLocalized( 280, 140, 120, 26, 1062967, true, false ); // <i>Their Kills</i>
			AddHtmlIntern( 410, 143, 120, 26, 11, false, false );

			AddImageTiled( 20, 172, 480, 2, 0x2711 );

			AddBackground( 275, 290, 225, 26, 0x2486 );
			AddButton( 280, 295, 0x845, 0x846, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 293, 185, 26, 3000091, false, false ); // Cancel

			AddBackground( 20, 290, 225, 26, 0x2486 );
			AddButton( 25, 295, 0x845, 0x846, 400, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 293, 185, 26, 1062989, false, false ); // Declare War!

			AddBackground( 20, 260, 225, 26, 0x2486 );
			AddButton( 25, 265, 0x845, 0x846, 305, GumpButtonType.Reply, 0 ); // Request Alliance
			AddHtmlLocalized( 50, 263, 185, 26, 1062990, false, false );

			if ( m_Guild.IsWar( t_Guild ) )
				AddHtmlLocalized( 20, 180, 480, 30, 1062965, true, false ); // war
			else if ( m_Guild.IsAlly( t_Guild ) )
				AddHtmlLocalized( 20, 180, 480, 30, 1062970, true, false ); // allied
			else
				AddHtmlLocalized( 20, 180, 480, 30, 1062973, true, false ); // peace

			AddImageTiled( 20, 245, 480, 2, 0x2711 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			int m_Rank = ( m_Mobile as PlayerMobile ).GuildRank;

			if ( m_Guild.BadMember( m_Mobile ) )
			{
				return;
			}

			switch ( info.ButtonID )
			{
				case 0:
					{
						BaseGuild[] guilds = Guild.Search( "" );

						m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );

						break;
					}
				case 400:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars.

							return;
						}
						else
						{
							m_Mobile.SendGump( new DeclareWarGump( m_Mobile, t_Guild ) );
						}

						break;
					}
				case 305:
					{
						if ( m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063436 ); // You don't have permission to negotiate an alliance.

							return;
						}
						else
						{
							if ( t_Guild != null && ( t_Guild.Enemies.Count <= 0 ) && ( m_Guild.Enemies.Count <= 0 ) )
							{
								if ( m_Guild.Allies.Count == 0 && m_Guild.AllyDeclarations.Count == 0 && t_Guild.Allies.Count <= 0 )
								{
									m_Mobile.Prompt = new GuildAllyPrompt( m_Mobile, t_Guild, m_Guild );
								}
								else
								{
									if ( m_Guild.AllianceLeader && t_Guild.AllyDeclarations.Count == 0 && t_Guild.Allies.Count == 0 )
									{
										if ( !m_Guild.AllyDeclarations.Contains( t_Guild ) )
										{
											m_Guild.AllyDeclarations.Add( t_Guild );

											m_Mobile.SendLocalizedMessage( 1070750, t_Guild.Name );
										}

										if ( !t_Guild.AllyInvitations.Contains( m_Guild ) )
										{
											t_Guild.AllyInvitations.Add( m_Guild );
										}
									}
									else
									{
										m_Mobile.SendLocalizedMessage( 1070748 ); // Failed to create alliance.
									}
								}
							}
						}

						break;
					}
			}
		}
	}
}