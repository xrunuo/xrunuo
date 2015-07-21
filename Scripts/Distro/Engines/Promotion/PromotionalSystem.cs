using System;
using System.Collections;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Events;

namespace Server.Engines.Help
{
	public class PromotionalSystem
	{
		public static bool Enabled = false; // if set in true, promotional code system is enabled

		public static bool HasRequest( Mobile m )
		{
			PlayerMobile from = m as PlayerMobile;

			if ( from != null && from.LastPromotionCode != null )
				return true;

			return false;
		}

		public static void Initialize()
		{
			EventSink.Instance.Login += new LoginEventHandler( OnLogin );
		}

		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( HasRequest( from ) )
				from.SendGump( new PromotionMessageGump( false, PromotionalType.None, ( (PlayerMobile) from ).LastPromotionCode ) );
		}
	}

	public class PromotionFailureGump : Gump
	{
		private Mobile from;

		private PromotionalType m_Type;

		private string m_Code;

		public PromotionFailureGump( Mobile m, PromotionalType type, string code )
			: base( 0, 0 )
		{
			from = m;

			m_Type = type;

			m_Code = code;

			AddBackground( 50, 25, 540, 430, 0xA28 );

			AddPage( 0 );

			AddHtmlLocalized( 150, 40, 360, 40, 1062610, false, false ); // <CENTER><U>Ultima Online Help Response</U></CENTER>

			AddHtml( 80, 90, 480, 290, String.Format( "PROMO_AGENT tells {0}: Promotion redemption failed: specified promotion code is invalid. Please contact a Game Master if you need additional support.", from.Name ), true, true );

			AddHtmlLocalized( 80, 390, 480, 40, 1062611, false, false ); // Clicking the OKAY button will remove the response you have received.

			AddButton( 400, 417, 0x81A, 0x81B, 1, GumpButtonType.Reply, 0 );

			AddButton( 475, 417, 0x819, 0x818, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 0: // Cancel
					{
						bool success = false;

						// TODO: Send query for inputed promo-code to database
						// We must receive following result: bool success

						from.SendGump( new PromotionMessageGump( success, m_Type, m_Code ) );

						break;
					}
				case 1: // OK
					{
						( (PlayerMobile) from ).LastPromotionCode = null;

						break;
					}
			}
		}
	}

	public class PromotionMessageGump : Gump
	{
		public override int TypeID { get { return 0xC9; } }

		private bool m_Success;
		private PromotionalType m_Type;
		private string m_Code;

		public PromotionMessageGump( bool success, PromotionalType type, string code )
			: base( 0, 0 )
		{
			m_Success = success;

			m_Type = type;

			m_Code = code;

			AddPage( 0 );

			AddButton( 23, 40, 0x845, 0x846, 0, GumpButtonType.Reply, 0 );

			AddLabel( 0, 50, 0x2B, "Message" );
		}

		public override void OnResponse( GameClient state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 0: // Message
					{
						if ( m_Success )
						{
							// TODO: send query to database for removing promotion code for this account

							( (PlayerMobile) from ).LastPromotionCode = null;

							PromotionalToken pt = new PromotionalToken( m_Type );

							from.SendMessage( 0x0, "A token has been placed in your backpack. Double-click it to redeem your promotion." );

							from.AddToBackpack( pt );
						}
						else
						{
							from.SendGump( new PromotionFailureGump( from, m_Type, m_Code ) );
						}

						break;
					}
			}
		}
	}


	public class PromotionalCodeGump : Gump
	{
		public override int TypeID { get { return 0x2332; } }

		public PromotionalCodeGump()
			: base( 0, 0 )
		{
			AddBackground( 50, 50, 400, 300, 0xA28 );

			AddPage( 0 );

			AddHtmlLocalized( 165, 70, 200, 20, 1062516, false, false ); // Enter Promotional Code

			AddHtmlLocalized( 75, 95, 350, 145, 1062869, true, true ); // Enter your promotional code EXACTLY as it was given to you (including dashes). Enter no other text in the box aside from your promotional code.

			AddButton( 125, 290, 0x81A, 0x81B, 1, GumpButtonType.Reply, 0 );
			AddButton( 320, 290, 0x819, 0x818, 0, GumpButtonType.Reply, 0 );

			AddImageTiled( 125, 250, 250, 20, 0xDB0 );
			AddImageTiled( 126, 250, 250, 2, 0x23C5 );
			AddImageTiled( 125, 250, 2, 20, 0x23C3 );
			AddImageTiled( 125, 270, 250, 2, 0x23C5 );
			AddImageTiled( 375, 250, 2, 22, 0x23C3 );

			AddTextEntry( 128, 251, 243, 17, 0xA28, 0, "" );
		}

		public override void OnResponse( GameClient state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			switch ( info.ButtonID )
			{
				case 0: // Close/Cancel
					{
						from.SendLocalizedMessage( 501235, "", 0x35 ); // Help request aborted.

						break;
					}
				case 1: // OK
					{
						TextRelay entry = info.GetTextEntry( 0 );
						string code = ( entry == null ? "" : entry.Text.Trim() );

						if ( code != "" )
						{
							from.SendLocalizedMessage( 1062098 ); // Your promotional code has been submitted.  We are processing your request.

							PromotionalType type = PromotionalType.None;
							bool success = false;
							( (PlayerMobile) from ).LastPromotionCode = code;

							// TODO: Send query for inputed promo-code to database
							// We must receive following results: bool success, PromotionalType type, string code

							from.SendGump( new PromotionMessageGump( success, type, code ) );
						}

						break;
					}
			}
		}
	}
}