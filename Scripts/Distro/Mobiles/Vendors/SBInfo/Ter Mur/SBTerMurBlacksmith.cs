using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurBlacksmith : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurBlacksmith()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( GemsMiningBook ), 10625, 20, 0xFBE, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishDaisho ), 23, 20, 0x48D1, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishTekagi ), 18, 20, 0x48CF, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishTessen ), 23, 20, 0x48CC, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishWarHammer ), 20, 20, 0x48C1, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishWarFork ), 16, 20, 0x48BF, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishScythe ), 30, 20, 0x48C5, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishPike ), 28, 20, 0x48C9, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishMaul ), 16, 20, 0x48C3, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishLance ), 36, 20, 0x48CB, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishKryss ), 16, 20, 0x48BD, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishKatana ), 16, 20, 0x48BB, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishCleaver ), 8, 20, 0x48AF, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishButcherKnife ), 8, 20, 0x48B7, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishBoneHarvester ), 30, 20, 0x48C7, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishBattleAxe ), 22, 20, 0x48B1, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishBardiche ), 43, 20, 0x48B5, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishAxe ), 25, 20, 0x48B3, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishGnarledStaff ), 11, 20, 0x48B9, 0 ) );
				Add( new GenericBuyInfo( typeof( GlassSword ), 28, 20, 0x090C, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishTalwar ), 65, 20, 0x4075, 0 ) );
				Add( new GenericBuyInfo( typeof( SoulGlaive ), 60, 20, 0x406B, 0 ) );
				Add( new GenericBuyInfo( typeof( Shortblade ), 57, 20, 0x4076, 0 ) );
				Add( new GenericBuyInfo( typeof( SerpentstoneStaff ), 28, 20, 0x406F, 0 ) );
				Add( new GenericBuyInfo( typeof( GlassStaff ), 10, 20, 0x0905, 0 ) );
				Add( new GenericBuyInfo( typeof( DiscMace ), 62, 20, 0x406E, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishDagger ), 20, 20, 0x902, 0 ) );
				Add( new GenericBuyInfo( typeof( Cyclone ), 51, 20, 0x406C, 0 ) );
				Add( new GenericBuyInfo( typeof( Boomerang ), 28, 20, 0x4067, 0 ) );
				Add( new GenericBuyInfo( typeof( Bloodblade ), 47, 20, 0x4072, 0 ) );
				Add( new GenericBuyInfo( typeof( DualShortAxes ), 83, 20, 0x4068, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishStoneArms ), 123, 20, 0x4057, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishStoneArms ), 123, 20, 0x4058, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishStoneChest ), 116, 20, 0x4059, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishStoneChest ), 116, 20, 0x405A, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishStoneKilt ), 121, 20, 0x405B, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishStoneKilt ), 135, 20, 0x405C, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishStoneLeggings ), 118, 20, 0x405D, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishStoneLeggings ), 140, 20, 0x405E, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishPlatemailArms ), 358, 20, 0x404F, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishPlatemailArms ), 395, 20, 0x4050, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishPlatemailChest ), 445, 20, 0x4051, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishPlatemailChest ), 450, 20, 0x4052, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishPlatemailKilt ), 343, 20, 0x4053, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishPlatemailKilt ), 370, 20, 0x4054, 0 ) );
				Add( new GenericBuyInfo( typeof( FemaleGargishPlatemailLeggings ), 366, 20, 0x4055, 0 ) );
				Add( new GenericBuyInfo( typeof( GargishPlatemailLeggings ), 355, 20, 0x4056, 0 ) );

				Add( new GenericBuyInfo( typeof( Tongs ), 13, 20, 0xFBB, 0 ) );
				Add( new GenericBuyInfo( typeof( IronIngot ), 8, 500, 0x1BF2, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( Tongs ), 7 );
				Add( typeof( IronIngot ), 4 );

				Add( typeof( FemaleGargishPlatemailArms ), 164 );
				Add( typeof( GargishPlatemailArms ), 179 );
				Add( typeof( FemaleGargishPlatemailChest ), 240 );
				Add( typeof( GargishPlatemailChest ), 240 );
				Add( typeof( FemaleGargishPlatemailKilt ), 159 );
				Add( typeof( GargishPlatemailKilt ), 182 );
				Add( typeof( FemaleGargishPlatemailLeggings ), 191 );
				Add( typeof( GargishPlatemailLeggings ), 220 );
				Add( typeof( FemaleGargishStoneArms ), 66 );
				Add( typeof( GargishStoneArms ), 63 );
				Add( typeof( FemaleGargishStoneChest ), 60 );
				Add( typeof( GargishStoneChest ), 61 );
				Add( typeof( FemaleGargishStoneKilt ), 61 );
				Add( typeof( GargishStoneKilt ), 67 );
				Add( typeof( FemaleGargishStoneLeggings ), 59 );
				Add( typeof( GargishStoneLeggings ), 70 );
				Add( typeof( DualShortAxes ), 42 );
				Add( typeof( Bloodblade ), 23 );
				Add( typeof( Boomerang ), 13 );
				Add( typeof( Cyclone ), 24 );
				Add( typeof( GargishDagger ), 10 );
				Add( typeof( DiscMace ), 31 );
				Add( typeof( GlassStaff ), 5 );
				Add( typeof( SerpentstoneStaff ), 14 );
				Add( typeof( Shortblade ), 28 );
				Add( typeof( SoulGlaive ), 32 );
				Add( typeof( GargishTalwar ), 31 );
				Add( typeof( GlassSword ), 14 );
				Add( typeof( GargishDaisho ), 11 );
				Add( typeof( GargishTekagi ), 9 );
				Add( typeof( GargishTessen ), 11 );
				Add( typeof( GargishWarHammer ), 7 );
				Add( typeof( GargishWarFork ), 7 );
				Add( typeof( GargishScythe ), 16 );
				Add( typeof( GargishPike ), 16 );
				Add( typeof( GargishMaul ), 6 );
				Add( typeof( GargishLance ), 18 );
				Add( typeof( GargishKryss ), 16 );
				Add( typeof( GargishKatana ), 8 );
				Add( typeof( GargishCleaver ), 4 );
				Add( typeof( GargishButcherKnife ), 4 );
				Add( typeof( GargishBoneHarvester ), 15 );
				Add( typeof( GargishBattleAxe ), 10 );
				Add( typeof( GargishBardiche ), 23 );
				Add( typeof( GargishAxe ), 13 );
				Add( typeof( GargishGnarledStaff ), 5 );
			}
		}
	}
}