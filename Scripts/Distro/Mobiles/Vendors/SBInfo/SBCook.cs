using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCook : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBCook()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( RollingPin ), 2, 20, 0x1043, 0 ) );
				Add( new GenericBuyInfo( typeof( FlourSifter ), 2, 20, 0x103E, 0 ) );
				Add( new GenericBuyInfo( "1044567", typeof( Skillet ), 3, 20, 0x97F, 0 ) );
				Add( new GenericBuyInfo( typeof( JarHoney ), 3, 20, 0x9EC, 0 ) );
				Add( new GenericBuyInfo( typeof( SackFlour ), 3, 20, 0x1039, 0 ) );
				Add( new GenericBuyInfo( typeof( RoastPig ), 106, 20, 0x9BB, 0 ) );
				Add( new GenericBuyInfo( typeof( PewterBowl ), 2, 20, 0x15FD, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfCarrots ), 3, 20, 0x15F9, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfCorn ), 3, 20, 0x15FA, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfLettuce ), 3, 20, 0x15FB, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfPeas ), 3, 20, 0x15FC, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfPotatoes ), 3, 20, 0x1602, 0 ) );
				Add( new GenericBuyInfo( typeof( BowlOfStew ), 3, 20, 0x1604, 0 ) );
				Add( new GenericBuyInfo( typeof( TomatoSoup ), 3, 20, 0x1606, 0 ) );
				Add( new GenericBuyInfo( typeof( ChickenLeg ), 5, 20, 0x1608, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 8, 20, 0x1609, 0 ) );
				Add( new GenericBuyInfo( typeof( CookedBird ), 17, 20, 0x9B7, 0 ) );
				Add( new GenericBuyInfo( typeof( CheeseWheel ), 21, 20, 0x97E, 0 ) );
				Add( new GenericBuyInfo( typeof( Muffins ), 3, 20, 0x9EA, 0 ) );
				Add( new GenericBuyInfo( typeof( Cake ), 13, 20, 0x9E9, 0 ) );
				//Add( new GenericBuyInfo( typeof( BakedPie ), 7, 20, 0x1041, 0 ) );
				Add( new GenericBuyInfo( typeof( BreadLoaf ), 5, 20, 0x103C, 0 ) );
				Add( new GenericBuyInfo( typeof( BreadLoaf ), 5, 20, 0x103B, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( CheeseWheel ), 12 );
				Add( typeof( CookedBird ), 8 );
				Add( typeof( RoastPig ), 53 );
				Add( typeof( Cake ), 5 );
				Add( typeof( JarHoney ), 1 );
				Add( typeof( SackFlour ), 1 );
				Add( typeof( BreadLoaf ), 2 );
				Add( typeof( ChickenLeg ), 3 );
				Add( typeof( LambLeg ), 4 );
				Add( typeof( Skillet ), 1 );
				Add( typeof( FlourSifter ), 1 );
				Add( typeof( RollingPin ), 1 );
				Add( typeof( Muffins ), 1 );
				//Add ( typeof( BakedPie ), 3 );
				Add( typeof( PewterBowl ), 1 );
				Add( typeof( BowlOfCarrots ), 1 );
				Add( typeof( BowlOfCorn ), 1 );
				Add( typeof( BowlOfLettuce ), 1 );
				Add( typeof( BowlOfPeas ), 1 );
				Add( typeof( BowlOfPotatoes ), 1 );
				Add( typeof( BowlOfStew ), 1 );
				Add( typeof( TomatoSoup ), 1 );
			}
		}
	}
}