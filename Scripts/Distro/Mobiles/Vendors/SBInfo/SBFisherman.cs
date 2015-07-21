using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBFisherman : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBFisherman()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( RawFishSteak ), 3, 20, 0x97A, 0 ) );
				Add( new GenericBuyInfo( typeof( Fish ), 6, 80, 0x9CC, 0 ) );
				Add( new GenericBuyInfo( typeof( Fish ), 6, 80, 0x9CD, 0 ) );
				Add( new GenericBuyInfo( typeof( Fish ), 6, 80, 0x9CE, 0 ) );
				Add( new GenericBuyInfo( typeof( Fish ), 6, 80, 0x9CF, 0 ) );
				Add( new GenericBuyInfo( typeof( FishingPole ), 15, 20, 0xDC0, 0 ) );

				Add( new GenericBuyInfo( typeof( AquariumFood ), 62, 20, 0xEFC, 0 ) );
				Add( new GenericBuyInfo( typeof( FishBowl ), 6312, 20, 0x241C, 0x482 ) );
				Add( new GenericBuyInfo( typeof( AquariumNorthDeed ), 250002, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( typeof( AquariumEastDeed ), 250002, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( typeof( AquariumFishingNet ), 250, 20, 0xDC8, 0 ) );
				Add( new GenericBuyInfo( typeof( VacationWafer ), 67, 20, 0x971, 0 ) );
				// TODO: Your New Aquarium Book
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( RawFishSteak ), 1 );
				Add( typeof( Fish ), 1 );
				Add( typeof( FishingPole ), 7 );
			}
		}
	}
}