using System.Collections;
using Server.Network;

namespace Server.Engines.Guilds
{
	public class GuildChatHandlers : GuildChat
	{
		public static void Initialize()
		{
			GuildChat.Handler = new GuildChatHandlers();
		}

		public override void OnGuildMessage( Mobile from, string text )
		{
			if ( text.Length > 128 || ( text = text.Trim() ).Length == 0 )
			{
				return;
			}

			Guild guild = (Guild) from.Guild;

			if ( guild != null )
			{
				SendGuildMessage( from, text );
			}
			else
			{
				from.SendLocalizedMessage( 1063142 ); // You are not in a guild!
			}
		}

		public override void OnAllianceMessage( Mobile from, string text )
		{
			if ( text.Length > 128 || ( text = text.Trim() ).Length == 0 )
			{
				return;
			}

			Guild guild = (Guild) from.Guild;

			if ( guild != null )
			{
				if ( guild.Allies.Count > 0 )
				{
					SendAllianceMessage( from, text );
				}
				else
				{
					from.SendLocalizedMessage( 1071020 ); // You are not in an alliance!
				}
			}
			else
			{
				from.SendLocalizedMessage( 1063142 ); // You are not in a guild!
			}
		}

		public void SendGuildMessage( Mobile from, string text )
		{
			Packet p = null;

			ArrayList listeners = new ArrayList();

			Guild guild = (Guild) from.Guild;

			for ( int i = 0; i < guild.Members.Count; ++i )
			{
				Mobile member = (Mobile) guild.Members[i];

				if ( member != null && !listeners.Contains( member ) )
				{
					listeners.Add( member );
				}
			}

			for ( int i = 0; i < listeners.Count; ++i )
			{
				Mobile mob = (Mobile) listeners[i];

				if ( p == null )
				{
					p = Packet.Acquire( new UnicodeMessage( from.Serial, from.Body, MessageType.Guild, from.SpeechHue, 3, from.Language, from.Name, text ) );
				}

				if ( mob != null )
				{
					mob.Send( p );
				}
			}

			Packet.Release( p );
		}

		public void SendAllianceMessage( Mobile from, string text )
		{
			Packet p = null;

			ArrayList listeners = new ArrayList();

			Guild guild = (Guild) from.Guild;

			for ( int i = 0; i < guild.Members.Count; ++i )
			{
				Mobile member = (Mobile) guild.Members[i];

				if ( member != null && !listeners.Contains( member ) )
				{
					listeners.Add( member );
				}
			}

			for ( int i = 0; i < guild.Allies.Count; ++i )
			{
				Guild g = (Guild) guild.Allies[i];

				if ( g != null )
				{
					for ( int j = 0; j < g.Members.Count; ++j )
					{
						Mobile member = (Mobile) g.Members[j];

						if ( member != null && !listeners.Contains( member ) )
						{
							listeners.Add( member );
						}
					}
				}
			}

			for ( int i = 0; i < listeners.Count; ++i )
			{
				Mobile mob = (Mobile) listeners[i];

				if ( p == null )
				{
					p = Packet.Acquire( new UnicodeMessage( from.Serial, from.Body, MessageType.Alliance, from.SpeechHue, 3, from.Language, from.Name, text ) );
				}

				if ( mob != null )
				{
					mob.Send( p );
				}
			}

			Packet.Release( p );
		}
	}
}
