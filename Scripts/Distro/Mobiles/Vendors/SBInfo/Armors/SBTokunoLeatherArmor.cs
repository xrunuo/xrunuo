using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTokunoLeatherArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTokunoLeatherArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( LeatherJingasa ), 11, 20, 0x2776, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherDo ), 92, 20, 0x277B, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherHiroSode ), 46, 20, 0x277E, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherSuneate ), 57, 20, 0x2786, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherHaidate ), 52, 20, 0x278A, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherNinjaPants ), 47, 20, 0x2791, 0 ) );
				Add( new GenericBuyInfo( typeof( LeatherNinjaJacket ), 51, 20, 0x2793, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( LeatherJingasa ), 1 );
				Add( typeof( LeatherDo ), 46 );
				Add( typeof( LeatherHiroSode ), 21 );
				Add( typeof( LeatherSuneate ), 28 );
				Add( typeof( LeatherHaidate ), 28 );
				Add( typeof( LeatherNinjaPants ), 23 );
				Add( typeof( LeatherNinjaJacket ), 25 );
			}
		}
	}
}