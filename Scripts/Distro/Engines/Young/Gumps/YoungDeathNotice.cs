using Server;

namespace Server.Gumps
{
	public class YoungDeathNotice : Gump
	{
		public override int TypeID { get { return 0x28D; } }

		public YoungDeathNotice()
			: base( 100, 15 )
		{
			AddPage( 0 );

			Closable = false;

			AddBackground( 25, 10, 425, 444, 0x13BE );

			AddImageTiled( 33, 20, 407, 425, 0xA40 );
			AddAlphaRegion( 33, 20, 407, 425 );

			AddHtmlLocalized( 190, 24, 120, 20, 1046287, 0x7D00, false, false ); // You have died.

			// As a ghost you cannot interact with the world. You cannot touch items nor can you use them.
			AddHtmlLocalized( 50, 50, 380, 40, 1046288, 0xFFFFFF, false, false );
			// You can pass through doors as though they do not exist.  However, you cannot pass through walls.
			AddHtmlLocalized( 50, 100, 380, 45, 1046289, 0xFFFFFF, false, false );
			// Since you are a new player, any items you had on your person at the time of your death will be in your backpack upon resurrection.
			AddHtmlLocalized( 50, 140, 380, 60, 1046291, 0xFFFFFF, false, false );
			// To be resurrected you must find a healer in town or wandering in the wilderness.  Some powerful players may also be able to resurrect you.
			AddHtmlLocalized( 50, 204, 380, 65, 1046292, 0xFFFFFF, false, false );
			// While you are still in young status, you will be transported to the nearest healer (along with your items) at the time of your death.
			AddHtmlLocalized( 50, 269, 380, 65, 1046293, 0xFFFFFF, false, false );
			// To rejoin the world of the living simply walk near one of the NPC healers, and they will resurrect you as long as you are not marked as a criminal.
			AddHtmlLocalized( 50, 334, 380, 70, 1046294, 0xFFFFFF, false, false );

			AddButton( 195, 410, 0xF8, 0xF9, 1, GumpButtonType.Reply, 0 );
		}
	}
}
