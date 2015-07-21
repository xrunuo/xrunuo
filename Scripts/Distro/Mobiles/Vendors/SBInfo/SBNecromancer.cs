using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBNecromancer : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBNecromancer()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Type[] types = Loot.RegularScrollTypes;

				int circles = 3;

				for ( int i = 0; i < circles * 8 && i < types.Length; ++i )
				{
					int itemID = 0x1F2E + i;

					if ( i == 6 )
						itemID = 0x1F2D;
					else if ( i > 6 )
						--itemID;

					Add( new GenericBuyInfo( types[i], 12 + ( ( i / 8 ) * 10 ), 20, itemID, 0 ) );
				}

				Add( new GenericBuyInfo( typeof( BlackPearl ), 5, 999, 0xF7A, 0 ) );
				Add( new GenericBuyInfo( typeof( Bloodmoss ), 7, 999, 0xF7B, 0 ) );
				Add( new GenericBuyInfo( typeof( MandrakeRoot ), 3, 999, 0xF86, 0 ) );
				Add( new GenericBuyInfo( typeof( Garlic ), 3, 999, 0xF84, 0 ) );
				Add( new GenericBuyInfo( typeof( Ginseng ), 3, 999, 0xF85, 0 ) );
				Add( new GenericBuyInfo( typeof( Nightshade ), 4, 999, 0xF88, 0 ) );
				Add( new GenericBuyInfo( typeof( SpidersSilk ), 3, 999, 0xF8D, 0 ) );
				Add( new GenericBuyInfo( typeof( SulfurousAsh ), 4, 999, 0xF8C, 0 ) );

				Add( new GenericBuyInfo( typeof( BatWing ), 4, 999, 0xF78, 0 ) );
				Add( new GenericBuyInfo( typeof( GraveDust ), 4, 999, 0xF8F, 0 ) );
				Add( new GenericBuyInfo( typeof( DaemonBlood ), 4, 999, 0xF7D, 0 ) );
				Add( new GenericBuyInfo( typeof( NoxCrystal ), 4, 999, 0xF8E, 0 ) );
				Add( new GenericBuyInfo( typeof( PigIron ), 4, 999, 0xF8A, 0 ) );

				Add( new GenericBuyInfo( typeof( NecromancerSpellbook ), 150, 10, 0x2253, 0 ) );

				Add( new GenericBuyInfo( typeof( WizardsHat ), 30, 10, 0x1718, 0 ) );

				//Add( new GenericBuyInfo( "1041267", typeof( Runebook ), 2500, 10, 0xEFA, 0x461 ) );

				Add( new GenericBuyInfo( typeof( RecallRune ), 25, 10, 0x1f14, 0 ) );
				Add( new GenericBuyInfo( typeof( Spellbook ), 50, 10, 0xEFA, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( WizardsHat ), 15 );
				Add( typeof( BlackPearl ), 3 );
				Add( typeof( Bloodmoss ), 4 );
				Add( typeof( MandrakeRoot ), 2 );
				Add( typeof( Garlic ), 2 );
				Add( typeof( Ginseng ), 2 );
				Add( typeof( Nightshade ), 2 );
				Add( typeof( SpidersSilk ), 2 );
				Add( typeof( SulfurousAsh ), 2 );
				Add( typeof( RecallRune ), 13 );
				Add( typeof( Spellbook ), 25 );

				Type[] types = Loot.RegularScrollTypes;

				for ( int i = 0; i < types.Length; ++i )
					Add( types[i], ( ( i / 8 ) + 2 ) * 5 );
			}
		}
	}
}