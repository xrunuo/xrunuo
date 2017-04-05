using Server.Engines.Guilds.Gumps;
using Server.Prompts;

namespace Server.Engines.Guilds.Prompts
{
	public class GuildWebsitePrompt : Prompt
	{
		// Enter the new website for the guild (50 characters max):
		public override int MessageCliloc { get { return 1013072; } }

		private Mobile m_Mobile;
		private Guild m_Guild;

		public GuildWebsitePrompt( Mobile m, Guild g )
			: base( m )
		{
			m_Mobile = m;
			m_Guild = g;
		}

		public override void OnCancel( Mobile from )
		{
			if ( m_Guild.BadLeader( m_Mobile ) )
				return;

			m_Mobile.CloseGump<MyGuildGump>();

			m_Mobile.SendLocalizedMessage( 1070778 ); // You submit a new guild website.
		}

		public override void OnResponse( Mobile from, string text )
		{
			if ( m_Guild.BadLeader( m_Mobile ) )
				return;

			Guild g = from.Guild as Guild;

			text = text.Trim();

			if ( text.Length > 50 )
				text = text.Substring( 0, 50 );

			if ( text.Length > 0 )
				g.Website = text;

			m_Mobile.CloseGump<MyGuildGump>();

			m_Mobile.SendLocalizedMessage( 1070778 ); // You submit a new guild website.
		}
	}
}