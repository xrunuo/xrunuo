using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMystic : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMystic()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( EnchantScroll ), 23, 500, 0x2DA1, 0 ) );
				Add( new GenericBuyInfo( typeof( PurgeMagicScroll ), 18, 500, 0x2DA0, 0 ) );
				Add( new GenericBuyInfo( typeof( HealingStoneScroll ), 13, 500, 0x2D9F, 0 ) );
				Add( new GenericBuyInfo( typeof( NetherBoltScroll ), 8, 500, 0x2D9E, 0 ) );

				Add( new GenericBuyInfo( typeof( FertileDirt ), 2, 20, 0xF81, 0 ) );
				Add( new GenericBuyInfo( typeof( Bone ), 3, 20, 0xF7E, 0 ) );
				Add( new GenericBuyInfo( typeof( SpidersSilk ), 3, 20, 0xF8D, 0 ) );
				Add( new GenericBuyInfo( typeof( SulfurousAsh ), 3, 20, 0xF8C, 0 ) );
				Add( new GenericBuyInfo( typeof( Nightshade ), 3, 20, 0xF88, 0 ) );
				Add( new GenericBuyInfo( typeof( MandrakeRoot ), 3, 20, 0xF86, 0 ) );
				Add( new GenericBuyInfo( typeof( Ginseng ), 3, 20, 0xF85, 0 ) );
				Add( new GenericBuyInfo( typeof( Garlic ), 3, 20, 0xF84, 0 ) );
				Add( new GenericBuyInfo( typeof( Bloodmoss ), 5, 20, 0xF7B, 0 ) );
				Add( new GenericBuyInfo( typeof( BlackPearl ), 5, 20, 0xF7A, 0 ) );

				Add( new GenericBuyInfo( typeof( LesserExplosionPotion ), 21, 500, 0xF0D, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserCurePotion ), 15, 500, 0xF07, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserPoisonPotion ), 15, 500, 0xF0A, 0 ) );
				Add( new GenericBuyInfo( typeof( StrengthPotion ), 15, 500, 0xF09, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserHealPotion ), 15, 500, 0xF0C, 0 ) );
				Add( new GenericBuyInfo( typeof( NightSightPotion ), 15, 500, 0xF06, 0 ) );
				Add( new GenericBuyInfo( typeof( AgilityPotion ), 15, 500, 0xF08, 0 ) );
				Add( new GenericBuyInfo( typeof( RefreshPotion ), 15, 500, 0xF0B, 0 ) );

				Add( new GenericBuyInfo( typeof( RecallRune ), 15, 20, 0x1F14, 0 ) );

				Add( new GenericBuyInfo( typeof( BlankScroll ), 5, 20, 0x0E34, 0 ) );
				Add( new GenericBuyInfo( typeof( ScribesPen ), 8, 20, 0xFBF, 0 ) );

				Add( new GenericBuyInfo( typeof( MysticismSpellbook ), 18, 20, 0x2D9D, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( MysticismSpellbook ), 9 );

				Add( typeof( RecallRune ), 7 );

				Add( typeof( BlankScroll ), 2 );

				Add( typeof( NightSightPotion ), 14 );
				Add( typeof( AgilityPotion ), 14 );
				Add( typeof( StrengthPotion ), 14 );
				Add( typeof( RefreshPotion ), 14 );
				Add( typeof( LesserCurePotion ), 14 );
				Add( typeof( LesserHealPotion ), 14 );
				Add( typeof( LesserPoisonPotion ), 14 );
				Add( typeof( LesserExplosionPotion ), 20 );

				Add( typeof( BlackPearl ), 3 );
				Add( typeof( Bloodmoss ), 3 );
				Add( typeof( MandrakeRoot ), 2 );
				Add( typeof( Garlic ), 2 );
				Add( typeof( Ginseng ), 2 );
				Add( typeof( Nightshade ), 2 );
				Add( typeof( SpidersSilk ), 2 );
				Add( typeof( SulfurousAsh ), 2 );
				Add( typeof( Bone ), 2 );
				Add( typeof( FertileDirt ), 2 );

				Type[] types = Loot.MysticScrollTypes;

				for ( int i = 0; i < types.Length; ++i )
					Add( types[i], 7 + ( i * 5 ) );
			}
		}
	}
}