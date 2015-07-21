using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Loyalty
{
	public enum LoyaltyGroup
	{
		GargoyleQueen
	}

	public class LoyaltyGroupInfo
	{
		public delegate bool LoyaltyChecker( PlayerMobile killer, BaseCreature victim );

		public class TierInfo
		{
			private int m_MinPoints;
			private int m_Cliloc;

			public int MinPoints { get { return m_MinPoints; } }
			public int Cliloc { get { return m_Cliloc; } }

			public TierInfo( int minPoints, int cliloc )
			{
				m_MinPoints = minPoints;
				m_Cliloc = cliloc;
			}
		}

		private int m_Name;
		private int m_GroupName;
		private int m_MaxPoints;
		private LoyaltyChecker m_Checker;
		private TierInfo[] m_Tiers;

		public int Name { get { return m_Name; } }
		public int GroupName { get { return m_GroupName; } }
		public int MinPoints { get { return m_Tiers[0].MinPoints; } }
		public int MaxPoints { get { return m_MaxPoints; } }
		public LoyaltyChecker Checker { get { return m_Checker; } }
		public TierInfo[] Tiers { get { return m_Tiers; } }

		public LoyaltyGroupInfo( int name, int groupName, int maxPoints, params TierInfo[] tiers )
			: this( name, groupName, maxPoints, null, tiers )
		{
		}

		public LoyaltyGroupInfo( int name, int groupName, int maxPoints, LoyaltyChecker checker, params TierInfo[] tiers )
		{
			m_Name = name;
			m_GroupName = groupName;
			m_MaxPoints = maxPoints;
			m_Checker = checker;
			m_Tiers = tiers;
		}

		public static LoyaltyGroupInfo[] Table { get { return m_Table; } }

		private static readonly LoyaltyGroupInfo[] m_Table = new LoyaltyGroupInfo[]
			{
				new LoyaltyGroupInfo( 1095163, // Gargoyle Queen
					1115922, // the Gargoyle Queen
					15000,
					(k, v) => v.Map == Map.TerMur,
					new TierInfo( -15000, 1095164 ), // Enemy
					new TierInfo(      0, 1095166 ), // Friend
					new TierInfo(   2000, 1095165 ), // Citizen
					new TierInfo(  10000, 1095167 )  // Noble
					)
			};

		public static LoyaltyGroupInfo GetInfo( LoyaltyGroup type )
		{
			int v = (int) type;

			if ( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}

		public int GetCliloc( int points )
		{
			for ( int i = m_Tiers.Length - 1; i > 0; i-- )
			{
				TierInfo tier = m_Tiers[i];

				if ( points >= tier.MinPoints )
					return tier.Cliloc;
			}

			return m_Tiers[0].Cliloc;
		}
	}
}