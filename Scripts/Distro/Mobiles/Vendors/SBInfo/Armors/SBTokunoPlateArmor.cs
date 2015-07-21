using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTokunoPlateArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTokunoPlateArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( PlateHatsuburi ), 76, 20, 0x2775, 0 ) );
				Add( new GenericBuyInfo( typeof( HeavyPlateJingasa ), 76, 20, 0x2777, 0 ) );
				Add( new GenericBuyInfo( typeof( DecorativePlateKabuto ), 95, 20, 0x27C3, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateDo ), 320, 20, 0x277D, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateHiroSode ), 206, 20, 0x2780, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateSuneate ), 245, 20, 0x2788, 0 ) );
				Add( new GenericBuyInfo( typeof( PlateHaidate ), 251, 20, 0x278D, 0 ) );
				Add( new GenericBuyInfo( typeof( ChainHatsuburi ), 76, 20, 0x2774, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( ChainHatsuburi ), 38 );
				Add( typeof( PlateHaidate ), 114 );
				Add( typeof( PlateSuneate ), 110 );
				Add( typeof( PlateHiroSode ), 121 );
				Add( typeof( PlateDo ), 164 );
				Add( typeof( DecorativePlateKabuto ), 47 );
				Add( typeof( HeavyPlateJingasa ), 38 );
				Add( typeof( PlateHatsuburi ), 38 );
			}
		}
	}
}