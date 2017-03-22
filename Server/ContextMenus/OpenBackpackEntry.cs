using System;
using Server.Items;

namespace Server.ContextMenus
{
	public class OpenBackpackEntry : ContextMenuEntry
	{
		private Mobile m_Mobile;

		public OpenBackpackEntry( Mobile m )
			: base( 6145 )
		{
			m_Mobile = m;
		}

		public override void OnClick()
		{
			m_Mobile.Use( m_Mobile.Backpack );
		}
	}
}