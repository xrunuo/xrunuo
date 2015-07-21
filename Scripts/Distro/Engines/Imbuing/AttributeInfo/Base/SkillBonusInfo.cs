using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the Skill Bonus properties from <see cref="SkillName" /> enum.
	/// </summary>
	public class SkillBonusInfo : BaseAttrInfo
	{
		private SkillName m_Skill;
		private int m_Description;
		private int m_Category;

		public override int Name { get { return 1044060 + (int) Skill; } }
		public override int Description { get { return m_Description; } }
		public SkillName Skill { get { return m_Skill; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.PlusValue; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }
		public override int Category { get { return 1114254 + m_Category; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.StarSapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.CrystallineBlackrock; } }

		public SkillBonusInfo( SkillName skill, int description, int category )
		{
			m_Skill = skill;
			m_Description = description;
			m_Category = category;
		}

		public override bool CanHold( Item item )
		{
			return item is ISkillBonuses;
		}

		public override bool Replaces( BaseAttrInfo otherAttr )
		{
			if ( otherAttr is SkillBonusInfo )
			{
				SkillBonusInfo skillAttr = otherAttr as SkillBonusInfo;

				if ( m_Skill == skillAttr.Skill || Category == skillAttr.Category )
					return true;
			}

			return false;
		}

		public override int GetValue( Item item )
		{
			ISkillBonuses sb = item as ISkillBonuses;

			for ( int i = 0; i < 5; ++i )
			{
				SkillName skill;
				double bonus;

				if ( !sb.SkillBonuses.GetValues( i, out skill, out bonus ) )
					continue;

				if ( Skill == skill )
					return (int) bonus;
			}

			return 0;
		}

		public override void SetValue( Item item, int value )
		{
			ISkillBonuses sb = item as ISkillBonuses;
			int firstValid = -1;

			for ( int i = 0; i < 5; ++i )
			{
				SkillName skill;
				double bonus;

				if ( !sb.SkillBonuses.GetValues( i, out skill, out bonus ) )
				{
					if ( firstValid == -1 )
						firstValid = i;

					continue;
				}

				if ( Skill == skill )
				{
					sb.SkillBonuses.SetValues( i, Skill, value );
					return;
				}
			}

			if ( firstValid != -1 )
			{
				sb.SkillBonuses.SetValues( firstValid, Skill, value );
				return;
			}
		}

		public override string ToString()
		{
			return m_Skill.ToString();
		}
	}
}