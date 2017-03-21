using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Collections
{
	public class SelectRewardTitleGump : Gump
	{
		private PlayerMobile m_Mobile;
		private int m_TitleIndex;

		public SelectRewardTitleGump( PlayerMobile pm, int titleindex )
			: base( 50, 50 )
		{
			m_TitleIndex = titleindex;
			m_Mobile = pm;

			int title = 0;

			if ( titleindex < pm.CollectionTitles.Count )
				title = (int) pm.CollectionTitles[titleindex];

			AddPage( 0 );

			AddBackground( 0, 0, 270, 120, 0x13BE );
			AddBackground( 10, 10, 250, 100, 0xBB8 );

			AddHtmlLocalized( 20, 15, 230, 20, 1073994, 0x1, false, false ); // Your title will be:
			AddHtmlLocalized( 20, 35, 230, 40, title, 0x32, true, false );

			AddHtmlLocalized( 55, 80, 75, 20, 1073996, 0x1, false, false ); // ACCEPT
			AddButton( 20, 80, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 170, 80, 75, 20, 1073997, 0x1, false, false ); // NEXT
			AddButton( 135, 80, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
			{
				int newtitle = m_TitleIndex + 1;

				if ( newtitle >= m_Mobile.CollectionTitles.Count )
					newtitle = 0;

				m_Mobile.SendGump( new SelectRewardTitleGump( m_Mobile, newtitle ) );
			}
			else if ( info.ButtonID == 2 )
			{
				if ( m_Mobile.CurrentCollectionTitle == m_TitleIndex )
					m_Mobile.SendLocalizedMessage( 1074009 ); // You decide to leave your title as it is.
				else if ( m_TitleIndex == 0 )
					m_Mobile.SendLocalizedMessage( 1074010 ); // You elect to hide your Reward Title.
				else
					m_Mobile.SendLocalizedMessage( 1074008, String.Format( "#{0}", m_Mobile.CollectionTitles[m_TitleIndex].ToString() ) ); // You change your Reward Title to "~1_TITLE~".

				m_Mobile.CurrentCollectionTitle = m_TitleIndex;
			}
			else
			{
				m_Mobile.SendLocalizedMessage( 1074009 ); // You decide to leave your title as it is.
			}
		}
	}
}