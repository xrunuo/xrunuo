using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Quests
{
	public class MLQuestRewardGump : BaseQuestGump
	{
		public override int TypeID { get { return 0x323; } }

		private BaseQuest m_Quest;

		public MLQuestRewardGump( BaseQuest quest )
			: base( 75, 25 )
		{
			m_Quest = quest;

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
			AddHtmlLocalized( 130, 45, 270, 16, 1072201, 0xFFFFFF, false, false ); // Reward
			AddImageTiled( 130, 65, 175, 1, 0x238D );
			AddButton( 95, 395, 0x2EE0, 0x2EE2, 1, GumpButtonType.Reply, 0 ); // Accept
			AddHtmlLocalized( 130, 68, 220, 48, 1114513, String.Format( "#{0}", quest.Title.ToString() ), 0x2710, false, false );
			AddHtmlLocalized( 98, 140, 312, 16, 1072201, 0x2710, false, false ); // Reward

			if ( quest.Rewards.Count == 1 )
			{
				BaseReward reward = m_Quest.Rewards[0];

				AddImage( 107, 147, 0x4B9 );
				AddHtmlObject( 135, 146, 280, 32, reward.Name, 0x15F90, false, false );
			}
			else
			{
				AddHtmlLocalized( 98, 156, 312, 16, 1072208, 0x2710, false, false ); // All of the following

				for ( int n = 0; n < quest.Rewards.Count; ++n )
				{
					BaseReward reward = m_Quest.Rewards[n];

					AddImage( 105, 179 + ( n * 16 ), 0x4B9 );
					AddHtmlObject( 133, 178 + ( n * 16 ), 280, 32, reward.Name, 0x15F90, false, false );
				}
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
				m_Quest.GiveRewards();
		}
	}
}
