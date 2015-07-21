using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTokunoRangedWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTokunoRangedWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Yumi ), 53, 20, 0x27A5, 0 ) );
				Add( new GenericBuyInfo( typeof( Fukiya ), 20, 20, 0x27F5, 0 ) );
				Add( new GenericBuyInfo( typeof( Nunchaku ), 35, 20, 0x27AE, 0 ) );
				Add( new GenericBuyInfo( typeof( FukiyaDart ), 3, 20, 0x2806, 0 ) );
				Add( new GenericBuyInfo( typeof( Bokuto ), 21, 20, 0x27A8, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bokuto ), 10 );
				Add( typeof( FukiyaDart ), 1 );
				Add( typeof( Nunchaku ), 17 );
				Add( typeof( Fukiya ), 10 );
				Add( typeof( Yumi ), 26 );
			}
		}
	}
}