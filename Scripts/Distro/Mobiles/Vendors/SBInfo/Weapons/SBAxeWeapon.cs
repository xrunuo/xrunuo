using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBAxeWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBAxeWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( BattleAxe ), 38, 20, 0xF47, 0 ) );
				Add( new GenericBuyInfo( typeof( DoubleAxe ), 32, 20, 0xF4B, 0 ) );
				Add( new GenericBuyInfo( typeof( ExecutionersAxe ), 38, 20, 0xF45, 0 ) );
				Add( new GenericBuyInfo( typeof( LargeBattleAxe ), 43, 20, 0x13FB, 0 ) );
				Add( new GenericBuyInfo( typeof( Pickaxe ), 32, 20, 0xE86, 0 ) );
				Add( new GenericBuyInfo( typeof( TwoHandedAxe ), 42, 20, 0x1443, 0 ) );
				Add( new GenericBuyInfo( typeof( WarAxe ), 38, 20, 0x13B0, 0 ) );
				Add( new GenericBuyInfo( typeof( Axe ), 48, 20, 0xF49, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( BattleAxe ), 36 );
				Add( typeof( DoubleAxe ), 16 );
				Add( typeof( ExecutionersAxe ), 33 );
				Add( typeof( LargeBattleAxe ), 21 );
				Add( typeof( Pickaxe ), 16 );
				Add( typeof( TwoHandedAxe ), 21 );
				Add( typeof( WarAxe ), 19 );
				Add( typeof( Axe ), 24 );
			}
		}
	}
}