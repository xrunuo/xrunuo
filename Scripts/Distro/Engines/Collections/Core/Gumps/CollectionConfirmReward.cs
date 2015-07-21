using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Collections
{
	public class CollectionConfirmReward : Gump
	{
		public override int TypeID { get { return 0x4269; } }

		private CollectionController m_Collection;
		private IRewardEntry m_Entry;
		private int m_Hue;
		private Mobile m_From;

		public CollectionConfirmReward( CollectionController collection, IRewardEntry entry, Mobile from, int hue )
			: base( 120, 50 )
		{
			m_Collection = collection;
			m_Entry = entry;
			m_Hue = hue;
			m_From = from;

			AddPage( 0 );

			Closable = false;
			AddImageTiled( 0, 0, 348, 262, 0xA8E );
			AddAlphaRegion( 0, 0, 348, 262 );
			AddImage( 0, 15, 0x27A8 );
			AddImageTiled( 0, 30, 17, 200, 0x27A7 );
			AddImage( 0, 230, 0x27AA );
			AddImage( 15, 0, 0x280C );
			AddImageTiled( 30, 0, 300, 17, 0x280A );
			AddImage( 315, 0, 0x280E );
			AddImage( 15, 244, 0x280C );
			AddImageTiled( 30, 244, 300, 17, 0x280A );
			AddImage( 315, 244, 0x280E );
			AddImage( 330, 15, 0x27A8 );
			AddImageTiled( 330, 30, 17, 200, 0x27A7 );
			AddImage( 330, 230, 0x27AA );
			AddImage( 333, 2, 0x2716 );
			AddImage( 333, 248, 0x2716 );
			AddImage( 2, 248, 0x2716 );
			AddImage( 2, 2, 0x2716 );
			AddItem( 130, 120, entry.Tile, hue );

			if ( entry.Cliloc != -1 )
				AddTooltip( entry.Cliloc );

			AddHtmlLocalized( 25, 22, 200, 20, 1074974, 0x7D00, false, false ); // Confirm Selection
			AddImage( 25, 40, 0xBBF );
			AddHtmlLocalized( 25, 55, 300, 120, 1074975, 0xFFFFFF, false, false ); // Are you sure you wish to select this?
			AddRadio( 25, 175, 0x25F8, 0x25FB, true, 1 );
			AddRadio( 25, 210, 0x25F8, 0x25FB, false, 0 );
			AddHtmlLocalized( 60, 180, 280, 20, 1074976, 0xFFFFFF, false, false ); // Yes
			AddHtmlLocalized( 60, 215, 280, 20, 1074977, 0xFFFFFF, false, false ); // No
			AddButton( 265, 220, 0xF7, 0xF8, 7, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( info.ButtonID == 7 )
			{
				if ( info.IsSwitched( 1 ) )
				{
					Mobile from = sender.Mobile;

					int price = m_Entry.GetPrice( from );

					if ( from.Backpack == null || price > (int) m_Collection.Table[from] )
						return;

					if ( m_Entry.Type == typeof( RewardTitle ) )
					{
						PlayerMobile pm = from as PlayerMobile;

						if ( pm.CollectionTitles.Contains( m_Entry.RewardTitle ) )
						{
							pm.SendLocalizedMessage( 1073626 ); // You already have that title!
						}
						else
						{
							m_Collection.Table[from] = (int) m_Collection.Table[from] - price;

							pm.CollectionTitles.Add( m_Entry.RewardTitle );
							pm.SendLocalizedMessage( 1073625, String.Format( "#{0}", m_Entry.RewardTitle.ToString() ) ); // The title "~1_TITLE~" has been bestowed upon you.

							pm.CurrentCollectionTitle = pm.CollectionTitles.Count - 1;
							pm.PlaySound( 0x5A7 );
						}
					}
					else
					{
						m_Collection.Table[from] = (int) m_Collection.Table[from] - price;

						Item reward = Activator.CreateInstance( m_Entry.Type, m_Entry.Args ) as Item;

						if ( m_Hue != 0 )
							reward.Hue = m_Hue;

						m_Collection.OnRewardCreated( from, reward );

						from.Backpack.DropItem( reward );

						from.SendLocalizedMessage( 1073621 ); // Your reward has been placed in your backpack.
						from.PlaySound( 0x5A7 );
					}

					m_Entry.OnRewardChosen( from );
				}

				sender.Mobile.SendGump( new CollectionRewardGump( m_Collection, m_From ) );
			}
		}
	}
}
