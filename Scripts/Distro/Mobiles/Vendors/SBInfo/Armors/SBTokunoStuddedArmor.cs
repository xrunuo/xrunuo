using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTokunoStuddedArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTokunoStuddedArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( StuddedMempo ), 58, 20, 0x27E8, 0 ) );
				Add( new GenericBuyInfo( typeof( StuddedDo ), 143, 20, 0x277C, 0 ) );
				Add( new GenericBuyInfo( typeof( StuddedHiroSode ), 67, 20, 0x277F, 0 ) );
				Add( new GenericBuyInfo( typeof( StuddedSuneate ), 77, 20, 0x2787, 0 ) );
				Add( new GenericBuyInfo( typeof( StuddedHaidate ), 73, 20, 0x27D6, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( StuddedMempo ), 28 );
				Add( typeof( StuddedDo ), 70 );
				Add( typeof( StuddedHaidate ), 38 );
				Add( typeof( StuddedHiroSode ), 36 );
				Add( typeof( StuddedSuneate ), 38 );
			}
		}
	}
}