using System;
using System.Collections.Generic;

namespace Server.Spells.Bard
{
	public class BardMastery
	{
		private static BardMastery[] m_Masteries = new BardMastery[]
			{
				new BardMastery( 0, SkillName.Discordance, 1151945 ),
				new BardMastery( 1, SkillName.Provocation, 1151946 ),
				new BardMastery( 2, SkillName.Peacemaking, 1151947 )
			};

		public static BardMastery Discordance { get { return m_Masteries[0]; } }
		public static BardMastery Provocation { get { return m_Masteries[1]; } }
		public static BardMastery Peacemaking { get { return m_Masteries[2]; } }

		public static IEnumerable<BardMastery> AllMasteries
		{
			get { return m_Masteries; }
		}

		private int m_Id;
		private SkillName m_Skill;
		private int m_Cliloc;

		public int Id { get { return m_Id; } }
		public SkillName Skill { get { return m_Skill; } }
		public int Cliloc { get { return m_Cliloc; } }

		public BardMastery( int id, SkillName skill, int cliloc )
		{
			m_Id = id;
			m_Skill = skill;
			m_Cliloc = cliloc;
		}

		public int Mask
		{
			get { return 1 << m_Id; }
		}

		public static BardMastery GetFromId( int id )
		{
			if ( id >= 0 && id < m_Masteries.Length )
				return m_Masteries[id];
			else
				return null;
		}
	}
}
