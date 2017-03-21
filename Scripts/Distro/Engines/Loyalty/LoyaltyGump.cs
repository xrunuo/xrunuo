using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Loyalty;

namespace Server.Gumps
{
	public class LoyaltyGump : Gump
	{
		public override int TypeID { get { return 0xF3E94; } }

		public LoyaltyGump( PlayerMobile pm )
			: base( 50, 50 )
		{
			LoyaltyInfo info = pm.LoyaltyInfo;

			AddPage( 0 );

			AddImage( 3, 0, 0x820 );
			AddImageTiled( 20, 37, 260, 240, 0x821 );
			AddImage( 20, 257, 0x823 );

			int y = 40;

			for ( int i = 0; i < LoyaltyGroupInfo.Table.Length; i++ )
			{
				LoyaltyGroupInfo group = LoyaltyGroupInfo.Table[i];
				int points = info.GetValue( i );

				AddHtmlLocalized( 50, y, 170, 18, group.Name, 0x0, false, false );
				AddHtmlLocalized( 70, y + 20, 170, 18, group.GetCliloc( points ), 0x0, false, false );
				AddHtmlLocalized( 170, y + 20, 170, 18, 1095171, points.ToString(), 0x0, false, false ); // (~1_AMT~ points)

				y += 40;
			}

			AddHtmlLocalized( 50, 210, 170, 18, 1115129, pm.Fame.ToString(), 0x0, false, false ); // Fame: ~1_AMT~
			AddHtmlLocalized( 50, 230, 170, 18, 1115130, pm.Karma.ToString(), 0x0, false, false ); // Karma: ~1_AMT~
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
		}
	}
}
