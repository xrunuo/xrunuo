using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurTanner : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurTanner()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Bag ), 6, 20, 0xE76, 0 ) );
				Add( new GenericBuyInfo( typeof( Pouch ), 6, 20, 0xE79, 0 ) );
				Add( new GenericBuyInfo( typeof( Leather ), 6, 20, 0x1081, 0 ) );
				Add( new GenericBuyInfo( "1041279", typeof( TaxidermyKit ), 100000, 20, 0x1EBA, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishDagger ), 20, 20, 0x902, 0 ) );
				Add( new GenericBuyInfo( typeof( Backpack ), 15, 20, 0x9B2, 0 ) );

				Add( new GenericBuyInfo( typeof( FemaleGargishLeatherArms ), 83, 20, 0x4047, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishLeatherArms ), 93, 20, 0x4048, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishLeatherChest ), 85, 20, 0x4049, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishLeatherChest ), 80, 20, 0x404A, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishLeatherKilt ), 100, 20, 0x404B, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishLeatherKilt ), 86, 20, 0x404C, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishLeatherLeggings ), 75, 20, 0x404D, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishLeatherLeggings ), 83, 20, 0x404E, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( GargishDagger ), 10 );

				Add( typeof( FemaleGargishLeatherArms ), 38 );
				Add( typeof( GargishLeatherArms ), 45 );
				Add( typeof( FemaleGargishLeatherChest ), 45 );
				Add( typeof( GargishLeatherChest ), 40 );
				Add( typeof( FemaleGargishLeatherKilt ), 50 );
				Add( typeof( GargishLeatherKilt ), 48 );
				Add( typeof( FemaleGargishLeatherLeggings ), 37 );
				Add( typeof( GargishLeatherLeggings ), 37 );
			}
		}
	}
}