using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBJewel : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBJewel()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "1060740", typeof( BroadcastCrystal ), 68, 20, 0x1ED0, 0, new object[] { 500 } ) ); // 500 charges
				Add( new GenericBuyInfo( "1060740", typeof( BroadcastCrystal ), 131, 20, 0x1ED0, 0, new object[] { 1000 } ) ); // 1000 charges
				Add( new GenericBuyInfo( "1060740", typeof( BroadcastCrystal ), 256, 20, 0x1ED0, 0, new object[] { 2000 } ) ); // 2000 charges

				Add( new GenericBuyInfo( "1060740", typeof( ReceiverCrystal ), 6, 20, 0x1ED0, 0 ) );

				Add( new GenericBuyInfo( typeof( Amber ), 90, 20, 0xF25, 0 ) );
				Add( new GenericBuyInfo( typeof( Amethyst ), 120, 20, 0xF16, 0 ) );
				Add( new GenericBuyInfo( typeof( Citrine ), 60, 20, 0xF15, 0 ) );
				Add( new GenericBuyInfo( typeof( Diamond ), 240, 20, 0xF26, 0 ) );
				Add( new GenericBuyInfo( typeof( Emerald ), 120, 20, 0xF10, 0 ) );
				Add( new GenericBuyInfo( typeof( Ruby ), 90, 20, 0xF13, 0 ) );
				Add( new GenericBuyInfo( typeof( Sapphire ), 120, 20, 0xF19, 0 ) );
				Add( new GenericBuyInfo( typeof( StarSapphire ), 150, 20, 0xF21, 0 ) );
				Add( new GenericBuyInfo( typeof( Tourmaline ), 90, 20, 0xF2D, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Amber ), 45 );
				Add( typeof( Amethyst ), 60 );
				Add( typeof( Citrine ), 30 );
				Add( typeof( Diamond ), 120 );
				Add( typeof( Emerald ), 60 );
				Add( typeof( Ruby ), 45 );
				Add( typeof( Sapphire ), 60 );
				Add( typeof( StarSapphire ), 75 );
				Add( typeof( Tourmaline ), 45 );
			}
		}
	}
}