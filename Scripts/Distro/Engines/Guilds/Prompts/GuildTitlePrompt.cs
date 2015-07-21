using Server.Engines.Guilds.Gumps;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Engines.Guilds.Prompts
{
	public class GuildTitlePrompt : Prompt
	{
		// Enter the new title for this guild member or 'none' to remove a title:
		public override int MessageCliloc { get { return 1011128; } }

		private Mobile m_Mobile, m_Target;
		private Guild m_Guild;

		public GuildTitlePrompt( Mobile from, Mobile target, Guild g )
			: base( from )
		{
			m_Mobile = from;
			m_Target = target;
			m_Guild = g;
		}

		public override void OnCancel( Mobile from )
		{
			int m_Rank = ( from as PlayerMobile ).GuildRank;

			if ( m_Rank != 3 && m_Rank != 5 )
			{
				return;
			}
			else if ( m_Target.Deleted || !m_Guild.IsMember( m_Target ) )
			{
				return;
			}

			m_Mobile.CloseGump( typeof( RosterMiscGump ) );

			m_Mobile.SendLocalizedMessage( 501176 ); // That title is disallowed.
		}

		public override void OnResponse( Mobile from, string text )
		{
			int m_Rank = ( from as PlayerMobile ).GuildRank;

			if ( m_Rank != 3 && m_Rank != 5 )
			{
				return;
			}
			else if ( m_Target.Deleted || !m_Guild.IsMember( m_Target ) )
			{
				return;
			}

			text = text.Trim();

			if ( text.Length > 20 )
			{
				text = text.Substring( 0, 20 );
			}

			if ( text.Length > 0 )
			{
				if ( text == "none" )
				{
					text = "";
				}

				m_Target.GuildTitle = text;
			}

			m_Mobile.CloseGump( typeof( RosterMiscGump ) );

			m_Mobile.SendLocalizedMessage( 1063156, m_Target.Name ); // The guild information for ~1_val~ has been updated.
		}
	}
}