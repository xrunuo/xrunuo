using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRangedWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBRangedWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Bolt ), 6, Utility.Random( 30, 60 ), 0x1BFB, 0 ) );
				Add( new GenericBuyInfo( typeof( Arrow ), 3, Utility.Random( 30, 60 ), 0xF3F, 0 ) );
				Add( new GenericBuyInfo( typeof( Shaft ), 3, Utility.Random( 30, 60 ), 0x1BD4, 0 ) );
				Add( new GenericBuyInfo( typeof( Feather ), 2, Utility.Random( 30, 60 ), 0x1BD1, 0 ) );

				Add( new GenericBuyInfo( typeof( HeavyCrossbow ), 56, 20, 0x13FD, 0 ) );
				Add( new GenericBuyInfo( typeof( Bow ), 46, 20, 0x13B2, 0 ) );
				Add( new GenericBuyInfo( typeof( Crossbow ), 46, 20, 0xF50, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Bolt ), 3 );
				Add( typeof( Arrow ), 2 );
				Add( typeof( Shaft ), 1 );
				Add( typeof( Feather ), 1 );

				Add( typeof( HeavyCrossbow ), 28 );
				Add( typeof( Bow ), 23 );
				Add( typeof( Crossbow ), 23 );

				Add( typeof( CompositeBow ), 23 );
				Add( typeof( RepeatingCrossbow ), 28 );
			}
		}
	}
}