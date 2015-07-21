using System;

namespace Server.ContextMenus
{
	public class AddPartyMemberEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private Mobile m_Mobile;

		public AddPartyMemberEntry( Mobile from, Mobile m )
			: base( 197, 8 )
		{
			m_From = from;
			m_Mobile = m;
		}

		public override void OnClick()
		{
			Engines.PartySystem.Party.Invite( m_From, m_Mobile );
		}
	}
}