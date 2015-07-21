using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
	public class ConfirmationCancelInsuranceGump : Gump
	{
		public override int TypeID { get { return 0x2337; } }

		private PlayerMobile m_Owner;
		private ItemInsuranceInfo[] m_InsuranceInfo;

		public ConfirmationCancelInsuranceGump( PlayerMobile pm )
			: this( pm, null )
		{
		}

		public ConfirmationCancelInsuranceGump( PlayerMobile pm, ItemInsuranceInfo[] info )
			: base( 250, 200 )
		{
			m_Owner = pm;
			m_InsuranceInfo = info;

			AddPage( 0 );

			AddBackground( 0, 0, 240, 142, 5054 );
			AddImageTiled( 6, 6, 228, 100, 2624 );
			AddImageTiled( 6, 116, 228, 20, 2624 );
			AddAlphaRegion( 6, 6, 228, 142 );

			AddHtmlLocalized( 8, 8, 228, 100, 1071021, 0x7FFF, false, false ); // You are about to disable inventory insurance auto-renewal.

			AddButton( 114, 116, 4005, 4007, 100, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 148, 118, 450, 20, 1071022, 0x7FFF, false, false ); // DISABLE IT!

			AddButton( 6, 116, 4017, 4018, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40, 118, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( info.ButtonID == 100 )
			{
				m_Owner.SendLocalizedMessage( 1061075, String.Empty, 0x23 ); // You have cancelled automatically reinsuring all insured items upon death
				m_Owner.AutoRenewInsurance = false;
			}
			else
			{
				m_Owner.SendLocalizedMessage( 1042021 ); // Cancelled.
			}

			if ( m_InsuranceInfo != null )
				m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_InsuranceInfo ) );
		}
	}
}