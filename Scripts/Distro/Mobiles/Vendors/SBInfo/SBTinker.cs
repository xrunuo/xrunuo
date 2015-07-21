using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTinker : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTinker()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Drums ), 50, 20, 0x0E9C, 0 ) );
				Add( new GenericBuyInfo( typeof( Tambourine ), 60, 20, 0x0E9E, 0 ) );
				Add( new GenericBuyInfo( typeof( LapHarp ), 30, 20, 0x0EB2, 0 ) );
				Add( new GenericBuyInfo( typeof( Lute ), 40, 20, 0x0EB3, 0 ) );

				Add( new GenericBuyInfo( typeof( Shovel ), 12, 20, 0xF39, 0 ) );
				Add( new GenericBuyInfo( typeof( SewingKit ), 3, 20, 0xF9D, 0 ) );
				Add( new GenericBuyInfo( typeof( Scissors ), 13, 20, 0xF9F, 0 ) );
				Add( new GenericBuyInfo( typeof( Tongs ), 3, 20, 0xFBB, 0 ) );
				Add( new GenericBuyInfo( typeof( Key ), 2, 20, 0x100E, 0, new object[] { KeyType.Copper } ) );
				Add( new GenericBuyInfo( typeof( Key ), 8, 20, 0x100F, 0, new object[] { KeyType.Gold } ) );
				Add( new GenericBuyInfo( typeof( Key ), 8, 20, 0x1010, 0, new object[] { KeyType.Iron } ) );
				Add( new GenericBuyInfo( typeof( Key ), 8, 20, 0x1013, 0, new object[] { KeyType.Rusty } ) );
				Add( new GenericBuyInfo( typeof( KeyRing ), 8, 20, 0x1011, 0 ) );

				Add( new GenericBuyInfo( typeof( DovetailSaw ), 14, 20, 0x1028, 0 ) );
				Add( new GenericBuyInfo( typeof( MouldingPlane ), 13, 20, 0x102C, 0 ) );
				Add( new GenericBuyInfo( typeof( Nails ), 3, 20, 0x102E, 0 ) );
				Add( new GenericBuyInfo( typeof( JointingPlane ), 13, 20, 0x1030, 0 ) );
				Add( new GenericBuyInfo( typeof( SmoothingPlane ), 12, 20, 0x1032, 0 ) );
				Add( new GenericBuyInfo( typeof( Saw ), 18, 20, 0x1034, 0 ) );

				Add( new GenericBuyInfo( typeof( Clock ), 22, 20, 0x104B, 0 ) );
				Add( new GenericBuyInfo( typeof( ClockParts ), 3, 20, 0x104F, 0 ) );
				Add( new GenericBuyInfo( typeof( AxleGears ), 3, 20, 0x1051, 0 ) );
				Add( new GenericBuyInfo( typeof( Gears ), 2, 20, 0x1053, 0 ) );
				Add( new GenericBuyInfo( typeof( Hinge ), 2, 20, 0x1055, 0 ) );
				Add( new GenericBuyInfo( typeof( Sextant ), 25, 20, 0x1057, 0 ) );
				Add( new GenericBuyInfo( typeof( SextantParts ), 5, 20, 0x1059, 0 ) );
				Add( new GenericBuyInfo( typeof( Axle ), 2, 20, 0x105B, 0 ) );
				Add( new GenericBuyInfo( typeof( Springs ), 3, 20, 0x105D, 0 ) );

				Add( new GenericBuyInfo( typeof( DrawKnife ), 12, 20, 0x10E4, 0 ) );
				Add( new GenericBuyInfo( typeof( Froe ), 12, 20, 0x10E5, 0 ) );
				Add( new GenericBuyInfo( typeof( Inshave ), 12, 20, 0x10E6, 0 ) );
				Add( new GenericBuyInfo( typeof( Scorp ), 12, 20, 0x10E7, 0 ) );

				Add( new GenericBuyInfo( typeof( Lockpick ), 12, 20, 0x14FC, 0 ) );
				Add( new GenericBuyInfo( typeof( TinkerTools ), 30, 20, 0x1EBC, 0 ) );

				Add( new GenericBuyInfo( typeof( Pickaxe ), 32, 20, 0xE86, 0 ) );
				Add( new GenericBuyInfo( typeof( SledgeHammer ), 22, 20, 0xFB5, 0 ) );
				Add( new GenericBuyInfo( typeof( Hammer ), 28, 20, 0x102A, 0 ) );
				Add( new GenericBuyInfo( typeof( SmithHammer ), 21, 20, 0x13E3, 0 ) );
				Add( new GenericBuyInfo( typeof( ButcherKnife ), 21, 20, 0x13F6, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Drums ), 10 );
				Add( typeof( Tambourine ), 10 );
				Add( typeof( LapHarp ), 10 );
				Add( typeof( Lute ), 10 );

				Add( typeof( Shovel ), 6 );
				Add( typeof( SewingKit ), 1 );
				Add( typeof( Scissors ), 5 );
				Add( typeof( Tongs ), 1 );
				Add( typeof( Key ), 1 );

				Add( typeof( DovetailSaw ), 6 );
				Add( typeof( MouldingPlane ), 6 );
				Add( typeof( Nails ), 1 );
				Add( typeof( JointingPlane ), 6 );
				Add( typeof( SmoothingPlane ), 6 );
				Add( typeof( Saw ), 9 );

				Add( typeof( Clock ), 11 );
				Add( typeof( ClockParts ), 1 );
				Add( typeof( AxleGears ), 1 );
				Add( typeof( Gears ), 1 );
				Add( typeof( Hinge ), 1 );
				Add( typeof( Sextant ), 6 );
				Add( typeof( SextantParts ), 2 );
				Add( typeof( Axle ), 1 );
				Add( typeof( Springs ), 1 );

				Add( typeof( DrawKnife ), 6 );
				Add( typeof( Froe ), 6 );
				Add( typeof( Inshave ), 6 );
				Add( typeof( Scorp ), 6 );

				Add( typeof( Lockpick ), 6 );
				Add( typeof( TinkerTools ), 3 );

				Add( typeof( Pickaxe ), 8 );
				Add( typeof( Hammer ), 2 ); //was 6
				Add( typeof( SmithHammer ), 2 );
				Add( typeof( ButcherKnife ), 4 );
			}
		}
	}
}