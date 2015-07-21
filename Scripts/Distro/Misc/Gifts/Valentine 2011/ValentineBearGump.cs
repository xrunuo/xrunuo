using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class ValentineBearGump : Gump
	{
		public override int TypeID { get { return 0xF3EAA; } }

		private ValentineBear m_Owner;

		public ValentineBearGump( ValentineBear bear )
			: base( 100, 100 )
		{
			m_Owner = bear;

			AddPage( 0 );

			AddBackground( 0, 0, 420, 320, 0x2454 );
			AddHtmlLocalized( 10, 10, 400, 18, 1114513, "#1150294", 0x4000, false, false ); // <DIV ALIGN=CENTER>St. Valentine Bear</DIV>
			
			AddHtmlLocalized( 10, 34, 400, 90, 1150293, 0x14AA, false, false ); // Enter up to three lines of personalized greeting for your St. Valentine Bear. You many enter up to 25 characters per line. Once you enter text, you will only be able to correct mistakes for 10 minutes.
			
			AddHtmlLocalized( 10, 130, 400, 16, 1150296, 0x1F0, false, false ); // Line 1:
			AddBackground( 10, 146, 400, 24, 0x2486 );
			AddTextEntry( 12, 148, 396, 20, 0x9C2, 0, bear.Line1, 25 );
			
			AddHtmlLocalized( 10, 170, 400, 16, 1150297, 0x1F0, false, false ); // Line 2:
			AddBackground( 10, 186, 400, 24, 0x2486 );
			AddTextEntry( 12, 188, 396, 20, 0x9C2, 1, bear.Line2, 25 );
			
			AddHtmlLocalized( 10, 210, 400, 16, 1150298, 0x1F0, false, false ); // Line 3:
			AddBackground( 10, 226, 400, 24, 0x2486 );
			AddTextEntry( 12, 228, 396, 20, 0x9C2, 2, bear.Line3, 25 );
			
			AddButton( 10, 290, 0xFAB, 0xFAC, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 50, 290, 100, 20, 1150299, 0x10, false, false ); // ACCEPT
			
			AddButton( 380, 290, 0xFB4, 0xFB5, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 270, 290, 100, 20, 1114514, "#1150300", 0x3E00, false, false ); // <DIV ALIGN=RIGHT>CANCEL</DIV>
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if ( info.ButtonID == 1 )
			{
				if ( !m_Owner.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1116249 ); // That must be in your backpack for you to use it.
				}
				else
				{
					m_Owner.Line1 = info.GetTextEntry( 0 ).Text;
					m_Owner.Line2 = info.GetTextEntry( 1 ).Text;
					m_Owner.Line3 = info.GetTextEntry( 2 ).Text;

					m_Owner.EditEnd = DateTime.Now + TimeSpan.FromMinutes( 10.0 );
				}
			}
		}
	}
}