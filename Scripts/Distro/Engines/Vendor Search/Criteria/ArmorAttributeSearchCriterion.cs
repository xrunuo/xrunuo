using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class ArmorAttributeSearchCriterion : ValuedSearchCriterion
	{
		public abstract AosArmorAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IArmor && ( (IArmor) item.Item ).ArmorAttributes[Attribute] >= Value;
		}
	}

	public abstract class BooleanArmorAttributeSearchCriterion : SearchCriterion
	{
		public abstract AosArmorAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IArmor && ( (IArmor) item.Item ).ArmorAttributes[Attribute] != 0;
		}
	}

	public class LowerRequirementsSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060435; } } // lower requirements ~1_val~%
		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.LowerStatReq; } }
	}

	public class SelfRepairSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060450; } } // self repair ~1_val~
		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.SelfRepair; } }
	}

	public class SoulChargeSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113630; } } // Soul Charge ~1_val~%
		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.SoulCharge; } }
	}

	public class ReactiveParalyzeSearchCriterion : BooleanArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1154660; } } // Reactive Paralyze
		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.ReactiveParalyze; } }
	}

	public class MageArmorSearchCriterion : BooleanArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1079758; } } // Mage Armor
		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.MageArmor; } }
	}
}
