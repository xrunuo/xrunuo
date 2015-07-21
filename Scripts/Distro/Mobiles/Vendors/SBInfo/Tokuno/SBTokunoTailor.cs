using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTokunoTailor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTokunoTailor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( ClothNinjaHood ), 33, 20, 0x278F, 0 ) );
				Add( new GenericBuyInfo( typeof( ClothNinjaJacket ), 31, 20, 0x2794, 0 ) );
				Add( new GenericBuyInfo( typeof( Kasa ), 31, 20, 0x27E3, 0 ) );
				Add( new GenericBuyInfo( typeof( Kamishimo ), 38, 20, 0x2799, 0 ) );
				Add( new GenericBuyInfo( typeof( Hakama ), 41, 20, 0x279A, 0 ) );
				Add( new GenericBuyInfo( typeof( TattsukeHakama ), 41, 20, 0x279B, 0 ) );
				Add( new GenericBuyInfo( typeof( HakamaShita ), 36, 20, 0x279C, 0 ) );
				Add( new GenericBuyInfo( typeof( Obi ), 16, 20, 0x27A0, 0 ) );
				Add( new GenericBuyInfo( typeof( JinBaori ), 26, 20, 0x27A1, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( JinBaori ), 13 );
				Add( typeof( Obi ), 8 );
				Add( typeof( JinBaori ), 13 );
				Add( typeof( HakamaShita ), 18 );
				Add( typeof( TattsukeHakama ), 20 );
				Add( typeof( Hakama ), 20 );
				Add( typeof( Kamishimo ), 19 );
				Add( typeof( Kasa ), 15 );
				Add( typeof( ClothNinjaHood ), 16 );
				Add( typeof( ClothNinjaJacket ), 15 );
			}
		}
	}
}