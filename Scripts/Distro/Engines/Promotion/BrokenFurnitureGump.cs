using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Promotion
{
	public abstract class BrokenFurnitureGump : Gump
	{
		private PromotionalToken m_Token;

		public BrokenFurnitureGump( PromotionalToken token )
			: base( 200, 200 )
		{
			m_Token = token;

			AddKRHtmlLocalized( 0, 0, 0, 0, 1015313, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1049004, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1076597, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1011036, false, false );
			AddKRHtmlLocalized( 0, 0, 0, 0, 1011012, false, false );

			AddPage( 0 );

			AddBackground( 0, 0, 291, 159, 0x13BE );

			AddImageTiled( 5, 6, 280, 20, 0xA40 );
			AddHtmlLocalized( 9, 8, 280, 20, 1049004, 0x7FFF, false, false ); // Confirm

			AddImageTiled( 5, 31, 280, 100, 0xA40 );
			AddHtmlLocalized( 9, 35, 272, 100, 1076597, 0x7FFF, false, false ); // Clicking "OK" will create the items in your backpack if there is room.  Otherwise it will be created in your bankbox.

			AddButton( 190, 133, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 225, 135, 90, 20, 1006044, 0x7FFF, false, false ); // OK

			AddButton( 5, 133, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40, 135, 100, 20, 1060051, 0x7FFF, false, false ); // CANCEL
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
			{
				Mobile m = sender.Mobile;
				Item item = null;

				item = new WoodenBox();

				Container c = item as Container;
				c.Hue = 1173;
				c.Name = "A Broken Furniture Set";

				c.DropItem( new BrokenBedDeed() );
				c.DropItem( new BrokenChestOfDrawersDeed() );
				c.DropItem( new BrokenFallenChairDeed() );
				c.DropItem( new StandingBrokenChairDeed() );
				c.DropItem( new BrokenArmoireDeed() );
				c.DropItem( new BrokenBookcaseDeed() );
				c.DropItem( new BrokenCoveredChairDeed() );
				c.DropItem( new BrokenVanityDeed() );


				if ( item != null && m_Token != null )
				{
					if ( !m.AddToBackpack( item ) )
					{
						if ( m.BankBox.TryDropItem( m, item, false ) )
							item.MoveToWorld( m.Location, m.Map );
					}

					m_Token.Delete();
				}
			}
		}
	}

	public class BrokenFurnitureConfirmGump : BrokenFurnitureGump
	{
		public override int TypeID { get { return 0x235F; } }

		public BrokenFurnitureConfirmGump( PromotionalToken token )
			: base( token )
		{
		}
	}
}