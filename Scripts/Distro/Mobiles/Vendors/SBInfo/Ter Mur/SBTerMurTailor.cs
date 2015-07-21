using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurTailor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurTailor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Scissors ), 13, 20, 0xF9F, 0 ) );
				Add( new GenericBuyInfo( typeof( SewingKit ), 3, 20, 0xF9D, 0 ) );
				Add( new GenericBuyInfo( typeof( Dyes ), 8, 20, 0xFA9, 0 ) );
				Add( new GenericBuyInfo( typeof( DyeTub ), 9, 20, 0xFAB, 0 ) );

				Add( new GenericBuyInfo( typeof( BoltOfCloth ), 120, 20, 0xf95, 0 ) );

				Add( new GenericBuyInfo( typeof( GargishRobe ), 32, 20, 0x4000, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishFancyRobe ), 46, 20, 0x4002, 0 ) );

				Add( new GenericBuyInfo( typeof( FemaleGargishClothArms ), 57, 20, 0x405F, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishClothArms ), 66, 20, 0x4060, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishClothChest ), 73, 20, 0x4061, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishClothChest ), 83, 20, 0x4062, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishClothLeggings ), 60, 20, 0x4065, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishClothLeggings ), 60, 20, 0x4066, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishClothKilt ), 57, 20, 0x4063, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishClothKilt ), 55, 20, 0x4064, 0 ) );

				Add( new GenericBuyInfo( typeof( SpoolOfThread ), 18, 20, 0xFA0, 0 ) );
				Add( new GenericBuyInfo( typeof( Flax ), 102, 20, 0x1A9C, 0 ) );
				Add( new GenericBuyInfo( typeof( Cotton ), 102, 20, 0xDF9, 0 ) );
				Add( new GenericBuyInfo( typeof( Wool ), 62, 20, 0xDF8, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Scissors ), 6 );
				Add( typeof( SewingKit ), 1 );
				Add( typeof( Dyes ), 4 );
				Add( typeof( DyeTub ), 4 );

				Add( typeof( BoltOfCloth ), 60 );

				Add( typeof( GargishRobe ), 16 );
				Add( typeof( GargishFancyRobe ), 23 );

				Add( typeof( FemaleGargishClothArms ), 32 );
				Add( typeof( GargishClothArms ), 33 );
				Add( typeof( FemaleGargishClothChest ), 36 );
				Add( typeof( GargishClothChest ), 41 );
				Add( typeof( FemaleGargishClothLeggings ), 34 );
				Add( typeof( GargishClothLeggings ), 35 );
				Add( typeof( FemaleGargishClothKilt ), 31 );
				Add( typeof( GargishClothKilt ), 30 );

				Add( typeof( SpoolOfThread ), 9 );

				Add( typeof( Flax ), 51 );
				Add( typeof( Cotton ), 51 );
				Add( typeof( Wool ), 31 );
			}
		}
	}
}