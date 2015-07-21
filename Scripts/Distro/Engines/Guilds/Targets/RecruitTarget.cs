using Server.Accounting;
using Server.Engines.Guilds.Gumps;
using Server.Factions;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Guilds.Targets
{
	public class InviteTarget : Target
	{
		private Mobile m_Mobile;
		private Guild m_Guild;

		public InviteTarget( Mobile m, Guild guild )
			: base( 10, false, TargetFlags.None )
		{
			m_Mobile = m;
			m_Guild = guild;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			int rank = ( from as PlayerMobile ).GuildRank;

			if ( rank != 3 && rank != 5 )
			{
				return;
			}

			if ( targeted is Mobile )
			{
				Mobile m = (Mobile) targeted;

				PlayerState guildState = PlayerState.Find( m_Guild.Leader );
				PlayerState targetState = PlayerState.Find( m );

				Faction guildFaction = ( guildState == null ? null : guildState.Faction );
				Faction targetFaction = ( targetState == null ? null : targetState.Faction );

				Account targetAccount = m.Account as Account;

				if ( !m.IsPlayer )
				{
					m_Mobile.SendLocalizedMessage( 1063334 ); // That isn't a valid player.
				}
				else if ( !m.Alive )
				{
					m_Mobile.SendLocalizedMessage( 501162 ); // Only the living may be recruited.   // need test ?
				}
				else if ( m_Mobile == m )
				{
					m_Mobile.SendLocalizedMessage( 502128 ); // You flatter yourself.
				}
				else if ( !( m as PlayerMobile ).AcceptGuildInvites )
				{
					m_Mobile.SendLocalizedMessage( 1063049, m.Name ); // ~1_val~ is not accepting guild invitations.
				}
				else if ( m_Guild.IsMember( m ) )
				{
					m_Mobile.SendLocalizedMessage( 1063050, m.Name ); // ~1_val~ is already a member of your guild!
				}
				else if ( m.Guild != null )
				{
					m_Mobile.SendLocalizedMessage( 1063051, m.Name ); // ~1_val~ is already a member of a guild.
				}
				else if ( m.Client == null )
				{
					m_Mobile.SendMessage( "El jugador debe estar online para aceptar invitaciones." );
				}
				#region Factions
				else if ( guildFaction != null && ( targetAccount == null || targetAccount.Trial ) )
				{
					m_Mobile.SendLocalizedMessage( 1111856 ); // You cannot invite a trial account player to your faction-aligned guild.
				}
				else if ( guildFaction != targetFaction )
				{
					if ( guildFaction == null )
					{
						m_Mobile.SendLocalizedMessage( 1013027 ); // That player cannot join a non-faction guild.
					}
					else if ( targetFaction == null )
					{
						m_Mobile.SendLocalizedMessage( 1013026 ); // That player must be in a faction before joining this guild.
					}
					else
					{
						m_Mobile.SendLocalizedMessage( 1013028 ); // That person has a different faction affiliation.
					}
				}
				else if ( targetState != null && targetState.IsLeaving )
				{
					// OSI does this quite strangely, so we'll just do it this way
					m_Mobile.SendMessage( "That person is quitting their faction and so you may not recruit them." );
				}
				#endregion
				else
				{
					m_Guild.Accepted.Add( m );
					m.SendGump( new InviteGump( m, m_Mobile, m_Guild ) );
				}
			}
			else
			{
				m_Mobile.SendLocalizedMessage( 1063334 ); // That isn't a valid player.
			}
		}

		protected override void OnTargetFinish( Mobile from )
		{
			int rank = ( (PlayerMobile) from ).GuildRank;

			if ( rank != 3 && rank != 5 )
				return;

			m_Mobile.CloseGump( typeof( RosterGump ) );
		}
	}
}