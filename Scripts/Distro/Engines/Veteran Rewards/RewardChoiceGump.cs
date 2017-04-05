using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.VeteranRewards
{
	public class RewardChoiceGump : Gump
	{
		public override int TypeID { get { return 0x1C6; } }

		private Mobile m_From;

		private void RenderBackground()
		{
			AddPage( 0 );

			AddBackground( 10, 10, 600, 450, 2600 );

			AddButton( 530, 415, 4017, 4019, 0, GumpButtonType.Reply, 0 );

			AddButton( 60, 415, 4014, 4016, 0, GumpButtonType.Page, 1 );
			AddHtmlLocalized( 95, 415, 200, 20, 1049755, false, false ); // Main Menu
		}

		private void RenderCategories()
		{
			int cur, max;

			RewardSystem.ComputeRewardInfo( m_From, out cur, out max );

			Intern( ( max - cur ).ToString() );
			Intern( cur.ToString() );

			TimeSpan rewardInterval = RewardSystem.RewardInterval;

			object interval;

			if ( rewardInterval == TimeSpan.FromDays( 90.0 ) )
				interval = "three months";
			else if ( rewardInterval == TimeSpan.FromDays( 360.0 ) )
				interval = 1006003;
			else if ( rewardInterval == TimeSpan.FromDays( 720.0 ) )
				interval = 1006004;
			else if ( rewardInterval == TimeSpan.FromDays( 1080.0 ) )
				interval = 1006005;
			else
				interval = String.Format( "{0} day{1}", rewardInterval.TotalDays, rewardInterval.TotalDays == 1 ? "" : "s" );

			AddPage( 1 );

			if ( interval is int )
			{
				AddHtmlLocalized( 60, 35, 500, 70, (int) interval, true, true );
			}
			else if ( interval is String )
			{
				AddHtmlLocalized( 60, 35, 500, 70, 1034379, true, true );
				AddHtml( 60, 35, 500, 70, "<B>Ultima Online Rewards Program</B><BR>" +
					"Thank you for being a part of the Ultima Online community for a full " + (string) interval + ".  " +
					"As a token of our appreciation,  you may select from the following in-game reward items listed below.  " +
					"The gift items will be attributed to the character you have logged-in with on the shard you are on when you chose the item(s).  " +
					"The number of rewards you are entitled to are listed below and are for your entire account.  " +
					"To read more about these rewards before making a selection, feel free to visit the uo.com site at " +
					"<A HREF=\"http://www.uo.com/rewards\">http://www.uo.com/rewards</A>.", true, true );
			}

			AddHtmlLocalized( 60, 105, 300, 35, 1006006, false, false ); // Your current total of rewards to choose:
			AddHtmlLocalized( 60, 140, 300, 35, 1006007, false, false ); // You have already chosen:

			AddLabelIntern( 370, 107, 50, 0 );
			AddLabelIntern( 370, 142, 50, 1 );

			RewardCategory[] categories = RewardSystem.Categories;

			for ( int i = 0; i < categories.Length; ++i )
			{
				if ( categories[i].Entries.Count == 0 )
					continue;

				if ( !RewardSystem.HasAccess( m_From, (RewardEntry) categories[i].Entries[0] ) )
					continue;

				AddButton( 100, 180 + ( i * 40 ), 4005, 4005, 0, GumpButtonType.Page, 2 + i );

				if ( categories[i].NameString != null )
					AddHtml( 135, 180 + ( i * 40 ), 300, 20, categories[i].NameString, false, false );
				else
					AddHtmlLocalized( 135, 180 + ( i * 40 ), 300, 20, categories[i].Name, false, false );
			}

			for ( int i = 0; i < categories.Length; ++i )
				RenderCategory( categories[i], i );
		}

		private int GetButtonID( int type, int index )
		{
			return 2 + ( index * 20 ) + type;
		}

		private void RenderCategory( RewardCategory category, int index )
		{
			//bool twopages = false;
			int num_pages = 1;

			AddPage( 2 + index );

			ArrayList entries = category.Entries;

			for ( int idx = 0; idx < entries.Count; ++idx )
			{
				//int i = twopages ? idx - 24 : idx;
				int i = num_pages > 1 ? idx - ( 24 * ( num_pages - 1 ) ) : idx;


				RewardEntry entry = (RewardEntry) entries[idx];

				if ( !RewardSystem.HasAccess( m_From, entry ) )
					break;

				AddButton( 55 + ( ( i / 12 ) * 250 ), 80 + ( ( i % 12 ) * 25 ), 5540, 5541, GetButtonID( index, idx ), GumpButtonType.Reply, 0 );

				if ( entry.NameString != null )
					AddHtml( 80 + ( ( i / 12 ) * 250 ), 80 + ( ( i % 12 ) * 25 ), 250, 20, entry.NameString, false, false );
				else
					AddHtmlLocalized( 80 + ( ( i / 12 ) * 250 ), 80 + ( ( i % 12 ) * 25 ), 250, 20, entry.Name, false, false );

				if ( i == 23 )
				{

					AddButton( 315, 415, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 7 + index + ( num_pages - 1 ) + ( index > 1 ? 2 : 0 ) );
					AddHtmlLocalized( 350, 415, 200, 20, 1011066, false, false );

					AddPage( 7 + index + ( num_pages - 1 ) + ( index > 1 ? 2 : 0 ) );

					AddButton( 190, 415, 0xFAE, 0xFB0, 0, GumpButtonType.Page, num_pages == 1 ? index + 2 : 5 + index + num_pages );
					AddHtmlLocalized( 225, 415, 200, 20, 1011067, false, false );

					//twopages = true;
					num_pages++;
				}



				//if (num_pages > 1) //(idx == entries.Count - 1 && num_pages > 1) // twopages )
				//{
				//    AddButton(270, 415, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 2 + index);
				//    AddHtmlLocalized(305, 415, 200, 20, 1011067, false, false);
				//}
			}
		}

		public RewardChoiceGump( Mobile from )
			: base( 0, 0 )
		{
			m_From = from;

			from.CloseGump<RewardChoiceGump>();

			RenderBackground();
			RenderCategories();
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int buttonID = info.ButtonID - 1;

			if ( buttonID == 0 )
			{
				int cur, max;

				RewardSystem.ComputeRewardInfo( m_From, out cur, out max );

				if ( cur < max )
					m_From.SendGump( new RewardNoticeGump( m_From ) );
			}
			else
			{
				--buttonID;

				int type = ( buttonID % 20 );
				int index = ( buttonID / 20 );

				RewardCategory[] categories = RewardSystem.Categories;

				if ( type >= 0 && type < categories.Length )
				{
					RewardCategory category = categories[type];

					if ( index >= 0 && index < category.Entries.Count )
					{
						RewardEntry entry = (RewardEntry) category.Entries[index];

						if ( !RewardSystem.HasAccess( m_From, entry ) )
							return;

						m_From.SendGump( new RewardConfirmGump( m_From, entry ) );
					}
				}
			}
		}
	}
}