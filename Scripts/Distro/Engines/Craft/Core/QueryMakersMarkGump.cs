using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Engines.Craft
{
	public class QueryMakersMarkGump : Gump
	{
		public override int TypeID { get { return 0x12E; } }

		private bool m_Exceptional;
		private Mobile m_From;
		private CraftItem m_CraftItem;
		private CraftSystem m_CraftSystem;
		private Type m_TypeRes;
		private BaseTool m_Tool;
		private bool m_QuestItem;

		public QueryMakersMarkGump( bool exceptional, Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, bool questItem )
			: base( 100, 200 )
		{
			from.CloseGump( typeof( QueryMakersMarkGump ) );

			m_Exceptional = exceptional;
			m_From = from;
			m_CraftItem = craftItem;
			m_CraftSystem = craftSystem;
			m_TypeRes = typeRes;
			m_Tool = tool;
			m_QuestItem = questItem;

			AddPage( 0 );

			AddBackground( 0, 0, 220, 170, 5054 );
			AddBackground( 10, 10, 200, 150, 3000 );

			AddHtmlLocalized( 20, 20, 180, 80, 1018317, false, false ); // Do you wish to place your maker's mark on this item?

			AddHtmlLocalized( 55, 100, 140, 25, 1011036, false, false ); // OKAY
			AddButton( 20, 100, 4005, 4007, 2, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 55, 125, 140, 25, 1011012, false, false ); // CANCEL
			AddButton( 20, 125, 4005, 4007, 1, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
			bool makersMark = ( info.ButtonID == 2 );

			if ( makersMark )
				m_From.SendLocalizedMessage( 501808 ); // You mark the item.
			else
				m_From.SendLocalizedMessage( 501809 ); // Cancelled mark.

			m_CraftItem.CompleteCraft( m_Exceptional, makersMark, m_QuestItem, m_From, m_CraftSystem, m_TypeRes, m_Tool, null );
		}
	}
}