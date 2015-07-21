using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class InviteGump : Gump
	{
		public override int TypeID { get { return 0x2D6; } }

		private Mobile m_Mobile, m_Invite;
		private Guild inv_Guild;

		public InviteGump( Mobile from, Mobile invite, Guild guild )
			: base( 10, 10 )
		{
			m_Mobile = from;
			m_Invite = invite;
			inv_Guild = guild;

			Intern( String.Format( "<center>{0}</center>", guild.Name ) );

			AddPage( 0 );

			AddBackground( 0, 0, 350, 170, 0x2422 );
			AddHtmlLocalized( 25, 20, 300, 45, 1062946, 0x0, true, false ); // You have been invited to join a guild! (Warning: Accepting will make you attackable!)
			AddHtmlIntern( 25, 75, 300, 25, 0, true, false );
			AddButton( 265, 130, 0xF7, 0xF8, 200, GumpButtonType.Reply, 0 );
			AddButton( 195, 130, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0 );
			AddButton( 20, 130, 0xD2, 0xD3, 151, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 130, 150, 30, 1062943, 0x0, false, false ); // Ignore Guild Invites

			//AddMasterGump( 725 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( inv_Guild != null )
			{
				if ( info.ButtonID == 151 )
				{
					( m_Mobile as PlayerMobile ).AcceptGuildInvites = false;
					inv_Guild.Accepted.Remove( m_Mobile );
					m_Invite.SendLocalizedMessage( 1063250, String.Format( "{0}\t{1}\t", m_Mobile.Name, inv_Guild.Name ) ); //  ~1_val~ has declined your invitation to join ~2_val~.
					m_Mobile.SendLocalizedMessage( 1070698 ); // You are now ignoring guild invitations.
				}
				else if ( info.ButtonID == 0 )
				{
					inv_Guild.Accepted.Remove( m_Mobile );
					m_Invite.SendLocalizedMessage( 1063250, String.Format( "{0}\t{1}\t", m_Mobile.Name, inv_Guild.Name ) ); //  ~1_val~ has declined your invitation to join ~2_val~.
				}
				else if ( info.ButtonID == 200 )
				{
					inv_Guild.Accepted.Remove( m_Mobile );
					inv_Guild.AddMember( m_Mobile );
					m_Mobile.Guild = inv_Guild;
					( m_Mobile as PlayerMobile ).GuildRank = 1;
					m_Mobile.SendLocalizedMessage( 1063056, inv_Guild.Name ); //  You have joined ~1_val~.
					m_Invite.SendLocalizedMessage( 1063249, String.Format( "{0}\t{1}\t", m_Mobile.Name, inv_Guild.Name ) ); //  ~1_val~ has accepted your invitation to join ~2_val~.
				}
			}
		}
	}
}
