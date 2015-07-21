using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSmithTools : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBSmithTools()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Tongs ), 3, 14, 0xFBB, 0 ) );
				Add( new GenericBuyInfo( typeof( SmithHammer ), 4, 16, 0x13E3, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Tongs ), 1 );
				Add( typeof( SmithHammer ), 2 );
			}
		}
	}
}