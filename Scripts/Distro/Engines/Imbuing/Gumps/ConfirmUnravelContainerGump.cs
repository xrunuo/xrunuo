using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Imbuing
{
	public class ConfirmUnravelContainerGump : Gump
	{
		private Container m_Container;

		public ConfirmUnravelContainerGump( Container container )
			: base( 25, 50 )
		{
			m_Container = container;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 250, 0x13BE );
			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 170, 0xA40 );
			AddImageTiled( 10, 220, 500, 20, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 230 );

			/* WARNING! The selected container contains items made with a special material.
			 * These items will be DESTROYED.
			 * Do you wish to unravel these items as well? */
			AddHtmlLocalized( 15, 58, 490, 113, 1112404, true, true );

			// <CENTER>UNRAVEL MAGIC ITEM CONFIRMATION</CENTER>
			AddHtmlLocalized( 10, 12, 500, 20, 1112402, 0x7FFF, false, false );

			// YES
			AddHtmlLocalized( 45, 190, 120, 20, 1049717, 0x7FFF, false, false );
			AddButton( 10, 188, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );

			// NO
			AddHtmlLocalized( 45, 222, 450, 20, 1049718, 0x7FFF, false, false );
			AddButton( 10, 220, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			from.EndAction( typeof( Imbuing ) );

			if ( info.ButtonID == 1 )
			{
				if ( !Soulforge.CheckProximity( from, 2 ) )
				{
					// You must be near a soulforge to magically unravel an item.
					from.SendLocalizedMessage( 1080433 );
				}
				else if ( !Soulforge.CheckQueen( from ) )
				{
					// You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
					from.SendLocalizedMessage( 1113736 );
				}
				else if ( !m_Container.IsChildOf( from.Backpack ) )
				{
					// This item must be in your backpack to be used.
					from.SendLocalizedMessage( 1062334 );
				}
				else
				{
					Unraveling.UnravelContainer( from, m_Container );
				}
			}
		}
	}
}