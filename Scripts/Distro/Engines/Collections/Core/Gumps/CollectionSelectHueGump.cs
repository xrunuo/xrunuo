using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Collections
{
	public class CollectionSelectHueGump : Gump
	{
		public override int TypeID { get { return 0x4268; } }

		private CollectionController m_Collection;
		private IRewardEntry m_Entry;
		private Mobile m_From;
		private int m_Points;
		private int m_ItemID;

		public CollectionSelectHueGump( CollectionController collection, IRewardEntry entry, Mobile from )
			: base( 250, 50 )
		{
			m_Collection = collection;
			m_Entry = entry;
			m_From = from;

			m_Points = (int) m_Collection.Table[from];

			// Cabecera
			AddPage( 0 );

			AddImage( 0, 0, 0x1F40 );
			AddImageTiled( 20, 37, 300, 308, 0x1F42 );
			AddImage( 20, 325, 0x1F43 );
			AddImage( 35, 8, 0x39 );
			AddImageTiled( 65, 8, 257, 10, 0x3A );
			AddImage( 290, 8, 0x3B );
			AddImage( 32, 33, 0x2635 );
			AddImageTiled( 70, 55, 230, 2, 0x23C5 );
			AddHtmlLocalized( 70, 35, 270, 20, 1072835, 0x1, false, false ); // Community Collection

			AddButton( 50, 335, 0x15E3, 0x15E7, -1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 75, 335, 100, 20, 1072842, 0x1, false, false ); // Rewards
			// Fin Cabecera

			int page = 1;
			bool shouldadd = true;
			int y = 130;

			if ( m_Entry.Type.IsSubclassOf( typeof( Item ) ) )
			{
				Item i = Activator.CreateInstance( m_Entry.Type ) as Item;
				m_ItemID = i.ItemID;
				i.Delete();
			}

			for ( int i = 0; i < m_Entry.Hues.Length; i++ )
			{
				if ( shouldadd )
				{
					AddPage( page );
					StartPage();
					shouldadd = false;
				}

				int hue = entry.Hues[i];

				AddButton( 35, y, 0x837, 0x838, i + 1, GumpButtonType.Reply, 0 );

				AddItem( 55, y - 5, m_ItemID, hue );
				AddTooltip( entry.Cliloc );

				if ( i == m_Entry.Hues.Length - 1 )
				{
					EndPage( page, true );
				}
				else if ( y >= 290 )
				{
					y = 130;
					shouldadd = true;
					EndPage( page, false );
					page++;
				}
				else
				{
					y += 40;
				}
			}
		}

		public void StartPage()
		{
			AddHtmlLocalized( 50, 65, 150, 20, 1072843, 0x1, false, false ); // Your Reward Points:
			AddLabel( 230, 65, 0x64, m_Points.ToString() );
			AddImageTiled( 35, 85, 270, 2, 0x23C5 );
			AddHtmlLocalized( 35, 90, 270, 20, 1074255, 0x1, false, false ); // Please select a hue for your Reward:
		}

		public void EndPage( int page, bool lastpage )
		{
			int pageto = 1;

			if ( !lastpage )
				pageto = page + 1;

			AddHtmlLocalized( 210, 335, 60, 20, 1074256, 0x1, false, false ); // More Hues
			AddButton( 270, 335, 0x15E1, 0x15E5, -2, GumpButtonType.Page, pageto );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			PlayerMobile pm = sender.Mobile as PlayerMobile;

			if ( pm == null )
				return;

			if ( info.ButtonID == -1 )
			{
				//pm.CloseGump( typeof( CollectionRewardGump ) );
				pm.SendGump( new CollectionRewardGump( m_Collection, m_From ) );
			}
			else if ( info.ButtonID > 0 )
			{
				int i = info.ButtonID - 1;

				if ( i < 0 || i >= m_Entry.Hues.Length || pm.Backpack == null )
					return;

				int hue = m_Entry.Hues[i];

				pm.SendGump( new CollectionConfirmReward( m_Collection, m_Entry, pm, hue ) );
			}
		}
	}
}