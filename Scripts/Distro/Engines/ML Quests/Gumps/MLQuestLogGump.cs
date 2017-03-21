using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Quests
{
	public class MLQuestLogGump : BaseQuestGump
	{
		public override int TypeID { get { return 0x329; } }

		private BaseQuest m_Quest;

		public MLQuestLogGump( BaseQuest quest )
			: base( 75, 25 )
		{
			m_Quest = quest;

			Intern( "" );

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
			AddLabelIntern( 100, 50, 0x481, 0 );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15A9 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 90, 33, 0x232D );
			AddHtmlLocalized( 130, 45, 270, 16, 1046026, 0xFFFFFF, false, false ); // Quest Log
			AddImageTiled( 130, 65, 175, 1, 0x238D );
			AddButton( 95, 395, 0x2EF5, 0x2EF7, 1, GumpButtonType.Reply, 0 ); // Resign
			AddButton( 313, 395, 0x2EEC, 0x2EEE, 2, GumpButtonType.Reply, 0 ); // Okay

			if ( m_Quest.Failed )
				AddHtmlLocalized( 160, 80, 250, 16, 500039, 0x3C00, false, false ); // Failed!

			AddHtmlLocalized( 130, 68, 220, 48, 1114513, String.Format( "#{0}", quest.Title.ToString() ), 0x2710, false, false );

			if ( m_Quest.ChainID != QuestChain.None )
				AddHtmlLocalized( 98, 140, 312, 16, 1075024, 0x2710, false, false ); // Description (quest chain)
			else
				AddHtmlLocalized( 98, 140, 312, 16, 1072202, 0x2710, false, false ); // Description

			AddHtmlObject( 98, 156, 312, 180, quest.Description, LightGreen, false, true );
			AddButton( 275, 370, 0x2EE9, 0x2EEB, 0, GumpButtonType.Page, 2 ); // Continue

			AddPage( 2 );

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
			AddLabelIntern( 100, 50, 0x481, 0 );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15A9 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 90, 33, 0x232D );

			AddHtmlLocalized( 130, 45, 270, 16, 1046026, 0xFFFFFF, false, false ); // Quest Log

			AddImageTiled( 130, 65, 175, 1, 0x238D );

			AddButton( 95, 395, 0x2EF5, 0x2EF7, 1, GumpButtonType.Reply, 0 );
			AddButton( 313, 395, 0x2EEC, 0x2EEE, 2, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 130, 68, 220, 48, 1114513, String.Format( "#{0}", quest.Title.ToString() ), 0x2710, false, false );

			AddButton( 275, 370, 0x2EE9, 0x2EEB, 0, GumpButtonType.Page, 2 );
			AddButton( 130, 370, 0x2EEF, 0x2EF1, 0, GumpButtonType.Page, 1 );

			AddHtmlLocalized( 98, 140, 312, 16, 1049073, 0x2710, false, false ); // Objective:

			if ( m_Quest.AllObjectives )
				AddHtmlLocalized( 98, 156, 312, 16, 1072208, 0x2710, false, false ); // All of the following	
			else
				AddHtmlLocalized( 98, 156, 312, 16, 1072209, 0x2710, false, false ); // Only one of the following

			int offset = 172;
			int internidx = 0;

			for ( int i = 0; i < m_Quest.Objectives.Count; i++ )
			{
				int field = 1;

				BaseObjective objective = m_Quest.Objectives[i];

				if ( objective is ObtainObjective )
				{
					ObtainObjective obtain = (ObtainObjective) objective;
					string returnTo = ReturnTo();

					AddKRHtmlLocalized( 0, 0, 0, 0, -3, false, false );

					/*1*/
					Intern( obtain.MaxProgress.ToString() );
					/*2*/
					Intern( obtain.Name );
					/*3*/
					Intern( obtain.Timed ? obtain.Seconds.ToString() : "" );
					/*4*/
					Intern( "" );
					/*5*/
					Intern( "" );
					/*6*/
					Intern( obtain.CurProgress.ToString() );
					/*7*/
					Intern( returnTo );

					AddHtmlLocalized( 98, offset, 350, 16, 1072205, 0x15F90, false, false ); // Obtain
					AddLabelIntern( 143, offset, 0x481, 1 + ( internidx * 7 ) );

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddLabelIntern( 173, offset, 0x481, 2 + ( internidx * 7 ) );

					if ( obtain.Image > 0 )
						AddItem( 350, offset, obtain.Image ); // Image

					offset += 16;

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );

					if ( obtain.Timed )
					{
						AddHtmlLocalized( 103, offset, 120, 16, 1062379, 0x15F90, false, false ); // Est. time remaining:
						AddLabelIntern( 223, offset, 0x481, 3 + ( internidx * 7 ) ); // %est. time remaining%

						offset += 16;
					}
					else
					{
						AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
						AddKRLabel( 0, 0, 0, 0, 3 + ( internidx * 7 ), false, false );
					}

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRLabel( 0, 0, 0, 0, 4 + ( internidx * 7 ), false, false );


					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRLabel( 0, 0, 0, 0, 5 + ( internidx * 7 ), false, false );

					AddHtmlLocalized( 103, offset, 120, 16, 3000087, 0x15F90, false, false ); // Total
					AddLabelIntern( 223, offset, 0x481, 6 + ( internidx * 7 ) );

					offset += 16;

					AddHtmlLocalized( 103, offset, 120, 16, 1074782, 0x15F90, false, false ); // Return to
					AddLabelIntern( 223, offset, 0x481, 7 + ( internidx * 7 ) );

					offset += 16;
				}
				else if ( objective is SlayObjective )
				{
					SlayObjective slay = (SlayObjective) objective;
					int internOffset = internidx * 8;
					string returnTo = ReturnTo();

					AddKRHtmlLocalized( 0, 0, 0, 0, -2, false, false );

					/*1*/
					Intern( slay.MaxProgress.ToString() );
					/*2*/
					Intern( slay.Name );
					/*3*/
					Intern( slay.Timed ? slay.Seconds.ToString() : "" );
					/*4*/
					Intern( slay.Region != null ? slay.Region.Name : "" );
					/*5*/
					Intern( "" );
					/*6*/
					Intern( "" );
					/*7*/
					Intern( slay.CurProgress.ToString() );
					/*8*/
					Intern( returnTo );

					AddHtmlLocalized( 98, offset, 312, 16, 1072204, 0x15F90, false, false ); // Slay
					AddLabelIntern( 133, offset, 0x481, 1 + internOffset ); // Amount

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddLabelIntern( slay.MaxProgress > 10 ? 163 : 148, offset, 0x481, 2 + internOffset ); // Type

					offset += 16;

					if ( slay.Timed )
					{
						AddHtmlLocalized( 103, offset, 120, 16, 1062379, 0x15F90, false, false ); // Est. time remaining:
						AddLabelIntern( 223, offset, 0x481, 3 + internOffset ); // %est. time remaining%

						offset += 16;
					}
					else
					{
						AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
						AddKRLabel( 0, 0, 0, 0, 3 + internOffset, false, false );
					}

					if ( slay.Region != null )
					{
						AddHtmlLocalized( 103, offset, 312, 20, 1018327, 0x15F90, false, false ); // Location
						AddLabelIntern( 223, offset, 0x481, 4 + internOffset ); // %est. time remaining%

						offset += 16;
					}
					else
					{
						AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
						AddKRLabel( 0, 0, 0, 0, 4 + internOffset, false, false );
					}

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRLabel( 0, 0, 0, 0, 5 + internOffset, false, false );

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRLabel( 0, 0, 0, 0, 6 + internOffset, false, false );

					AddHtmlLocalized( 103, offset, 120, 16, 3000087, 0x15F90, false, false ); // Total
					AddLabelIntern( 223, offset, 0x481, 7 + internOffset );

					offset += 16;

					AddHtmlLocalized( 103, offset, 120, 16, 1074782, 0x15F90, false, false ); // Return to
					AddLabelIntern( 223, offset, 0x481, 8 + internOffset );

					offset += 16;
				}
				else if ( objective is DeliverObjective )
				{
					DeliverObjective deliver = (DeliverObjective) objective;
					int internOffset = internidx * 5;

					AddKRHtmlLocalized( 0, 0, 0, 0, -5, false, false );

					AddHtmlLocalized( 98, offset, 312, 16, 1072207, 0x15F90, false, false ); // Deliver

					Intern( deliver.MaxProgress.ToString() );
					Intern( deliver.DeliveryName );

					AddLabelIntern( 143, offset, 0x481, internOffset + 1 );
					AddLabelIntern( 158, offset, 0x481, internOffset + 2 );

					offset += 16;

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );

					if ( deliver.Timed )
					{
						AddHtmlLocalized( 103, offset, 120, 16, 1062379, 0x15F90, false, false ); // Est. time remaining:
						Intern( deliver.Seconds.ToString() );
						AddLabelIntern( 223, offset, 0x481, internOffset + 3 ); // %est. time remaining%

						offset += 16;
					}
					else
					{
						AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
						AddKRLabel( 0, 0, 0, 0, internOffset + 3, false, false );
						Intern( "" );
					}

					AddHtmlLocalized( 103, offset, 120, 16, 1072379, 0x15F90, false, false ); // Deliver to
					Intern( deliver.DestName );
					AddLabelIntern( 223, offset, 0x481, internOffset + 4 );

					offset += 16;

					AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
					AddKRLabel( 0, 0, 0, 0, internOffset + 5, false, false );
					Intern( "" );
				}
				else if ( objective is EscortObjective )
				{
					EscortObjective escort = (EscortObjective) objective;

					AddKRHtmlLocalized( 0, 0, 0, 0, -4, false, false );

					AddHtmlLocalized( 98, offset, 312, 16, 1072206, 0x15F90, false, false ); // Escort to
					AddHtmlObject( 173, offset, 312, 20, escort.Region.Name, 0xFFFFFF, false, false );

					offset += 16;

					if ( escort.Timed )
					{
						AddHtmlLocalized( 103, offset, 120, 16, 1062379, 0x15F90, false, false ); // Est. time remaining:

						Intern( escort.Seconds.ToString() );

						AddLabelIntern( 223, offset, 0x481, ( internidx * 3 ) + ( field++ ) ); // %est. time remaining%

						offset += 16;
					}

					for ( int j = field; j < 3; j++ )
					{
						AddKRHtmlLocalized( 0, 0, 0, 0, 1078089, false, false );
						AddKRLabel( 0, 0, 0, 0, j, false, false );
						Intern( "" );
					}
				}
				else if ( objective is ApprenticeObjective )
				{
					ApprenticeObjective apprentice = (ApprenticeObjective) objective;

					AddKRHtmlLocalized( 0, 0, 0, 0, -9, false, false );

					AddHtmlLocalized( 98, offset, 200, 16, 1077485, "#" + ( 1044060 + (int) apprentice.Skill ) + "\t" + apprentice.MaxProgress, 0x15F90, false, false ); // Increase ~1_SKILL~ to ~2_VALUE~

					offset += 16;
				}
				else if ( objective is BaseBardObjective )
				{
					BaseBardObjective bardObjective = (BaseBardObjective) objective;

					AddHtmlLocalized( 98, offset, 200, 16, bardObjective.Cliloc, 0x15F90, false, false );

					offset += 16;
				}

				internidx++;
			}

			AddButton( 275, 370, 0x2EE9, 0x2EEB, 0, GumpButtonType.Page, 3 );

			AddPage( 3 );

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
			AddLabelIntern( 100, 50, 0x481, 0 );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15A9 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 90, 33, 0x232D );
			AddHtmlLocalized( 130, 45, 270, 16, 1046026, 0xFFFFFF, false, false ); // Quest Log
			AddImageTiled( 130, 65, 175, 1, 0x238D );
			AddButton( 95, 395, 0x2EF5, 0x2EF7, 1, GumpButtonType.Reply, 0 );
			AddButton( 313, 395, 0x2EEC, 0x2EEE, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 130, 68, 220, 48, 1114513, String.Format( "#{0}", quest.Title.ToString() ), 0x2710, false, false );
			AddButton( 130, 370, 0x2EEF, 0x2EF1, 0, GumpButtonType.Page, 2 );
			AddHtmlLocalized( 98, 140, 312, 16, 1072201, 0x2710, false, false ); // Reward

			if ( quest.Rewards.Count == 1 )
			{
				BaseReward reward = m_Quest.Rewards[0];

				AddImage( 105, 163, 0x4B9 );
				AddHtmlObject( 133, 162, 280, 32, reward.Name, 0x15F90, false, false );
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
				sender.Mobile.SendGump( new MondainResignGump( m_Quest ) );
		}

		public virtual string ReturnTo()
		{
			if ( m_Quest == null )
				return null;

			if ( m_Quest.StartingMobile != null )
			{
				string returnTo = m_Quest.StartingMobile.Name;

				if ( m_Quest.StartingMobile.Region != null )
					returnTo = String.Format( "{0} ({1})", returnTo, m_Quest.StartingMobile.Region.Name );
				else
					returnTo = String.Format( "{0}", returnTo );

				return returnTo;
			}

			return "(null)";
		}
	}
}