using Server;

namespace Server.Gumps
{
	public class YoungDungeonWarning : Gump
	{
		public override int TypeID { get { return 0x82; } }

		public YoungDungeonWarning()
			: base( 150, 200 )
		{
			AddPage( 0 );

			AddBackground( 0, 0, 250, 170, 0xA28 );

			AddHtmlLocalized( 20, 43, 215, 70, 1018030, true, true ); // Warning: monsters may attack you on site down here in the dungeons!

			AddButton( 70, 123, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 105, 125, 100, 35, 1011036, false, false ); // OKAY
		}
	}
}