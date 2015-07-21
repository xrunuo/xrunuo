using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Quests
{
	public class MLQuestMainLogGump : BaseQuestGump
	{
		public override int TypeID { get { return 0x325; } }

		public MLQuestMainLogGump( PlayerMobile from )
			: base( 75, 25 )
		{
			AddPage( 1 );

			Closable = false;

			AddImageTiled( 50, 20, 400, 400, 0x1404 );
			AddImageTiled( 50, 29, 30, 390, 10460 );
			AddImageTiled( 34, 140, 17, 279, 9263 );

			AddImage( 48, 135, 10411 );
			AddImage( -16, 285, 10402 );
			AddImage( 0, 10, 10421 );
			AddImage( 25, 0, 10420 );

			AddImageTiled( 83, 15, 350, 15, 10250 );

			AddImage( 34, 419, 10306 );
			AddImage( 442, 419, 10304 );

			AddImageTiled( 51, 419, 392, 17, 10101 );
			AddImageTiled( 415, 29, 44, 390, 2605 );
			AddImageTiled( 415, 29, 30, 390, 10460 );

			AddLabel( 100, 50, 0x481, "" );

			AddImage( 370, 50, 1417 );
			AddImage( 379, 60, 9012 );
			AddImage( 425, 0, 10441 );
			AddImage( 90, 33, 9005 );

			AddHtmlLocalized( 130, 45, 270, 16, 1046026, 0xFFFFFF, false, false ); // Quest Log
			AddImageTiled( 130, 65, 175, 1, 0x238D );

			AddButton( 313, 395, 12012, 12014, 0, GumpButtonType.Reply, 0 );

			int offset = 140;

			for ( int i = 0; i < from.Quests.Count; i++ )
			{
				BaseQuest quest = from.Quests[i];

				AddHtmlObject( 98, offset, 270, 21, quest.Title, quest.Failed ? 0x3C00 : 0xFFFFFF, false, false ); // Quest Name
				AddButton( 368, offset, 0x26B0, 0x26B1, i + 1, GumpButtonType.Reply, 0 );

				offset += 21;
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			PlayerMobile from = (PlayerMobile) sender.Mobile;

			int questID = info.ButtonID - 1;

			// lo hacemos de esta manera para evitar exploits por
			// ejemplo, mandar un buttonID modificado para romper
			// el rango de la lista de quests.
			if ( questID >= 0 && questID < from.Quests.Count )
				from.SendGump( new MLQuestLogGump( from.Quests[questID] ) );
		}
	}
}
