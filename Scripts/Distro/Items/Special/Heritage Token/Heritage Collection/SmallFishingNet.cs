using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x1EA3, 0x1EA4 )]
	public class SmallFishingNetComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1076286; } } // Small Fish Net

		public SmallFishingNetComponent( int itemID )
			: base( itemID )
		{
		}

		public SmallFishingNetComponent( Serial serial )
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

	public class SmallFishingNetAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SmallFishingNetDeed(); } }

		[Constructable]
		public SmallFishingNetAddon()
			: this( true )
		{
		}

		[Constructable]
		public SmallFishingNetAddon( bool east )
			: base()
		{
			if ( east )
			{
				AddComponent( new SmallFishingNetComponent( 0x1EA4 ), 0, 0, 0 );
			}
			else
			{
				AddComponent( new SmallFishingNetComponent( 0x1EA3 ), 0, 0, 0 );
			}
		}


		public SmallFishingNetAddon( Serial serial )
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

	public class SmallFishingNetDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SmallFishingNetAddon( m_East ); } }
		public override int LabelNumber { get { return 1076286; } } // Small Fish Net

		private bool m_East;

		[Constructable]
		public SmallFishingNetDeed()
			: base()
		{
			LootType = LootType.Blessed;
		}

		public SmallFishingNetDeed( Serial serial )
			: base( serial )
		{
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

		private class InternalGump : Gump
		{
			private SmallFishingNetDeed m_Deed;

			public InternalGump( SmallFishingNetDeed deed )
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
				AddHtml( 14, 12, 273, 20, @"<CENTER><basefont color=#FFFFFF>Select your fishing net position</basefont></CENTER>", false, false ); // Please select your fish net position

				AddPage( 1 );

				AddButton( 19, 49, 0x845, 0x846, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 47, 213, 20, 1075386, 0x7FFF, false, false ); // South
				AddButton( 19, 73, 0x845, 0x846, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 44, 71, 213, 20, 1075387, 0x7FFF, false, false ); // East
			}

			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				if ( m_Deed == null || m_Deed.Deleted || info.ButtonID == 0 )
					return;

				m_Deed.m_East = ( info.ButtonID != 1 );
				m_Deed.SendTarget( sender.Mobile );
			}
		}
	}
}