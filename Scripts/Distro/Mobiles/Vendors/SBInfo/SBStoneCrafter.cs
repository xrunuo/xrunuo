using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBStoneCrafter : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBStoneCrafter()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Making Valuables With Stonecrafting", typeof( MasonryBook ), 10625, 10, 0xFBE, 0 ) );
				Add( new GenericBuyInfo( "Mining For Quality Stone", typeof( StoneMiningBook ), 10625, 10, 0xFBE, 0 ) );
				Add( new GenericBuyInfo( "1044515", typeof( MalletAndChisel ), 3, 50, 0x12B3, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( MasonryBook ), 5000 );
				Add( typeof( StoneMiningBook ), 5000 );
				Add( typeof( MalletAndChisel ), 1 );
			}
		}
	}
}