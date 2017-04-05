using System;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	public class HairRestylingDeed : Item
	{
		public override int LabelNumber { get { return 1041061; } } // a coupon for a free hair restyling

		[Constructable]
		public HairRestylingDeed()
			: base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public HairRestylingDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendGump( new InternalGump( from, this ) );
			}
		}

		private class InternalGump : Gump
		{
			public override int TypeID { get { return 0xF3E6F; } }

			private Mobile m_From;
			private HairRestylingDeed m_Deed;

			public InternalGump( Mobile from, HairRestylingDeed deed )
				: base( 50, 50 )
			{
				m_From = from;
				m_Deed = deed;

				from.CloseGump<InternalGump>();

				AddPage( 0 );

				AddBackground( 50, 10, 450, 300, 0xA28 );

				AddButton( 95, 250, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddButton( 320, 250, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0 );

				AddHtmlLocalized( 127, 250, 90, 35, 1006044, false, false ); // OK
				AddHtmlLocalized( 352, 250, 90, 35, 1006045, false, false ); // Cancel

				AddHtmlLocalized( 235, 250, 85, 35, 1011064, false, false ); // Bald (If you choose to go bald, you will lose your hair color.)

				AddHtmlLocalized( 100, 25, 350, 20, 1018353, false, false ); // <center>New Hairstyle</center>

				AddBackground( 137, 60, 50, 50, 0xA3C );
				AddBackground( 137, 125, 50, 50, 0xA3C );
				AddBackground( 137, 190, 50, 50, 0xA3C );
				AddBackground( 260, 60, 50, 50, 0xA3C );
				AddBackground( 260, 125, 50, 50, 0xA3C );
				AddBackground( 260, 190, 50, 50, 0xA3C );
				AddBackground( 383, 60, 50, 50, 0xA3C );
				AddBackground( 383, 125, 50, 50, 0xA3C );
				AddBackground( 383, 190, 50, 50, 0xA3C );

				if ( from.Race == Race.Elf )
				{
					AddImage( 50, 10, 0x6F9 );
					AddImage( 50, 75, 0x6FA );
					AddImage( 50, 140, 0x6FB );
					AddImage( 173, 75, 0x6FE );
					AddImage( 173, 130, 0x6FF );
					AddImage( 300, 80, 0x701 );

					AddRadio( 90, 80, 0xD0, 0xD1, false, 12224 );
					AddRadio( 90, 145, 0xD0, 0xD1, false, 12225 );
					AddRadio( 90, 210, 0xD0, 0xD1, false, 12226 );
					AddRadio( 215, 145, 0xD0, 0xD1, false, 12238 );
					AddRadio( 215, 210, 0xD0, 0xD1, false, 12239 );
					AddRadio( 340, 145, 0xD0, 0xD1, false, 12241 );

					if ( from.Female )
					{
						AddImage( 296, 10, 0x6F6 );
						AddRadio( 340, 80, 0xD0, 0xD1, false, 12240 );
						AddImage( 300, 135, 0x6FC );
						AddRadio( 340, 210, 0xD0, 0xD1, false, 12236 );
					}
					else
					{
						AddImage( 173, 10, 0x6FD );
						AddRadio( 215, 80, 0xD0, 0xD1, false, 12237 );
						AddImage( 300, 135, 0x6F8 );
						AddRadio( 340, 210, 0xD0, 0xD1, false, 12223 );
					}
				}
				else
				{
					AddImage( 70, 20, 0xC60C );
					AddImage( 70, 75, 0xED24 );
					AddImage( 70, 140, 0xED1E );
					AddImage( 193, 18, 0xED26 );
					AddImage( 193, 85, 0xEDE4 );
					AddImage( 193, 140, 0xED23 );
					AddImage( 316, 25, 0xC60F );
					AddImage( 320, 85, 0xED29 );

					AddRadio( 90, 80, 0xD0, 0xD1, false, 8251 );
					AddRadio( 90, 145, 0xD0, 0xD1, false, 8252 );
					AddRadio( 90, 210, 0xD0, 0xD1, false, 8253 );
					AddRadio( 215, 80, 0xD0, 0xD1, false, 8261 );
					AddRadio( 215, 145, 0xD0, 0xD1, false, 8263 );
					AddRadio( 215, 210, 0xD0, 0xD1, false, 8265 );
					AddRadio( 340, 80, 0xD0, 0xD1, false, 8260 );
					AddRadio( 340, 145, 0xD0, 0xD1, false, 8266 );

					if ( from.Female )
					{
						AddImage( 317, 145, 0xED28 );
						AddRadio( 340, 210, 0xD0, 0xD1, false, 8262 );
					}
					else
					{
						AddImage( 315, 150, 0xEDE5 );
						AddRadio( 340, 210, 0xD0, 0xD1, false, 8264 );
					}
				}

				AddRadio( 215, 250, 0xD0, 0xD1, false, 0 );
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( m_Deed.Deleted )
					return;

				if ( info.ButtonID != 1 )
				{
					m_From.SendLocalizedMessage( 1013009 ); // You decide not to change your hairstyle.
					return;
				}

				int[] switches = info.Switches;

				if ( switches.Length == 0 )
					return;

				if ( m_From is PlayerMobile )
				{
					PlayerMobile pm = (PlayerMobile) m_From;

					pm.SetHairMods( -1, -1 ); // clear any hairmods (disguise kit, incognito)
				}

				m_From.HairItemID = switches[0];

				m_Deed.Delete();
			}
		}
	}
}