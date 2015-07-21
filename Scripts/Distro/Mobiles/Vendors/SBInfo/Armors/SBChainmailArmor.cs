using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBChainmailArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBChainmailArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( ChainChest ), 207, 20, 0x13BF, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainLegs ), 166, 20, 0x13BE, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainCoif ), 130, 20, 0x13BB, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( ChainChest ), 103 );
				Add( typeof( ChainLegs ), 83 );
				Add( typeof( ChainCoif ), 65 );
			}
		}
	}
}