using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public abstract class RequiredSkillSearchCriterion : SearchCriterion
	{
		public abstract SkillName RequiredSkill { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			var weapon = item.Item as BaseWeapon;

			if ( weapon != null )
				return weapon.Skill == RequiredSkill;
			else
				return false;
		}

		public override string ReplacementKey
		{
			get { return typeof( RequiredSkillSearchCriterion ).FullName; }
		}
	}

	public class SwordsReqSkillSearchCriterion : RequiredSkillSearchCriterion
	{
		public override int LabelNumber { get { return 1002151; } } // Swordsmanship
		public override SkillName RequiredSkill { get { return SkillName.Swords; } }
	}

	public class MacingReqSkillSearchCriterion : RequiredSkillSearchCriterion
	{
		public override int LabelNumber { get { return 1044101; } } // Mace Fighting
		public override SkillName RequiredSkill { get { return SkillName.Macing; } }
	}

	public class FencingReqSkillSearchCriterion : RequiredSkillSearchCriterion
	{
		public override int LabelNumber { get { return 1044102; } } // Fencing
		public override SkillName RequiredSkill { get { return SkillName.Fencing; } }
	}

	public class ArcheryReqSkillSearchCriterion : RequiredSkillSearchCriterion
	{
		public override int LabelNumber { get { return 1002029; } } // Archery
		public override SkillName RequiredSkill { get { return SkillName.Archery; } }
	}

	public class ThrowingReqSkillSearchCriterion : RequiredSkillSearchCriterion
	{
		public override int LabelNumber { get { return 1044117; } } // Throwing
		public override SkillName RequiredSkill { get { return SkillName.Throwing; } }
	}
}
