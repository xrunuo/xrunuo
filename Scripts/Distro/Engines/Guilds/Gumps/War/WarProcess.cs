using System.Collections;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class ProcessWarGump : Gump
	{
		public override int TypeID { get { return 0x2DA; } }

		private Mobile m_Mobile;
		private Guild m_Guild;
		private Guild w_Guild;

		public ProcessWarGump( Mobile from, Guild guild )
			: base( 10, 40 )
		{
			m_Mobile = from;
			m_Guild = from.Guild as Guild;
			w_Guild = guild;

			uint ET = m_Guild.GetExpTime( w_Guild );

			int h = (int) ( ET / 60 );
			int m = (int) ( ET - h * 60 );

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
			Intern( "<basefont color=#black>" + m_Guild.GetKills( w_Guild ) + "/" + m_Guild.GetMaxKills( w_Guild ) + "</basefont>" );
			/*10*/
			Intern( "<basefont color=#black>" + h + ":" + m + "</basefont>" );
			/*11*/
			Intern( "<basefont color=#black>" + w_Guild.GetKills( m_Guild ) + "/" + m_Guild.GetMaxKills( w_Guild ) + "</basefont>" );

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
			AddButton( 25, 295, 0x845, 0x846, 100, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 293, 185, 26, 1062980, 0x0, false, false ); // Surrender

			AddHtmlLocalized( 20, 180, 480, 30, 1062965, 0x0, true, false ); // You are at war with this guild!
			AddImageTiled( 20, 245, 480, 2, 0x2711 );

			//AddMasterGump( 725 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int m_Rank = ( m_Mobile as PlayerMobile ).GuildRank;

			if ( m_Guild.BadMember( m_Mobile ) )
				return;

			switch ( info.ButtonID )
			{
				case 0:
					{
						m_Mobile.CloseGump<ProcessWarGump>();

						BaseGuild[] guilds = Guild.Search( "" );

						m_Mobile.SendGump( new DiplomacyGump( m_Mobile, m_Guild, new ArrayList( guilds ), 2 ) );

						break;
					}
				case 100:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars. 

							return;
						}

						if ( w_Guild != null && m_Guild.Allies.Count == 0 || ( m_Guild.Allies.Count > 0 && m_Guild.AllianceLeader ) )
						{
							m_Guild.RemoveEnemy( w_Guild );

							Guild a_Guild, wa_Guild;

							if ( m_Guild.Allies.Count > 0 )
							{
								for ( int i = 0; i < m_Guild.Allies.Count; i++ )
								{
									a_Guild = m_Guild.Allies[i] as Guild;

									a_Guild.RemoveEnemy( w_Guild );

									for ( int j = 0; j < w_Guild.Allies.Count; j++ )
									{
										wa_Guild = w_Guild.Allies[j] as Guild;

										a_Guild.RemoveEnemy( wa_Guild );
									}
								}
							}

							for ( int j = 0; j < w_Guild.Allies.Count; j++ )
							{
								wa_Guild = w_Guild.Allies[j] as Guild;

								m_Guild.RemoveEnemy( wa_Guild );
							}

							m_Mobile.SendLocalizedMessage( 1070740, w_Guild.Name ); // You have lost the war with ~1_val~.

							w_Guild.Leader.SendLocalizedMessage( 1070740, m_Guild.Name ); // You have lost the war with ~1_val~.

							m_Mobile.CloseGump<ProcessWarGump>();
						}
						else
						{
							m_Mobile.SendMessage( 0, "Only Alliance Leader must Surrender a guild war." ); // ???
						}

						break;
					}
			}
		}
	}
}