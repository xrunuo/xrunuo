using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Collections
{
	public class CollectionRewardGump : Gump
	{
		public override int TypeID { get { return 0x4268; } }

		private Mobile m_From;
		private CollectionController m_Collection;
		private int m_Points;

		private int m_Max;

		public void GetMax( IRewardEntry[] list )
		{
			m_Max = 0;

			if ( list != null )
			{
				for ( int i = 0; i < list.Length; i++ )
					if ( m_Max < list[i].Width )
						m_Max = list[i].Width;
			}
		}

		public CollectionRewardGump( CollectionController collection, Mobile from )
			: base( 250, 50 )
		{
			m_Collection = collection;
			m_From = from;

			m_Points = (int) m_Collection.Table[from];

			GetMax( m_Collection.Rewards );

			PlayerMobile pm = (PlayerMobile) from;

			BuildHeader();

			int page = 1;

			int offset = 110;
			int next = 0;

			AddPage( page );
			StartPage();

			for ( int i = 0; i < m_Collection.Rewards.Length; i++ )
			{
				RewardEntry entry = m_Collection.Rewards[i] as RewardEntry;

				if ( !entry.IsEligibleBy( from ) )
					continue;

				int min = entry.Name != null ? 30 : 20;

				if ( i < m_Collection.Rewards.Length )
					next = Math.Max( m_Collection.Rewards[i].Height, min );
				else
					next = 0;

				int height = Math.Max( entry.Height, 20 );

				if ( offset + next >= 310 )
				{
					EndPage( page, false );

					page++;
					AddPage( page );
					StartPage();

					offset = 110;
					next = 0;
				}

				// ******* Build Entry *******
				int price = entry.GetPrice( from );
				bool enabled = m_Points >= price;

				if ( enabled )
				{
					AddButton( 35, offset + (int) ( height / 2 ) - 5, 0x837, 0x838, i + 100, GumpButtonType.Reply, 0 );

					if ( entry.Cliloc != -1 )
						AddTooltip( entry.Cliloc );
				}

				int y = offset - entry.Y;

				if ( entry.Height < min )
					y += ( min - entry.Height ) / 2;

				if ( entry.Name != null )
				{
					AddItem( 55 - entry.X + m_Max / 2 - entry.Width / 2, y, entry.Tile, enabled ? entry.Hue : 995 );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 18, ( enabled ? 0x64 : 0x21 ) - 1, entry.Name.ToString() );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 2, ( enabled ? 0x64 : 0x21 ), price.ToString() );
				}
				else
				{
					AddItem( 55 - entry.X + m_Max / 2 - entry.Width / 2, y, entry.Tile, enabled ? entry.Hue : 995 );
					AddTooltip( entry.Cliloc );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 10, enabled ? 0x64 : 0x21, price.ToString() );
					AddTooltip( entry.Cliloc );
				}

				// ***** End Build Entry *****

				offset += 5 + height;

				if ( entry.Name != null )
					offset += 5;
			}

			EndPage( page, true );
		}

		public void BuildHeader()
		{
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
		}

		public void StartPage()
		{
			AddHtmlLocalized( 50, 65, 150, 20, 1072843, 0x1, false, false ); // Your Reward Points:
			AddLabel( 230, 65, 0x64, m_Points.ToString() );

			AddImageTiled( 35, 85, 270, 2, 0x23C5 );

			AddHtmlLocalized( 35, 90, 270, 20, 1072844, 0x1, false, false ); // Please Choose a Reward:

			AddButton( 50, 335, 0x15E3, 0x15E7, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 75, 335, 100, 20, 1072845, 0x1, false, false ); // Status
		}

		public void EndPage( int page, bool lastpage )
		{
			if ( page > 1 )
			{
				AddButton( 150, 335, 0x15E3, 0x15E7, -2, GumpButtonType.Page, page - 1 );
				AddHtmlLocalized( 170, 335, 60, 20, 1074880, 0x1, false, false ); // Previous
			}
			else
			{
				AddKRButton( 0, 0, 0x0, 0x0, -2, GumpButtonType.Page, 0 );
			}

			if ( !lastpage )
			{
				AddHtmlLocalized( 240, 335, 60, 20, 1072854, 0x1, false, false ); // <div align=right>Next</div>
				AddButton( 300, 335, 0x15E1, 0x15E5, -2, GumpButtonType.Page, page + 1 );
			}
			else
			{
				AddKRButton( 0, 0, 0x0, 0x0, -2, GumpButtonType.Page, 0 );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile pm = sender.Mobile as PlayerMobile;

			if ( pm == null )
				return;

			if ( m_Collection is BritainLibraryCollection )
			{
				BritainLibraryCollection col = m_Collection as BritainLibraryCollection;

				if ( col.Representative != null && pm.GetDistanceToSqrt( col.Representative.Location ) > 10 )
					return;
			}
			else if ( pm.GetDistanceToSqrt( m_Collection.Location ) > 10 )
			{
				return;
			}

			if ( info.ButtonID == 1 )
			{
				//pm.CloseGump( typeof( CollectionRewardGump ) );
				pm.SendGump( new CollectionDonateGump( m_Collection, m_From ) );
			}
			else if ( info.ButtonID >= 100 )
			{
				int i = info.ButtonID - 100;

				if ( i < 0 || i >= m_Collection.Rewards.Length || pm.Backpack == null )
					return;

				IRewardEntry entry = m_Collection.Rewards[i];

				if ( entry.Hues == null )
					pm.SendGump( new CollectionConfirmReward( m_Collection, entry, pm, 0 ) );
				else if ( entry.Hues.Length == 1 )
					pm.SendGump( new CollectionConfirmReward( m_Collection, entry, pm, entry.Hues[0] ) );
				else
					pm.SendGump( new CollectionSelectHueGump( m_Collection, entry, pm ) );
			}
		}
	}
}