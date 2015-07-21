using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Craft;
using Server.Engines.Loyalty;

namespace Server.Engines.Quests
{
	public class BaseReward
	{
		private BaseQuest m_Quest;

		public BaseQuest Quest
		{
			get { return m_Quest; }
			set { m_Quest = value; }
		}

		private Type m_Type;
		private int m_Amount;
		private object m_Name;

		public Type Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public int Amount
		{
			get { return m_Amount; }
			set { m_Amount = value; }
		}

		public object Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public BaseReward( object name )
			: this( null, 1, name )
		{
		}

		public BaseReward( Type type, object name )
			: this( type, 1, name )
		{
		}

		public BaseReward( Type type, int amount, object name )
		{
			m_Type = type;
			m_Amount = amount;
			m_Name = name;
		}

		public virtual void GiveReward()
		{
		}

		#region Hues
		private static int[] m_SatchelHues = new int[]
		{
			0x1C,	0x37, 	0x71, 	0x3A,	0x62,	0x44,
			0x59,	0x13,	0x21,	0x3,	0xD,	0x3F,
		};	// TODO update

		public static int SatchelHue()
		{
			return m_SatchelHues[Utility.Random( m_SatchelHues.Length )];
		}

		private static int[] m_RewardBagHues = new int[]
		{
		//	from,	to,
			0x385,	0x3E9,
			0x4B0,	0x4E6,
			0x514,	0x54A,
			0x578,	0x5AE,
			0x5DC,	0x612,
			0x640,	0x676,
			0x6A5,	0x6DA,
			0x708,	0x774
		};

		public static int RewardBagHue()
		{
			if ( Utility.RandomDouble() < 0.005 )
				return 0;

			int row = Utility.Random( m_RewardBagHues.Length / 2 ) * 2;

			return Utility.RandomMinMax( m_RewardBagHues[row], m_RewardBagHues[row + 1] );
		}

		public static int StrongboxHue()
		{
			return Utility.RandomMinMax( 0x898, 0x8B0 );
		}
		#endregion

		#region Random Loot
		public static Item RandomItem( int attributeCount, int min, int max )
		{
			Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry( false, true );

			if ( item is BaseWeapon )
				BaseRunicTool.ApplyAttributesTo( (BaseWeapon) item, attributeCount, min, max );
			else if ( item is BaseArmor )
				BaseRunicTool.ApplyAttributesTo( (BaseArmor) item, attributeCount, min, max );
			else if ( item is BaseJewel )
				BaseRunicTool.ApplyAttributesTo( (BaseJewel) item, attributeCount, min, max );

			return item;
		}

		public static Item Jewlery()
		{
			BaseJewel item = Loot.RandomJewelry();

			if ( item != null )
			{
				int attributeCount = Utility.RandomMinMax( 1, 5 );

				BaseRunicTool.ApplyAttributesTo( item, false, 0, attributeCount, 10, 100 ); // ?
			}

			return item;
		}

		public static Item RangedWeapon()
		{
			BaseWeapon item = Loot.RandomRangedWeapon( false, true );

			if ( item != null )
			{
				int attributeCount = Utility.RandomMinMax( 1, 5 );

				BaseRunicTool.ApplyAttributesTo( item, false, 0, attributeCount, 10, 100 ); // ?
			}

			return item;
		}

		public static Item Armor()
		{
			BaseArmor item = Loot.RandomArmor( false, true );

			if ( item != null )
			{
				int attributeCount = Utility.RandomMinMax( 1, 5 );

				BaseRunicTool.ApplyAttributesTo( item, false, 0, attributeCount, 10, 100 ); // ?
			}

			return item;
		}

		public static Item Weapon()
		{
			BaseWeapon item = Loot.RandomWeapon( false, true );

			if ( item != null )
			{
				int attributeCount = Utility.RandomMinMax( 1, 5 );

				BaseRunicTool.ApplyAttributesTo( item, false, 0, attributeCount, 10, 100 ); // ?
			}

			return item;
		}
		#endregion

		#region Recipes
		public static Item RandomRecipe()
		{
			switch ( Utility.Random( 7 ) )
			{
				case 0:
					return FletcherRecipe();
				case 1:
					return TailorRecipe();
				case 2:
				case 3:
					return SmithRecipe();
				case 4:
					return TinkerRecipe();
				default:
				case 5:
				case 6:
					return CarpRecipe();
			}
		}

		public static Item FletcherRecipe()
		{
			return GetRecipe( Enum.GetValues( typeof( BowRecipe ) ), Enum.GetValues( typeof( BowRecipeGreater ) ) );
		}

		public static Item TailorRecipe()
		{
			return GetRecipe( Enum.GetValues( typeof( TailorRecipe ) ), Enum.GetValues( typeof( TailorRecipeGreater ) ) );
		}

		public static Item SmithRecipe()
		{
			return GetRecipe( Enum.GetValues( typeof( SmithRecipe ) ), Enum.GetValues( typeof( SmithRecipeGreater ) ) );
		}

		public static Item TinkerRecipe()
		{
			return GetRecipe( Enum.GetValues( typeof( TinkerRecipe ) ), Enum.GetValues( typeof( TinkerRecipeGreater ) ) );
		}

		public static Item CarpRecipe()
		{
			return GetRecipe( Enum.GetValues( typeof( CarpRecipe ) ), Enum.GetValues( typeof( CarpRecipeGreater ) ) );
		}

		public static RecipeScroll GetRecipe( Array lessers, Array greaters )
		{
			Array selected = Utility.RandomDouble() < 0.01 ? greaters : lessers;

			int[] recipes = new int[selected.Length];

			int index = 0;

			foreach ( int i in selected )
				recipes[index++] = i;

			return new RecipeScroll( recipes[Utility.Random( selected.Length )] );
		}
		#endregion
	}

	public class LoyaltyReward : BaseReward
	{
		private LoyaltyGroup m_LoyaltyGroup;

		public LoyaltyReward( LoyaltyGroup loyaltyGroup, int amount )
			: base( null, amount, 1049594 ) // Loyalty Rating
		{
			m_LoyaltyGroup = loyaltyGroup;
		}

		public override void GiveReward()
		{
			LoyaltyGroupInfo loyaltyGroupInfo = LoyaltyGroupInfo.GetInfo( m_LoyaltyGroup );

			Quest.Owner.LoyaltyInfo.Award( m_LoyaltyGroup, Amount );

			// Your loyalty to ~1_GROUP~ has increased by ~2_AMOUNT~
			Quest.Owner.SendLocalizedMessage( 1115920, String.Format( "#{0}\t{1}", loyaltyGroupInfo.GroupName.ToString(), Amount.ToString() ), 0x21 );
		}
	}
}