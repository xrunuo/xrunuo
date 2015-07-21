using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBSpearForkWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBSpearForkWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Spear ), 38, 20, 0xF62, 0 ) );
				Add( new GenericBuyInfo( typeof( Pitchfork ), 25, 20, 0xE87, 0 ) );
				Add( new GenericBuyInfo( typeof( ShortSpear ), 32, 20, 0x1403, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Spear ), 36 );
				Add( typeof( Pitchfork ), 12 );
				Add( typeof( ShortSpear ), 16 );
			}
		}
	}
}