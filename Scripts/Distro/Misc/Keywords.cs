using System;
using Server;
using Server.Engines.Guilds;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
using Server.Gumps;
using Server.Events;

namespace Server.Misc
{
	public class Keywords
	{
		public static void Initialize()
		{
			// Register our speech handler
			EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
		}

		public static void EventSink_Speech( SpeechEventArgs args )
		{
			Mobile from = args.Mobile;
			int[] keywords = args.Keywords;

			for ( int i = 0; i < keywords.Length; ++i )
			{
				switch ( keywords[i] )
				{
					case 0x002A: // *i resign from my guild*
						{
							if ( from.Guild != null )
								( (Guild) from.Guild ).RemoveMember( from );

							break;
						}
					case 0x0032: // *i must consider my sins*
						{
							from.SendMessage( "Short Term Murders : {0}", from.ShortTermMurders );
							from.SendMessage( "Long Term Murders : {0}", from.Kills );
							from.SendMessage( "Fame : {0}", from.Fame );
							from.SendMessage( "Karma : {0}", from.Karma );

							break;
						}
					case 0x0035: // i renounce my young player status*
						{
							if ( from is PlayerMobile && ( (PlayerMobile) from ).Young && !from.HasGump<RenounceYoungGump>() )
							{
								from.SendGump( new RenounceYoungGump() );
							}

							break;
						}
				}
			}
		}
	}
}