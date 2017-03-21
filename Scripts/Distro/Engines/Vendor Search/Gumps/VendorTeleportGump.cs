using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Misc;

namespace Server.Gumps
{
	public class VendorTeleportGump : Gump
	{
		private ShopMap m_ShopMap;
		private bool m_Used;

		public VendorTeleportGump( ShopMap map )
			: base( 10, 10 )
		{
			m_ShopMap = map;
			m_Used = map.Used;

			AddPage( 0 );

			AddBackground( 0, 0, 414, 214, 0x7752 );

			if ( m_Used )
			{
				/**
				 * Please select 'Accept' if you would like to return to ~1_loc~ (~2_facet~).  This map will be deleted after use.
				 */
				AddHtmlLocalized( 27, 47, 380, 80, 1154637, string.Format( "{0}\t#{1}", ShopMap.GetLocationFormatted( map.PreviousLocation, map.PreviousMap ), map.PreviousMap.GetNameCliloc() ), 0x4E73, false, false );
			}
			else
			{
				/**
				 * Please select 'Accept' if you would like to pay ~1_cost~ gold to teleport to vendor ~2_name~.
				 * For this price you will also be able to teleport back to this location within the next ~3_minutes~ minutes.
				 */
				AddHtmlLocalized( 27, 47, 380, 80, 1154635, string.Format( "{0}\t{1}\t{2}", ShopMap.TeleportCost, map.VendorName, ShopMap.DeleteDelayMinutes ), 0x4E73, false, false );
			}

			AddButton( 377, 167, 0x7746, 0x7746, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 267, 167, 100, 40, 1114514, "#1150299", 0x4E73, false, false ); // <DIV ALIGN=RIGHT>ACCEPT</DIV>

			AddButton( 7, 167, 0x7747, 0x7747, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 47, 167, 100, 40, 1150300, 0x4E73, false, false ); // CANCEL
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( info.ButtonID == 1 )
			{
				if ( m_Used )
					m_ShopMap.TeleportToPreviousLocation( from );
				else
					m_ShopMap.TeleportToVendor( from );
			}
		}
	}
}
