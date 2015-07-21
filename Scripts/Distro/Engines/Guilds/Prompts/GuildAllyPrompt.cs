using Server.Engines.Guilds.Gumps;
using Server.Prompts;

namespace Server.Engines.Guilds.Prompts
{
	public class GuildAllyPrompt : Prompt
	{
		// Enter a name for the new alliance:
		public override int MessageCliloc { get { return 1063439; } }

		private Mobile m_Leader;
		private Guild m_Guild, m_Target;

		public GuildAllyPrompt( Mobile leader, Guild target, Guild g )
			: base( leader )
		{
			m_Leader = leader;
			m_Target = target;
			m_Guild = g;
		}

		public override void OnCancel( Mobile from )
		{
			if ( m_Guild.BadLeader( m_Leader ) )
			{
				return;
			}
			else if ( m_Target.Disbanded || m_Guild.IsWar( m_Target ) )
			{
				return;
			}

			m_Leader.CloseGump( typeof( DiplomacyMiscGump ) );

			m_Leader.SendLocalizedMessage( 1070886 ); // That alliance name is not allowed.			
		}

		public override void OnResponse( Mobile from, string text )
		{
			if ( m_Guild.BadLeader( m_Leader ) )
			{
				return;
			}
			else if ( m_Target.Disbanded || m_Guild.IsWar( m_Target ) )
			{
				return;
			}

			if ( m_Guild.WarDeclarations.Count > 0 || m_Guild.WarInvitations.Count > 0 )
			{
				from.SendLocalizedMessage( 1063427, m_Guild.Name ); // ~1_val~ is currently involved in a guild war.

				return;
			}

			if ( m_Target.WarDeclarations.Count > 0 || m_Target.WarInvitations.Count > 0 )
			{
				from.SendLocalizedMessage( 1063427, m_Target.Name ); // ~1_val~ is currently involved in a guild war.

				return;
			}

			text = text.Trim();

			if ( text.Length > 20 )
			{
				text = text.Substring( 0, 20 );
			}

			if ( text.Length > 0 )
			{
				m_Guild.AllianceName = text;
			}

			if ( !m_Guild.AllyDeclarations.Contains( m_Target ) )
			{
				m_Guild.AllyDeclarations.Add( m_Target );

				from.SendLocalizedMessage( 1070750, m_Target.Name ); // An invitation to join your alliance has been sent to ~1_val~.

				m_Target.Leader.SendLocalizedMessage( 1070780, m_Guild.Name ); // ~1_val~ has proposed an alliance.
			}

			if ( !m_Target.AllyInvitations.Contains( m_Guild ) )
			{
				m_Target.AllyInvitations.Add( m_Guild );
			}

			m_Leader.CloseGump( typeof( DiplomacyMiscGump ) );
		}
	}
}