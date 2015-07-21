using System;

namespace Server.Engines.Chat
{
	public class ChatActionHandlers
	{
		private static ChatActionHandler[] m_Handlers;

		static ChatActionHandlers()
		{
			m_Handlers = new ChatActionHandler[0x100];

			Register( 0x43, true, new OnChatAction( LeaveChannel ) );
			Register( 0x61, true, new OnChatAction( ChannelMessage ) );
			Register( 0x62, false, new OnChatAction( JoinChannel ) );
			Register( 0x63, false, new OnChatAction( CreateChannel ) );
		}

		public static void Register( int actionID, bool requireConference, OnChatAction callback )
		{
			if ( actionID >= 0 && actionID < m_Handlers.Length )
				m_Handlers[actionID] = new ChatActionHandler( requireConference, callback );
		}

		public static ChatActionHandler GetHandler( int actionID )
		{
			if ( actionID >= 0 && actionID < m_Handlers.Length )
				return m_Handlers[actionID];

			return null;
		}

		public static void ChannelMessage( ChatUser from, Channel channel, string param )
		{
			channel.SendMessage( 57, from, from.GetColorCharacter() + from.Username, String.Format( "{{{0}}} {1}", channel.Name, param ) ); // %1: %2
			ChatLogging.LogMessage( channel.Name, from.Username, param );
		}

		public static void LeaveChannel( ChatUser from, Channel channel, string param )
		{
			channel.RemoveUser( from );
		}

		public static void JoinChannel( ChatUser from, Channel channel, string param )
		{
			string name;
			string password = null;

			int start = param.IndexOf( '\"' );

			if ( start >= 0 )
			{
				int end = param.IndexOf( '\"', ++start );

				if ( end >= 0 )
				{
					name = param.Substring( start, end - start );
					password = param.Substring( ++end );
				}
				else
				{
					name = param.Substring( start );
				}
			}
			else
			{
				int indexOf = param.IndexOf( ' ' );

				if ( indexOf >= 0 )
				{
					name = param.Substring( 0, indexOf++ );
					password = param.Substring( indexOf );
				}
				else
				{
					name = param;
				}
			}

			CreateAndJoin( from, name );
		}

		public static void CreateChannel( ChatUser from, Channel channel, string param )
		{
			CreateAndJoin( from, param );
		}

		private static void CreateAndJoin( ChatUser from, string name )
		{
			Channel joined = Channel.FindChannelByName( name );

			if ( joined == null )
			{
				if ( ChatSystem.AllowCreateChannels )
				{
					from.Mobile.SendMessage( "You have created the channel {0}", name );
					joined = Channel.AddChannel( name );
				}
				else
				{
					from.Mobile.SendMessage( "Channel creation is not allowed right now. Switching to default channel..." );
					joined = Channel.Default;
				}
			}

			joined.AddUser( from );
		}
	}
}
