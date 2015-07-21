using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class DeclareWarGump : Gump
	{
		public override int TypeID { get { return 0x2DB; } }

		private Mobile m_Mobile;
		private Guild m_Guild;
		private Guild t_Guild;

		public DeclareWarGump( Mobile from, Guild target )
			: base( 10, 10 )
		{
			m_Mobile = from;

			m_Guild = m_Mobile.Guild as Guild;

			t_Guild = target as Guild;

			Intern( "0" );
			Intern( "0" );

			AddPage( 0 );

			AddBackground( 0, 0, 500, 340, 0x24AE );
			AddBackground( 65, 50, 370, 30, 0x2486 );

			AddHtmlLocalized( 75, 55, 370, 30, 1062979, 0x3C00, false, false );
			AddImage( 410, 45, 0x232C );

			AddHtmlLocalized( 65, 95, 200, 20, 1063009, 0x14AF, false, false );
			AddHtmlLocalized( 65, 120, 400, 20, 1063010, 0x0, false, false );
			AddBackground( 65, 150, 40, 30, 0x2486 );
			AddTextEntryIntern( 68, 154, 25, 20, 0x481, 175, 0 );

			AddHtmlLocalized( 65, 195, 200, 20, 1063011, 0x14AF, false, false );
			AddHtmlLocalized( 65, 220, 400, 20, 1063012, 0x0, false, false );
			AddBackground( 65, 250, 40, 30, 0x2486 );
			AddTextEntryIntern( 68, 254, 25, 20, 0x481, 176, 1 );

			AddBackground( 190, 270, 130, 26, 0x2486 );

			AddButton( 195, 275, 0x845, 0x846, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 220, 273, 90, 26, 1006045, 0x0, false, false );

			AddBackground( 330, 270, 130, 26, 0x2486 );

			AddButton( 335, 275, 0x845, 0x846, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 360, 273, 90, 26, 1062989, 0x5000, false, false );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			int m_Rank = ( m_Mobile as PlayerMobile ).GuildRank;

			if ( m_Guild.BadMember( m_Mobile ) )
				return;

			switch ( info.ButtonID )
			{
				case 0:
					{
						m_Mobile.CloseGump( typeof( DeclareWarGump ) );
						m_Mobile.SendGump( new DiplomacyMiscGump( m_Mobile, t_Guild ) );

						break;
					}
				case 1:
					{
						if ( m_Rank != 4 && m_Rank != 5 )
						{
							m_Mobile.SendLocalizedMessage( 1063440 ); // You don't have permission to negotiate wars. 

							return;
						}
						if ( t_Guild != null )
						{
							int text_int = 0;
							TextRelay text;

							try
							{
								text = info.GetTextEntry( 176 );

								text_int = ( ( text.Text.Length == 0 ) ? (int) 0 : Convert.ToInt32( text.Text.Trim() ) );
							}
							catch
							{
								text_int = 0;
							}

							if ( text_int >= 999 )
							{
								text_int = 999;
							}

							uint hour_int = 0;

							try
							{
								text = info.GetTextEntry( 175 );

								hour_int = ( ( text.Text.Length == 0 ) ? (uint) 0 : Convert.ToUInt32( text.Text.Trim() ) );
							}
							catch
							{
								hour_int = 0;
							}

							if ( hour_int > 0 || text_int > 0 )
							{
								if ( hour_int >= 999 )
								{
									hour_int = 999;

									text_int = 0;
								}

								t_Guild.DelWar( m_Guild );

								m_Guild.WarDeclarations.Remove( t_Guild );

								t_Guild.WarInvitations.Remove( m_Guild );

								m_Guild.AddWar( t_Guild, 0, text_int, 60 * hour_int, 60 * hour_int );
							}

							if ( !m_Guild.WarDeclarations.Contains( t_Guild ) )
							{
								m_Guild.WarDeclarations.Add( t_Guild );

								m_Mobile.SendLocalizedMessage( 1070751, t_Guild.Name ); // War proposal has been sent to ~1_val~.
							}

							if ( !t_Guild.WarInvitations.Contains( m_Guild ) )
							{
								t_Guild.WarInvitations.Add( m_Guild );

								t_Guild.Leader.SendLocalizedMessage( 1070781, m_Guild.Name ); // ~1_val~ has proposed a war.
							}
						}

						m_Mobile.CloseGump( typeof( DeclareWarGump ) );

						break;
					}
			}
		}
	}
}