using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Prompts;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Engines.Collections
{
	public class CollectionDonateGump : Gump
	{
		public override int TypeID { get { return 0x4268; } }

		private Mobile m_From;
		private CollectionController m_Collection;

		private int m_Max;

		public void GetMax( DonationEntry[] list )
		{
			m_Max = 0;

			if ( list != null )
			{
				for ( int i = 0; i < list.Length; i++ )
					if ( m_Max < list[i].Width )
						m_Max = list[i].Width;
			}
		}

		public CollectionDonateGump( CollectionController collection, Mobile from )
			: base( 250, 50 )
		{
			if ( from.Backpack == null )
				return;

			m_Collection = collection;
			m_From = from;

			// Chequeamos la tabla de la colecci�n
			if ( !m_Collection.Table.Contains( from ) )
				m_Collection.Table.Add( from, ( Misc.TestCenter.Enabled ? 5000000 : 0 ) );

			GetMax( m_Collection.Donations );

			BuildHeader();

			AddPage( 1 );
			StartPage();

			int page = 1;

			int offset = 150;
			int next = 0;

			for ( int i = 0; i < m_Collection.Donations.Length; i++ )
			{
				DonationEntry entry = m_Collection.Donations[i] as DonationEntry;

				int height = Math.Max( entry.Height, 20 );

				if ( offset + next >= 310 )
				{
					EndPage( page, false );

					page++;
					AddPage( page );
					StartPage();

					offset = 150;
					next = 0;
				}

				// ******* Build Entry *******
				int amount = 0;

				if ( entry.Type == typeof( TreasureMap ) )
				{
					List<TreasureMap> maps = from.Backpack.FindItemsByType<TreasureMap>();

					for ( int j = 0; j < maps.Count; j++ )
						if ( CollectionController.CheckTreasureMap( maps[j], entry.Award ) )
							amount++;
				}
				else
				{
					amount = GetTotalAmountByType( from, entry.Type );
				}

				if ( amount > 0 )
				{
					int yOffset = offset + (int) ( height / 2 ) - 5;

					AddButton( 35, yOffset, 0x837, 0x838, i + 300, GumpButtonType.Reply, 0 );
					AddHtml( 245, yOffset - 4, 60, 20, String.Format( "<BODY><BASEFONT COLOR=\"#448844\"><DIV ALIGN=\"RIGHT\">{0}</DIV></BASEFONT></BODY>", amount ), false, false );
				}

				int y = offset - entry.Y;

				if ( entry.Height < 20 )
					y += ( 20 - entry.Height ) / 2;

				string award;
				if ( entry.Type == typeof( BankCheck ) )
					award = "1";
				else if ( entry.Award > 0 && entry.Award < 1 )
					award = String.Format( "1 per {0}", (int) ( 1 / entry.Award ) );
				else
					award = entry.Award.ToString();

				if ( entry.Name != null )
				{
					AddItem( 55 - entry.X + m_Max / 2 - entry.Width / 2, y, entry.Tile, entry.Hue );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 18, 0x63, entry.Name.ToString() );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 2, 0x64, award );
				}
				else
				{
					AddItem( 55 - entry.X + m_Max / 2 - entry.Width / 2, y, entry.Tile, entry.Hue );
					AddTooltip( entry.Cliloc );
					AddLabel( 65 + m_Max, offset + (int) ( height / 2 ) - 10, 0x64, award );
					AddTooltip( entry.Cliloc );
				}
				// ***** End Build Entry *****

				offset += 10 + height;

				if ( i < m_Collection.Donations.Length )
					next = Math.Max( m_Collection.Donations[i].Height, 20 );
				else
					next = 0;
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
			AddHtmlLocalized( 50, 65, 150, 20, 1072836, 0x1, false, false ); // Current Tier:
			AddLabel( 230, 65, 0x64, m_Collection.Tier.ToString() );

			AddHtmlLocalized( 50, 85, 150, 20, 1072837, 0x1, false, false ); // Current Points:
			AddLabel( 230, 85, 0x64, m_Collection.Points.ToString() );

			AddHtmlLocalized( 50, 105, 150, 20, 1072838, 0x1, false, false ); // Points Until Next Tier:
			AddLabel( 230, 105, 0x64, m_Collection.PointsUntilNextTier.ToString() );

			AddImageTiled( 35, 125, 270, 2, 0x23C5 );

			AddHtmlLocalized( 35, 130, 270, 20, 1072840, 0x1, false, false ); // Donations Accepted:
			AddHtmlLocalized( 245, 130, 60, 20, 1115571, 0x1, false, false ); // <DIV ALIGN="RIGHT">You have:</DIV>
		}

		public void EndPage( int page, bool lastpage )
		{
			AddButton( 50, 335, 0x15E3, 0x15E7, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 75, 335, 100, 20, 1072842, 0x1, false, false ); // Rewards

			if ( page > 1 )
			{
				AddButton( 150, 335, 0x15E3, 0x15E7, -2, GumpButtonType.Page, page - 1 );
				AddHtmlLocalized( 170, 335, 60, 20, 1074880, 0x1, false, false ); // Previous
			}

			if ( !lastpage )
			{
				AddHtmlLocalized( 240, 335, 60, 20, 1072854, 0x1, false, false ); // <div align=right>Next</div>
				AddButton( 300, 335, 0x15E1, 0x15E5, -2, GumpButtonType.Page, page + 1 );
			}
		}

		private static int GetTotalAmountByType( Mobile m, Type type )
		{
			int sum = 0;

			Container pack = m.Backpack;

			if ( pack != null )
			{
				List<Item> packItems = pack.Items;

				for ( int i = 0; i < packItems.Count; i++ )
				{
					Item item = packItems[i];

					if ( item is CommodityDeed )
						item = ( (CommodityDeed) item ).Commodity;

					if ( item != null && item.GetType() == type )
					{
						if ( item is BankCheck )
							sum += ( (BankCheck) item ).Worth;
						else
							sum += item.Amount;
					}
				}
			}

			return sum;
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
				pm.SendGump( new CollectionRewardGump( m_Collection, m_From ) );
			}
			else if ( info.ButtonID >= 300 )
			{
				int i = info.ButtonID - 300;

				if ( i < 0 || i >= m_Collection.Donations.Length || pm.Backpack == null )
					return;

				if ( m_Collection is BritainLibraryCollection && !pm.FriendOfTheLibrary )
				{
					pm.SendLocalizedMessage( 1074273 ); // You must speak with Librarian Verity before you can donate to this collection. 
					pm.SendGump( new CollectionDonateGump( m_Collection, m_From ) );

					return;
				}

				DonationEntry entry = m_Collection.Donations[i];

				if ( entry.Type == typeof( BankCheck ) )
				{
					List<BankCheck> checks = pm.Backpack.FindItemsByType<BankCheck>();

					if ( checks.Count == 0 )
					{
						// You do not have enough of that item to make a donation!
						pm.SendLocalizedMessage( 1073167 );
					}
					else
					{
						int balance = 0;

						for ( int j = 0; j < checks.Count; j++ )
						{
							BankCheck check = checks[j];

							balance += check.Worth;
							check.Delete();
						}

						int award = (int) ( balance * entry.Award );

						m_Collection.Award( pm, award );
					}

					pm.SendGump( new CollectionDonateGump( m_Collection, m_From ) );
				}
				else
				{
					Item item = FindItemByType( pm.Backpack, entry.Type );

					if ( item == null )
					{
						// You do not have enough of that item to make a donation!
						pm.SendLocalizedMessage( 1073167 );

						pm.SendGump( new CollectionDonateGump( m_Collection, m_From ) );
					}
					else if ( item.Stackable )
					{
						pm.Prompt = new AmountPrompt( m_Collection, i );
					}
					else
					{
						pm.SendLocalizedMessage( 1073389 ); // Please target the item you wish to donate.
						pm.Target = new DonationTarget( m_Collection, i );
					}
				}
			}
		}

		private Item FindItemByType( Container cont, Type type )
		{
			List<Item> list = cont.Items;

			for ( int i = 0; i < list.Count; ++i )
			{
				Item item = list[i];

				if ( item is CommodityDeed )
					item = ( (CommodityDeed) item ).Commodity;

				if ( item != null && type == item.GetType() )
					return item;
			}

			return null;
		}

		public class DonationTarget : Target
		{
			private CollectionController m_Collection;
			private int m_EntryIndex;

			public DonationTarget( CollectionController collection, int entryindex )
				: base( -1, false, TargetFlags.None )
			{
				m_Collection = collection;
				m_EntryIndex = entryindex;
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendLocalizedMessage( 1073184 ); // You cancel your donation.
				from.SendGump( new CollectionDonateGump( m_Collection, from ) );
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				DonationEntry entry = m_Collection.Donations[m_EntryIndex];

				if ( !( targeted is Item ) || !( (Item) targeted ).IsChildOf( from.Backpack ) || targeted.GetType() != entry.Type )
				{
					from.SendLocalizedMessage( 1073390 ); // The item you selected to donate is not of the type you picked.
				}
				else if ( targeted is TreasureMap && !CollectionController.CheckTreasureMap( (TreasureMap) targeted, entry.Award ) )
				{
					from.SendLocalizedMessage( 1073390 ); // The item you selected to donate is not of the type you picked.
				}
				else
				{
					( (Item) targeted ).Delete();

					m_Collection.Award( from, (int) entry.Award );
				}

				from.SendGump( new CollectionDonateGump( m_Collection, from ) );
			}
		}

		private class AmountPrompt : Prompt
		{
			// Please enter how much of that item you wish to donate:
			public override int MessageCliloc { get { return 1073178; } }

			private CollectionController m_Collection;
			private int m_EntryIndex;

			public AmountPrompt( CollectionController collection, int entryindex )
				: base( collection )
			{
				m_Collection = collection;
				m_EntryIndex = entryindex;
			}

			public override void OnCancel( Mobile from )
			{
				from.SendLocalizedMessage( 1073184 ); // You cancel your donation.
				from.SendGump( new CollectionDonateGump( m_Collection, from ) );
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( from.Backpack == null )
					return;

				int amount = Utility.ToInt32( text );

				DonationEntry entry = m_Collection.Donations[m_EntryIndex];

				Donate( from, entry.Type, entry.Award, amount );

				from.SendGump( new CollectionDonateGump( m_Collection, from ) );
			}

			/// <summary>
			/// Core procedure of the donation system for stackable items.
			/// </summary>
			/// <param name="m">The player whose backpack the items will be taken</param>
			/// <param name="type">The type of the donation item.</param>
			/// <param name="ratio">Number of points per unit of the item.</param>
			/// <param name="amount">How much is he going to donate.</param>
			private void Donate( Mobile m, Type type, double ratio, int amount )
			{
				if ( amount <= 0 )
				{
					// That is not a valid donation quantity.
					m.SendLocalizedMessage( 1073181 );

					return;
				}

				int sum = GetTotalAmountByType( m, type );
				int points = (int) ( amount * ratio );

				if ( sum < amount )
				{
					// You do not have enough to make a donation of that magnitude!
					m.SendLocalizedMessage( 1073182 );
				}
				else if ( points <= 0 )
				{
					// Your donation is too small to award any points.  Try giving a larger donation.
					m.SendLocalizedMessage( 1073166 );
				}
				else
				{
					List<Item> packItems = new List<Item>( m.Backpack.Items );

					for ( int i = 0; amount > 0 && i < packItems.Count; i++ )
					{
						Item item = packItems[i];

						CommodityDeed deed = null;

						if ( item is CommodityDeed )
						{
							deed = item as CommodityDeed;
							item = deed.Commodity;
						}

						if ( item != null && item.GetType() == type )
						{
							if ( amount >= item.Amount )
							{
								amount -= item.Amount;
								item.Delete();

								if ( deed != null )
									deed.Delete();
							}
							else
							{
								item.Amount -= amount;
								amount = 0;

								if ( deed != null )
									deed.InvalidateProperties();
							}
						}
					}

					m_Collection.Award( m, points );
				}
			}
		}
	}
}