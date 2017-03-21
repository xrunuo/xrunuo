using System;
using Server;
using Server.Gumps;
using Server.Network;
namespace Server.Items
{
	[Flipable( 0xC1B, 0xC1C, 0xC1E, 0xC1D )]
	public class StandingBrokenChairComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1076259; } } // Standing Broken Chair

		public StandingBrokenChairComponent()
			: base( 0xC1B )
		{
		}

		public StandingBrokenChairComponent( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class StandingBrokenChairAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new StandingBrokenChairDeed(); } }

		[Constructable]
		public StandingBrokenChairAddon( string position )
			: base()
		{
			if ( position == "south" )
				AddComponent( new LocalizedAddonComponent( 0xC1B, 1076259 ), 0, 0, 0 );
			else if ( position == "east" )
				AddComponent( new LocalizedAddonComponent( 0xC1C, 1076259 ), 0, 0, 0 );
			else if ( position == "north" )
				AddComponent( new LocalizedAddonComponent( 0xC1E, 1076259 ), 0, 0, 0 );
			else
				AddComponent( new LocalizedAddonComponent( 0xC1D, 1076259 ), 0, 0, 0 );

			//AddComponent( new StandingBrokenChairComponent(), 0, 0, 0 );
		}

		public StandingBrokenChairAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class StandingBrokenChairDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new StandingBrokenChairAddon( m_Position ); } }
		public override int LabelNumber { get { return 1076259; } } // Standing Broken Chair

		private string m_Position;

		[Constructable]
		public StandingBrokenChairDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public StandingBrokenChairDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.CloseGump( typeof( InternalGump ) );
				from.SendGump( new InternalGump( this ) );
			}
			else
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
		}

		private void SendTarget( Mobile m )
		{
			base.OnDoubleClick( m );
		}

		private class InternalGump : Gump
		{
			private StandingBrokenChairDeed m_Deed;

			public InternalGump( StandingBrokenChairDeed deed )
				: base( 60, 36 )
			{
				m_Deed = deed;

				AddPage( 0 );

				AddBackground( 0, 0, 273, 324, 0x13BE );
				AddImageTiled( 10, 10, 253, 20, 0xA40 );
				AddImageTiled( 10, 40, 253, 244, 0xA40 );
				AddImageTiled( 10, 294, 253, 20, 0xA40 );
				AddAlphaRegion( 10, 10, 253, 304 );

				AddButton( 10, 294, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 296, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your broken chair position</basefont></CENTER>", false, false ); // Please select your broken bed position

				AddPage( 1 );

				AddButton( 19, 49, 0x845, 0x846, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 47, 213, 20, 1075386, 0x7FFF, false, false ); // South
				AddButton( 19, 73, 0x845, 0x846, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 71, 213, 20, 1075387, 0x7FFF, false, false ); // East
				AddButton( 19, 97, 0x845, 0x846, 3, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 95, 213, 20, 1075389, 0x7FFF, false, false ); // North
				AddButton( 19, 121, 0x845, 0x846, 4, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 119, 213, 20, 1075390, 0x7FFF, false, false ); // West
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Deed == null || m_Deed.Deleted || info.ButtonID == 0 )
					return;

				if ( info.ButtonID == 1 )
					m_Deed.m_Position = "south";
				else if ( info.ButtonID == 2 )
					m_Deed.m_Position = "east";
				else if ( info.ButtonID == 3 )
					m_Deed.m_Position = "north";
				else if ( info.ButtonID == 4 )
					m_Deed.m_Position = "west";

				m_Deed.SendTarget( sender.Mobile );
			}
		}

	}
}