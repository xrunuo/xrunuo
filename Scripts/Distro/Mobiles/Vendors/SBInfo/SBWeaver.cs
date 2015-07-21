using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBWeaver : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBWeaver()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Scissors ), 13, 20, 0xF9F, 0 ) );
				Add( new GenericBuyInfo( typeof( Dyes ), 8, 20, 0xFA9, 0 ) );
				Add( new GenericBuyInfo( typeof( DyeTub ), 9, 20, 0xFAB, 0 ) );
				Add( new GenericBuyInfo( typeof( BoltOfCloth ), 120, 20, 0xf95, 0 ) );
				Add( new GenericBuyInfo( typeof( LightYarnUnraveled ), 18, 20, 0xE1F, 0 ) );
				Add( new GenericBuyInfo( typeof( LightYarn ), 18, 20, 0xE1E, 0 ) );
				Add( new GenericBuyInfo( typeof( DarkYarn ), 18, 20, 0xE1D, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Scissors ), 6 );
				Add( typeof( Dyes ), 4 );
				Add( typeof( DyeTub ), 4 );
				Add( typeof( BoltOfCloth ), 60 );
				Add( typeof( LightYarnUnraveled ), 9 );
				Add( typeof( LightYarn ), 9 );
				Add( typeof( DarkYarn ), 9 );
			}
		}
	}
}