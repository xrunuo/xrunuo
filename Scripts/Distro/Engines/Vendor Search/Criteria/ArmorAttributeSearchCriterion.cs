using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class ArmorAttributeSearchCriterion : ValuedSearchCriterion
	{
		public abstract ArmorAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IArmor && ( (IArmor) item.Item ).ArmorAttributes[Attribute] >= Value;
		}
	}

	public abstract class BooleanArmorAttributeSearchCriterion : SearchCriterion
	{
		public abstract ArmorAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IArmor && ( (IArmor) item.Item ).ArmorAttributes[Attribute] != 0;
		}
	}

	public class LowerRequirementsSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060435; } } // lower requirements ~1_val~%
		public override ArmorAttribute Attribute { get { return ArmorAttribute.LowerStatReq; } }
	}

	public class SelfRepairSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1060450; } } // self repair ~1_val~
		public override ArmorAttribute Attribute { get { return ArmorAttribute.SelfRepair; } }
	}

	public class SoulChargeSearchCriterion : ArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113630; } } // Soul Charge ~1_val~%
		public override ArmorAttribute Attribute { get { return ArmorAttribute.SoulCharge; } }
	}

	public class ReactiveParalyzeSearchCriterion : BooleanArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1154660; } } // Reactive Paralyze
		public override ArmorAttribute Attribute { get { return ArmorAttribute.ReactiveParalyze; } }
	}

	public class MageArmorSearchCriterion : BooleanArmorAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1079758; } } // Mage Armor
		public override ArmorAttribute Attribute { get { return ArmorAttribute.MageArmor; } }
	}
}
