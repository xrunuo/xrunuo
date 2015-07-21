using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBHelmetArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBHelmetArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Bascinet ), 127, 20, 0x140C, 0 ) );
				Add( new GenericBuyInfo( typeof( CloseHelm ), 145, 20, 0x1408, 0 ) );
				Add( new GenericBuyInfo( typeof( Helmet ), 116, 20, 0x140A, 0 ) );
				Add( new GenericBuyInfo( typeof( NorseHelm ), 145, 20, 0x140E, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bascinet ), 46 );
				Add( typeof( CloseHelm ), 72 );
				Add( typeof( Helmet ), 58 );
				Add( typeof( NorseHelm ), 53 );
			}
		}
	}
}