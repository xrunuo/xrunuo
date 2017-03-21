using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.BulkOrders
{
	public class SmallBODGump : Gump
	{
		public override int TypeID { get { return 0x1C8; } }

		private SmallBOD m_Deed;
		private Mobile m_From;

		public SmallBODGump( Mobile from, SmallBOD deed )
			: base( 25, 25 )
		{
			m_From = from;
			m_Deed = deed;

			m_From.CloseGump( typeof( LargeBODGump ) );
			m_From.CloseGump( typeof( SmallBODGump ) );

			int factor = 0;

			if ( deed.RequireExceptional )
				factor++;

			if ( deed.Material != BulkMaterialType.None )
				factor++;

			if ( factor != 0 )
				factor++;

			factor *= 24;

			AddPage( 0 );

			AddBackground( 50, 10, 455, 167 + factor, 5054 );
			AddImageTiled( 58, 20, 438, 148 + factor, 2624 );
			AddAlphaRegion( 58, 20, 438, 148 + factor );

			AddImage( 45, 5, 10460 );
			AddImage( 480, 5, 10460 );
			AddImage( 45, 152 + factor, 10460 );
			AddImage( 480, 152 + factor, 10460 );

			AddHtmlLocalized( 75, 48, 250, 20, 1045138, 0xFFFFFF, false, false ); // Amount to make:
			AddLabel( 275, 48, 1152, deed.AmountMax.ToString() );

			AddHtmlLocalized( 275, 76, 200, 20, 1045153, 0xFFFFFF, false, false ); // Amount finished:

			AddHtmlLocalized( 160, 144 + factor, 120, 20, 1011441, 0xFFFFFF, false, false ); // EXIT
			AddButton( 125, 144 + factor, 4005, 4007, 1, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 225, 25, 120, 20, 1045133, 0xFFFFFF, false, false ); // A bulk order

			AddHtmlLocalized( 75, 72, 120, 20, 1045136, 0xFFFFFF, false, false ); // Item requested:
			AddItem( 380, 72, deed.Graphic );

			AddHtmlLocalized( 75, 96, 210, 20, deed.Number, 0xFFFFFF, false, false );
			AddLabel( 275, 96, 0x480, deed.AmountCur.ToString() );

			if ( deed.RequireExceptional || deed.Material != BulkMaterialType.None )
			{
				// Special requirements to meet:
				AddHtmlLocalized( 75, 120, 200, 20, 1045140, 0xFFFFFF, false, false );
			}
			else
				AddKRHtmlLocalized( 0, 0, 0, 0, -1, false, false );

			if ( deed.RequireExceptional )
			{
				// All items must be exceptional.
				AddHtmlLocalized( 75, 144, 300, 20, 1045141, 0xFFFFFF, false, false );
			}
			else
				AddKRHtmlLocalized( 0, 0, 0, 0, -1, false, false );

			if ( deed.Material != BulkMaterialType.None )
			{
				// All items must be made with x material.
				AddHtmlLocalized( 75, deed.RequireExceptional ? 168 : 144, 300, 20, GetMaterialNumberFor( deed.Material ), 0xFFFFFF, false, false );
			}
			else
				AddKRHtmlLocalized( 0, 0, 0, 0, -1, false, false );

			AddKRHtmlLocalized( 0, 0, 0, 0, -1, false, false );

			AddButton( 125, 120 + factor, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 160, 120 + factor, 300, 20, 1045154, 0xFFFFFF, false, false ); // Combine this deed with the item requested.
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Deed.Deleted || !m_Deed.IsChildOf( m_From.Backpack ) )
			{
				return;
			}

			if ( info.ButtonID == 2 ) // Combine
			{
				m_From.SendGump( new SmallBODGump( m_From, m_Deed ) );
				m_Deed.BeginCombine( m_From );
			}
		}

		public static int GetMaterialNumberFor( BulkMaterialType material )
		{
			if ( material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite )
			{
				return 1045142 + (int) ( material - BulkMaterialType.DullCopper );
			}
			else if ( material >= BulkMaterialType.Spined && material <= BulkMaterialType.Barbed )
			{
				return 1049348 + (int) ( material - BulkMaterialType.Spined );
			}

			return 0;
		}
	}
}