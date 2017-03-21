using System;
using System.Threading.Tasks;
using Server;
using Server.Engines.VendorSearch;
using Server.Network;

namespace Server.Gumps
{
	public class VendorSearchWaitGump : Gump
	{
		private Timer m_PollingTimer;

		public VendorSearchWaitGump( Timer waitTimer )
			: base( 10, 10 )
		{
			m_PollingTimer = waitTimer;

			AddPage( 0 );

			AddBackground( 0, 0, 414, 214, 0x7752 );

			// <DIV ALIGN=CENTER>Please wait for your search to complete.</DIV>
			AddHtmlLocalized( 27, 47, 380, 80, 1114513, "#1154678", 0x4E73, false, false );

			AddButton( 7, 167, 0x7747, 0x7747, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 47, 167, 300, 40, 1154677, 0x4E73, false, false ); // Cancel Vendor Search
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			m_PollingTimer.Stop();
		}
	}
}
