using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurTinker : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurTinker()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Making Valuables With Basket Weaving", typeof( BasketWeavingBook ), 10625, 20, 0xFBE, 0 ) );
				// TODO (SA): Statuette Engraving Tool
				Add( new GenericBuyInfo( typeof( AudChar ), 33, 20, 0x403B, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}