using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Engines.VendorSearch;

namespace Server.Gumps
{
	public class VendorSearchResultsGump : Gump
	{
		public static readonly int ResultsPerPage = 5;

		private IVendorSearchItem[] m_Items;
		private SearchCriteria m_Criteria;
		private int m_Page;

		public VendorSearchResultsGump( IVendorSearchItem[] items, SearchCriteria criteria, int page )
			: base( 30, 30 )
		{
			m_Items = items;
			m_Criteria = criteria;
			m_Page = page;

			int pageIndex = 0;

			AddPage( pageIndex++ );

			AddBackground( 0, 0, 500, 550, 0x7748 );
			AddHtmlLocalized( 50, 50, 400, 18, 1154509, 0x4BBD, false, false ); // <center>Vendor Search Results</center>

			AddHtmlLocalized( 162, 70, 102, 18, 1114513, "#1062218", 0x4BBD, false, false ); // <DIV ALIGN=CENTER>Price</DIV>
			AddHtmlLocalized( 274, 70, 61, 18, 1114513, "#1154644", 0x4BBD, false, false ); // <DIV ALIGN=CENTER>Facet</DIV>
			AddHtmlLocalized( 345, 70, 102, 18, 1114513, "#1154642", 0x4BBD, false, false ); // <DIV ALIGN=CENTER>Create Map</DIV>

			int yOffset = 0;

			for ( int i = 0; i < m_Items.Length; i++ )
			{
				IVendorSearchItem item = m_Items[i];

				if ( ( i % ResultsPerPage ) == 0 )
				{
					AddPage( pageIndex );

					if ( pageIndex > 1 )
					{
						AddButton( 50, 480, 0x7745, 0x7745, 0, GumpButtonType.Page, pageIndex - 1 );
						AddHtmlLocalized( 90, 480, 255, 20, 1044044, 0x4BBD, false, false ); // PREV PAGE
					}

					bool thereAreMorePages = ( i + ResultsPerPage < m_Items.Length );

					if ( thereAreMorePages )
					{
						AddButton( 430, 480, 0x7745, 0x7745, 0, GumpButtonType.Page, pageIndex + 1 );
						AddHtmlLocalized( 355, 480, 70, 20, 1114514, "#1044045", 0x4BBD, false, false ); // <DIV ALIGN=RIGHT>NEXT PAGE</DIV>
					}

					pageIndex++;
					yOffset = 0;
				}

				AddButtonTileArt( 50, 101 + yOffset, 0x918, 0x918, GumpButtonType.Page, 0, 0, item.Item.ItemID, item.Item.Hue, 18, 12 );
				AddItemPropertyTooltip( item.Item );

				AddHtmlLocalized( 162, 101 + yOffset, 102, 72, item.IsContained ? 1154598 : 1154645, item.Price.ToString( "N0" ), 0x6B55, false, false ); // <center>Container Price<BR>~1_cost~</center> / <center>~1_val~</center>
				AddHtmlLocalized( 274, 101 + yOffset, 102, 72, item.Item.Map.GetNameCliloc(), 0x6B55, false, false );
				AddButton( 386, 106 + yOffset, 0x7745, 0x7745, 100 + i, GumpButtonType.Reply, 0 );

				yOffset += 74;
			}

			if ( m_Items.Length == VendorItemFinder.MaxResults )
			{
				AddButton( 430, 480, 0x7745, 0x7745, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 225, 480, 200, 20, 1114514, "#1154687", 0x4BBD, false, false ); // <DIV ALIGN=RIGHT>~1_TOKEN~</DIV>
			}
		}

		private void AddItemPropertyTooltip( Item item )
		{
			item.GetProperties( new ObjectPropertyListTooltip( this ) );
		}

		private class ObjectPropertyListTooltip : ObjectPropertyList
		{
			private Gump m_Gump;

			public ObjectPropertyListTooltip( Gump gump )
			{
				m_Gump = gump;
			}

			public void Add( int number )
			{
				m_Gump.AddTooltip( number );
			}

			public void Add( int number, string arguments )
			{
				m_Gump.AddTooltip( number, arguments );
			}

			public void Add( int number, string format, params object[] args )
			{
				m_Gump.AddTooltip( number, string.Format( format, args ) );
			}

			public void Add( string text )
			{
				m_Gump.AddTooltip( 1042971, text );
			}

			public void Add( string format, params object[] args )
			{
				m_Gump.AddTooltip( 1042971, string.Format( format, args ) );
			}
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( info.ButtonID == 2 )
			{
				VendorSearchQueryGump.StartSearch( from, m_Criteria, m_Page + 1 );
				return;
			}

			int index = info.ButtonID - 100;

			if ( !from.BeginAction( typeof( ShopMap ) ) )
			{
				from.SendLocalizedMessage( 500119 ); // You must wait to perform another action
				return;
			}

			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), () => from.EndAction( typeof( ShopMap ) ) );

			if ( index >= 0 && index < m_Items.Length )
			{
				IVendorSearchItem item = m_Items[index];

				var mapItem = new ShopMap( item.Vendor, item.Container );

				if ( from.AddToBackpack( mapItem ) )
				{
					from.SendLocalizedMessage( 1154690 ); // The vendor map has been placed in your backpack.
				}
				else
				{
					from.SendLocalizedMessage( 502385 ); // Your pack cannot hold this item.
					mapItem.Delete();
				}

				from.SendGump( new VendorSearchResultsGump( m_Items, m_Criteria, m_Page ) );
			}
		}
	}
}