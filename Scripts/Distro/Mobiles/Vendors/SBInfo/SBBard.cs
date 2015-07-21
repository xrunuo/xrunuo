using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBBard : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBard()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( LapHarp ), 30, ( 10 ), 0x0EB2, 0 ) );
				Add( new GenericBuyInfo( typeof( Lute ), 40, ( 10 ), 0x0EB3, 0 ) );
				Add( new GenericBuyInfo( typeof( Drums ), 50, ( 10 ), 0x0E9C, 0 ) );
				Add( new GenericBuyInfo( typeof( Harp ), 90, ( 10 ), 0x0EB1, 0 ) );
				Add( new GenericBuyInfo( typeof( Tambourine ), 60, ( 10 ), 0x0E9E, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( LapHarp ), 15 );
				Add( typeof( Lute ), 20 );
				Add( typeof( Drums ), 25 );
				Add( typeof( Harp ), 45 );
				Add( typeof( Tambourine ), 30 );
			}
		}
	}
}