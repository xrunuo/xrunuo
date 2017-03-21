using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
	public class ConfirmInsureGump : Gump
	{
		public override int TypeID { get { return 0x2337; } }

		private PlayerMobile m_Owner;
		private ItemInsuranceInfo[] m_InsuranceInfo;

		public ConfirmInsureGump( PlayerMobile pm, ItemInsuranceInfo[] info )
			: base( 250, 200 )
		{
			m_Owner = pm;
			m_InsuranceInfo = info;

			AddPage( 0 );

			AddBackground( 0, 0, 240, 142, 5054 );
			AddImageTiled( 6, 6, 228, 100, 2624 );
			AddImageTiled( 6, 116, 228, 20, 2624 );
			AddAlphaRegion( 6, 6, 228, 142 );

			AddHtmlLocalized( 8, 8, 228, 100, 1114300, 0x7FFF, false, false ); // Do you wish to insure all newly selected items?

			AddButton( 114, 116, 4005, 4007, 100, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 148, 118, 450, 20, 1073996, 0x7FFF, false, false ); // ACCEPT

			AddButton( 6, 116, 4017, 4018, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40, 118, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 100 )
			{
				ApplyInsuranceChanges( m_Owner, m_InsuranceInfo );
			}
			else
			{
				m_Owner.SendLocalizedMessage( 1042021 ); // Cancelled.
				m_Owner.SendGump( new ItemInsuranceMenu( m_Owner, m_InsuranceInfo ) );
			}
		}

		public static void ApplyInsuranceChanges( Mobile from, ItemInsuranceInfo[] arInfo )
		{
			if ( !from.CheckAlive() )
				return;

			foreach ( ItemInsuranceInfo info in arInfo )
			{
				if ( info.Toggled )
				{
					Item item = info.Item;

					if ( !item.IsChildOf( from ) )
					{
						from.SendLocalizedMessage( 1060871, String.Empty, 0x23 ); // You can only insure items that you have equipped or that are in your backpack
					}
					else if ( item.Insured )
					{
						item.Insured = false;
						from.SendLocalizedMessage( 1060874, String.Empty, 0x35 ); // You cancel the insurance on the item
					}
					else
					{
						if ( !item.PayedInsurance )
						{
							int cost = info.GetCost();

							if ( Banker.Withdraw( from, cost ) )
							{
								from.SendLocalizedMessage( 1060398, cost.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
								item.PayedInsurance = true;
							}
							else
							{
								from.SendLocalizedMessage( 1061079, String.Empty, 0x23 ); // You lack the funds to purchase the insurance
								continue;
							}
						}

						item.Insured = true;
						from.SendLocalizedMessage( 1060873, String.Empty, 0x23 ); // You have insured the item
					}
				}
			}
		}
	}
}