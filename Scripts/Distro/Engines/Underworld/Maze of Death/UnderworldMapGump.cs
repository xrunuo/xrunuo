using System;
using Server;
using Server.Network;

namespace Server.Gumps
{
	public class UnderworldMapGump : Gump
	{
		public override int TypeID { get { return 0xF3E97; } }

		public UnderworldMapGump()
			: base( 100, 100 )
		{
			AddImage( 0, 0, 0x7739 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
		}
	}
}