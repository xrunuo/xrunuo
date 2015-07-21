using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Quests
{
	public enum MLQuestConverType
	{
		Refuse,
		InProgress
	}

	public class MLQuestConversationGump : BaseQuestGump
	{
		public override int TypeID { get { return 0x326; } }

		public MLQuestConversationGump( BaseQuest quest, MLQuestConverType converType )
			: base( 75, 25 )
		{
			AddPage( 1 );

			Closable = false;
			AddImageTiled( 50, 20, 400, 400, 0x1404 );
			AddImageTiled( 50, 29, 30, 390, 0x28DC );
			AddImageTiled( 34, 140, 17, 279, 0x242F );
			AddImage( 48, 135, 0x28AB );
			AddImage( -16, 285, 0x28A2 );
			AddImage( 0, 10, 0x28B5 );
			AddImage( 25, 0, 0x28B4 );
			AddImageTiled( 83, 15, 350, 15, 0x280A );
			AddImage( 34, 419, 0x2842 );
			AddImage( 442, 419, 0x2840 );
			AddImageTiled( 51, 419, 392, 17, 0x2775 );
			AddImageTiled( 415, 29, 44, 390, 0xA2D );
			AddImageTiled( 415, 29, 30, 390, 0x28DC );
			AddLabel( 100, 50, 0x481, "" );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15A9 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 90, 33, 0x232D );
			AddHtmlLocalized( 130, 45, 270, 16, 3006156, 0xFFFFFF, false, false ); // Quest Conversation
			AddImageTiled( 130, 65, 175, 1, 0x238D );
			AddHtmlLocalized( 130, 68, 220, 48, 1114513, String.Format( "#{0}", quest.Title.ToString() ), 0x2710, false, false );

			switch ( converType )
			{
				case MLQuestConverType.Refuse:
					{
						AddHtmlObject( 98, 140, 312, 180, quest.Refuse, 0x15F90, false, true );
						AddButton( 313, 395, 0x2EE6, 0x2EE8, 3, GumpButtonType.Reply, 0 );
						break;
					}
				case MLQuestConverType.InProgress:
					{
						AddHtmlObject( 98, 140, 312, 180, quest.Uncomplete, 0x15F90, false, true );
						AddButton( 313, 395, 0x2EE6, 0x2EE8, 3, GumpButtonType.Reply, 0 ); // Close
						break;
					}
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
		}
	}
}
