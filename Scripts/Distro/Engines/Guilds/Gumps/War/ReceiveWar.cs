using System;
using System.Collections;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class ReceiveWarGump : Gump
	{
		public override int TypeID { get { return 0x2DA; } }

		private Mobile m_Mobile;
		private Guild m_Guild;
		private Guild i_Guild;

		public ReceiveWarGump( Mobile from, Guild guild )
			: base( 10, 40 )
		{
			m_Mobile = from;
			m_Guild = from.Guild as Guild;
			i_Guild = guild;

			int h, m;
			uint ET;

			ET = i_Guild.GetWarTime( m_Guild );

			h = (int) ( ET / 60 );
			m = (int) ( ET - h * 60 );

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
			Intern( String.Format( "<basefont color=#black>{0}</basefont>", guild.AllianceName ) );
			/*8*/
			Intern( guild.Abbreviation );
			/*9*/
			Intern( String.Format( "<basefont color=#990000>0/{0}</basefont>", i_Guild.GetMaxKills( m_Guild ) ) );
			/*10*/
			Intern( String.Format( "<basefont color=#990000>{0}:{1}</basefont>", h, m ) );
			/*11*/
			Intern( String.Format( "<basefont color=#990000>0/{0}</basefont>", i_Guild.GetMaxKills( m_Guild ) ) );

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
			AddButton( 25, 295, 0x845, 0x846, 201, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 293, 185, 26, 1062982, 0x0, false, false ); // Dismiss Challenge

			AddBackground( 20, 260, 225, 26, 0x2486 );
			AddButton( 25, 265, 0x845, 0x846, 200, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 263, 185, 26, 1062981, 0x0, false, false ); // Accept Challenge

			AddBackground( 275, 260, 225, 26, 0x2486 );
			AddButton( 280, 265, 0x845, 0x846, 202, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 305, 263, 185, 26, 1062983, 0x0, false, false ); // Modify Terms

			AddHtmlLocalized( 20, 180, 480, 30, 1062969, 0x0, true, false ); // This guild has challenged you to war!
			AddImageTiled( 20, 245, 480, 2, 0x2711 );

			//AddMasterGump( 725 );
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
				case 0:
					{
						m_Mobile.CloseGump<ReceiveWarGump>();

						BaseGuild[] guilds = Guild.Search( "" );

						m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );

						break;
					}
				case 201:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars. 
							return;
						}

						if ( i_Guild != null )
						{
							m_Guild.WarInvitations.Remove( i_Guild );
							i_Guild.WarDeclarations.Remove( m_Guild );

							i_Guild.RemoveEnemy( m_Guild );
							i_Guild.DelWar( m_Guild );

							m_Mobile.CloseGump<ReceiveWarGump>();
						}

						break;
					}
				case 200:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars. 
							return;
						}

						if ( i_Guild != null )
						{
							m_Guild.WarInvitations.Remove( i_Guild );
							i_Guild.WarDeclarations.Remove( m_Guild );

							m_Guild.AddEnemy( i_Guild );

							Guild a_Guild, wa_Guild;

							if ( m_Guild.Allies.Count > 0 )
							{
								for ( int i = 0; i < m_Guild.Allies.Count; i++ )
								{
									a_Guild = m_Guild.Allies[i] as Guild;

									a_Guild.WarInitializations.Add( i_Guild );

									a_Guild.AddWar( i_Guild, 0, i_Guild.GetMaxKills( m_Guild ), i_Guild.GetWarTime( m_Guild ), i_Guild.GetExpTime( m_Guild ) );
									a_Guild.AddEnemy( i_Guild );

									if ( i_Guild.AllianceLeader )
									{
										for ( int j = 0; j < i_Guild.Allies.Count; j++ )
										{
											wa_Guild = i_Guild.Allies[j] as Guild;

											a_Guild.AddWar( wa_Guild, 0, i_Guild.GetMaxKills( m_Guild ), i_Guild.GetWarTime( m_Guild ), i_Guild.GetExpTime( m_Guild ) );
											a_Guild.AddEnemy( wa_Guild );

											wa_Guild.AddWar( a_Guild, 0, i_Guild.GetMaxKills( m_Guild ), i_Guild.GetWarTime( m_Guild ), i_Guild.GetExpTime( m_Guild ) );
											wa_Guild.AddEnemy( a_Guild );
										}
									}
								}
							}

							m_Guild.AddWar( i_Guild, 0, i_Guild.GetMaxKills( m_Guild ), i_Guild.GetWarTime( m_Guild ), i_Guild.GetExpTime( m_Guild ) );

							if ( i_Guild.AllianceLeader )
							{
								for ( int i = 0; i < i_Guild.Allies.Count; i++ )
								{
									a_Guild = i_Guild.Allies[i] as Guild;

									m_Guild.AddWar( a_Guild, 0, i_Guild.GetMaxKills( m_Guild ), i_Guild.GetWarTime( m_Guild ), i_Guild.GetExpTime( m_Guild ) );
									m_Guild.AddEnemy( a_Guild );
								}
							}

							m_Guild.WarInitializations.Add( i_Guild );

							m_Guild.GuildMessage( 1018020, "" ); // Guild Message: Your guild is now at war.
							i_Guild.GuildMessage( 1018020, "" ); // Guild Message: Your guild is now at war.

							m_Mobile.SendLocalizedMessage( 1070752 ); // The proposal has been updated.

							m_Mobile.CloseGump<ReceiveWarGump>();
						}

						break;
					}
				case 202:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars. 
							return;
						}

						if ( i_Guild != null )
						{
							m_Guild.WarInvitations.Remove( i_Guild );
							i_Guild.WarDeclarations.Remove( m_Guild );

							i_Guild.RemoveEnemy( m_Guild );

							i_Guild.DelWar( m_Guild );
						}

						m_Mobile.CloseGump<ReceiveWarGump>();
						m_Mobile.SendGump( new DeclareWarGump( m_Mobile, i_Guild ) );

						break;
					}
			}
		}
	}
}