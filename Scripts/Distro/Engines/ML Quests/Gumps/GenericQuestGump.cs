using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
	public delegate void GenericQuestGumpResponse( Mobile from );

	[Flags]
	public enum GenericQuestGumpButton
	{
		Accept = 0x01,
		Refuse = 0x02,
		Continue = 0x04,
		Close = 0x08
	}

	public class GenericQuestGump : Gump
	{
		public GenericQuestGumpResponse m_OnAccept, m_OnCancel;

		public GenericQuestGump( int textCliloc )
			: this( textCliloc, GenericQuestGumpButton.Close, null, null )
		{
		}

		public GenericQuestGump( int textCliloc, GenericQuestGumpButton buttons, GenericQuestGumpResponse onAccepted )
			: this( textCliloc, buttons, onAccepted, null )
		{
		}

		public GenericQuestGump( int textCliloc, GenericQuestGumpButton buttons, GenericQuestGumpResponse onAccepted, GenericQuestGumpResponse onRefused )
			: base( 75, 25 )
		{
			m_OnAccept = onAccepted;
			m_OnCancel = onRefused;

			Disposable = false;
			Closable = false;

			AddImageTiled( 50, 20, 400, 400, 0x1404 );
			AddImageTiled( 83, 15, 350, 15, 0x280A );
			AddImageTiled( 50, 29, 30, 390, 0x28DC );
			AddImageTiled( 34, 140, 17, 279, 0x242F );
			AddImage( 48, 135, 0x28AB );
			AddImage( -16, 285, 0x28A2 );
			AddImage( 0, 10, 0x28B5 );
			AddImage( 25, 0, 0x28B4 );
			AddImageTiled( 415, 29, 44, 390, 0xA2D );
			AddImageTiled( 415, 29, 30, 390, 0x28DC );
			AddLabel( 100, 50, 0x481, "" );
			AddImage( 370, 50, 0x589 );
			AddImage( 379, 60, 0x15E8 );
			AddImage( 425, 0, 0x28C9 );
			AddImage( 34, 419, 0x2842 );
			AddImage( 442, 419, 0x2840 );
			AddImageTiled( 51, 419, 392, 17, 0x2775 );
			AddHtmlLocalized( 130, 45, 270, 16, 1049010, 0x7FFF, false, false ); // Quest Offer
			AddHtmlLocalized( 98, 156, 312, 180, textCliloc, 0x15F90, false, true );
			AddImage( 90, 33, 0x232D );
			AddImageTiled( 130, 65, 175, 1, 0x238D );

			if ( ( buttons & GenericQuestGumpButton.Accept ) != 0 )
				AddButton( 95, 395, 0x2EE0, 0x2EE2, 1, GumpButtonType.Reply, 0 );

			if ( ( buttons & GenericQuestGumpButton.Refuse ) != 0 )
				AddButton( 313, 395, 0x2EF2, 0x2EF4, 0, GumpButtonType.Reply, 0 );

			if ( ( buttons & GenericQuestGumpButton.Continue ) != 0 )
				AddButton( 95, 395, 0x2EE9, 0x2EEB, 1, GumpButtonType.Reply, 0 );

			if ( ( buttons & GenericQuestGumpButton.Close ) != 0 )
				AddButton( 95, 395, 0x2EE6, 0x2EE8, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			sender.Mobile.CloseGump( typeof( GenericQuestGump ) );

			if ( info.ButtonID == 1 && m_OnAccept != null )
				m_OnAccept( sender.Mobile );
			else if ( m_OnCancel != null )
				m_OnCancel( sender.Mobile );
		}
	}
}