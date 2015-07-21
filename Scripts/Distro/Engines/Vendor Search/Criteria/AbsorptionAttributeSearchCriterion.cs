using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class AbsorptionAttributeSearchCriterion : ValuedSearchCriterion
	{
		public abstract AbsorptionAttribute Attribute { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IAbsorption && ( (IAbsorption) item.Item ).AbsorptionAttributes[Attribute] >= Value;
		}
	}

	public class FireEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113593; } } // Fire Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.FireEater; } }
	}

	public class ColdEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113594; } } // Cold Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.ColdEater; } }
	}

	public class PoisonEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113595; } } // Poison Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.PoisonEater; } }
	}

	public class EnergyEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113596; } } // Energy Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.EnergyEater; } }
	}

	public class KineticEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113597; } } // Kinetic Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.KineticEater; } }
	}

	public class DamageEaterSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113598; } } // Damage Eater ~1_Val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.DamageEater; } }
	}

	public class FireResonanceSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113691; } } // Fire Resonance ~1_val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.FireResonance; } }
	}

	public class ColdResonanceSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113692; } } // Cold Resonance ~1_val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.ColdResonance; } }
	}

	public class PoisonResonanceSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113693; } } // Poison Resonance ~1_val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.PoisonResonance; } }
	}

	public class EnergyResonanceSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113694; } } // Energy Resonance ~1_val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.EnergyResonance; } }
	}

	public class KineticResonanceSearchCriterion : AbsorptionAttributeSearchCriterion
	{
		public override int LabelNumber { get { return 1113695; } } // Kinetic Resonance ~1_val~%
		public override AbsorptionAttribute Attribute { get { return AbsorptionAttribute.KineticResonance; } }
	}
}
