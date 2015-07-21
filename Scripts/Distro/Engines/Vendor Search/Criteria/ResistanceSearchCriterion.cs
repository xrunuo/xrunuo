using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public class PhysicalResistSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060448; } } // physical resist ~1_val~%

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.PhysicalResistance >= Value;
		}
	}

	public class FireResistSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060447; } } // fire resist ~1_val~%

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.FireResistance >= Value;
		}
	}

	public class ColdResistSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060445; } } // cold resist ~1_val~%

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.ColdResistance >= Value;
		}
	}

	public class PoisonResistSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060449; } } // poison resist ~1_val~%

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.PoisonResistance >= Value;
		}
	}

	public class EnergyResistSearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1060446; } } // energy resist ~1_val~%

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.EnergyResistance >= Value;
		}
	}
}
