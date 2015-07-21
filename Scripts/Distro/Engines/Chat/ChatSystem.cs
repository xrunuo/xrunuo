using System;
using Server;
using Server.Misc;
using Server.Network;
using Server.Accounting;

namespace Server.Engines.Chat
{
	public class ChatSystem
	{
		public static readonly bool Enabled = true;

		public static readonly bool AllowCreateChannels = false;
		public static readonly string DefaultChannel = "General";

		public static void Initialize()
		{
			PacketHandlers.Instance.Register( 0xB5, 0x40, true, new OnPacketReceive( OpenChatWindowRequest ) );
			PacketHandlers.Instance.Register( 0xB3, 0, true, new OnPacketReceive( ChatAction ) );
		}

		public static void OpenChatWindowRequest( GameClient state, PacketReader pvSrc )
		{
			Mobile from = state.Mobile;

			if ( !Enabled )
			{
				from.SendMessage( "The chat system has been disabled." );
				return;
			}

			pvSrc.Seek( 2, System.IO.SeekOrigin.Begin );
			/*string chatName = */
			pvSrc.ReadUnicodeStringSafe( ( 0x40 - 2 ) >> 1 ).Trim();

			string chatName = from.Name;

			SendCommandTo( from, ChatCommand.OpenChatWindow, chatName );
			ChatUser.AddChatUser( from );
		}

		public static void ChatAction( GameClient state, PacketReader pvSrc )
		{
			if ( !Enabled )
				return;

			try
			{
				Mobile from = state.Mobile;
				ChatUser user = ChatUser.GetChatUser( from );

				if ( user == null )
					return;

				string lang = pvSrc.ReadStringSafe( 4 );
				int actionId = pvSrc.ReadInt16();
				string param = pvSrc.ReadUnicodeString();

				ChatActionHandler handler = ChatActionHandlers.GetHandler( actionId );

				if ( handler != null )
				{
					Channel channel = user.CurrentChannel;

					if ( handler.RequireConference && channel == null )
					{
						/* You must be in a conference to do this.
						 * To join a conference, select one from the Conference menu.
						 */
						user.SendMessage( 31 );
					}
					else
					{
						handler.Callback( user, channel, param );
					}
				}
				else
				{
					Console.WriteLine( "Client: {0}: Unknown chat action 0x{1:X}: {2}", state, actionId, param );
				}
			}
			catch ( Exception e )
			{
				Logger.Error( e.ToString() );
			}
		}

		public static void SendCommandTo( Mobile to, ChatCommand type, string param1 = null, string param2 = null )
		{
			if ( to != null )
				to.Send( new ChatMessagePacket( null, (int) type + 20, param1, param2 ) );
		}
	}
}
