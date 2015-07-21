using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBButcher : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBButcher()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( SkinningKnife ), 15, 20, 0xEC4, 0 ) );
				Add( new GenericBuyInfo( typeof( Cleaver ), 12, 20, 0xEC3, 0 ) );
				Add( new GenericBuyInfo( typeof( ButcherKnife ), 15, 20, 0x13F6, 0 ) );
				Add( new GenericBuyInfo( typeof( RawRibs ), 7, 20, 0x9F1, 0 ) );
				Add( new GenericBuyInfo( typeof( RawLambLeg ), 5, 20, 0x1609, 0 ) );
				Add( new GenericBuyInfo( typeof( RawBird ), 3, 20, 0x9B9, 0 ) );
				Add( new GenericBuyInfo( typeof( RawChickenLeg ), 2, 20, 0x1607, 0 ) );
				Add( new GenericBuyInfo( typeof( Sausage ), 17, 20, 0x9C0, 0 ) );
				Add( new GenericBuyInfo( typeof( Ham ), 20, 20, 0x9C9, 0 ) );
				Add( new GenericBuyInfo( typeof( Bacon ), 3, 20, 0x979, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( RawRibs ), 3 );
				Add( typeof( RawLambLeg ), 2 );
				Add( typeof( RawChickenLeg ), 1 );
				Add( typeof( RawBird ), 1 );
				Add( typeof( Bacon ), 1 );
				Add( typeof( Sausage ), 8 );
				Add( typeof( Ham ), 10 );
				Add( typeof( ButcherKnife ), 7 );
				Add( typeof( Cleaver ), 6 );
				Add( typeof( SkinningKnife ), 7 );
			}
		}
	}
}