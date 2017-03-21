using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Promotion
{
	public abstract class AnniversaryRewardGump : Gump
	{
		private PromotionalToken m_Token;

		public AnniversaryRewardGump( PromotionalToken token )
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

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 1 )
			{
				Mobile m = sender.Mobile;
				Item item = null;

				switch ( m_Token.Type )
				{
					case PromotionalType.PersonalAttendant:
						//item = new PersonalAttendantDeed();
						break;
					case PromotionalType.CrystalItems:
						{
							item = new WoodenBox();

							Container c = item as Container;
							c.Hue = 1173;
							c.Name = "A Box of Crystal Items";

							c.DropItem( new CrystalAltarDeed() );
							c.DropItem( new CrystalBeggarStatueDeed() );
							c.DropItem( new CrystalBrazierDeed() );
							c.DropItem( new CrystalBullStatueDeed() );
							c.DropItem( new CrystalRunnerStatueDeed() );
							c.DropItem( new CrystalSuplicantStatueDeed() );
							c.DropItem( new CrystalTableDeed() );
							c.DropItem( new CrystalThroneDeed() );

							break;
						}
					case PromotionalType.ShadowItems:
						{
							item = new WoodenBox();

							Container c = item as Container;
							c.Hue = 1908;
							c.Name = "A Box of Shadow Items";

							c.DropItem( new FireDemonStatueDeed() );
							c.DropItem( new GlobeOfSosariaDeed() );
							c.DropItem( new ObsidianPillarDeed() );
							c.DropItem( new ObsidianRockDeed() );
							c.DropItem( new ShadowAltarDeed() );
							c.DropItem( new ShadowBannerDeed() );
							c.DropItem( new ShadowFirePitDeed() );
							c.DropItem( new ShadowPillarDeed() );
							c.DropItem( new SpikeColumnDeed() );
							c.DropItem( new SpikePostDeed() );

							break;
						}
				}

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

	public class CrystalSetConfirmGump : AnniversaryRewardGump
	{
		public override int TypeID { get { return 0x235F; } }

		public CrystalSetConfirmGump( PromotionalToken token )
			: base( token )
		{
		}
	}

	public class ShadowSetConfirmGump : AnniversaryRewardGump
	{
		public override int TypeID { get { return 0x2360; } }

		public ShadowSetConfirmGump( PromotionalToken token )
			: base( token )
		{
		}
	}
}