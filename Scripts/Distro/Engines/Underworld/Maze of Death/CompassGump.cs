using System;
using Server;
using Server.Network;

namespace Server.Gumps
{
	[Flags]
	public enum CompassDirection
	{
		None = 0x00,
		North = 0x01,
		South = 0x02,
		East = 0x04,
		West = 0x08
	}

	public class CompassGump : Gump
	{
		public override int TypeID { get { return 0x323071; } }

		public CompassGump( CompassDirection dir )
			: base( 100, 100 )
		{
			AddImage( 0, 0, 0x232F );
			AddAlphaRegion( 0, 0, 200, 200 );

			if ( ( dir & CompassDirection.West ) != 0 )
				AddImage( 50, 50, 0x119B );
			if ( ( dir & CompassDirection.North ) != 0 )
				AddImage( 100, 50, 0x1195 );
			if ( ( dir & CompassDirection.South ) != 0 )
				AddImage( 50, 100, 0x1199 );
			if ( ( dir & CompassDirection.East ) != 0 )
				AddImage( 100, 100, 0x1197 );
		}

		public override void OnResponse( GameClient sender, RelayInfo info )
		{
		}
	}
}