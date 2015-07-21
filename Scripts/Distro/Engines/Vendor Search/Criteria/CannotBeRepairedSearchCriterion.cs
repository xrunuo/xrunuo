using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public class CannotBeRepairedSearchCriterion : SearchCriterion
	{
		public override int LabelNumber { get { return 1151826; } } // Cannot Be Repaired

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IDurability && ( (IDurability) item.Item ).CannotBeRepaired;
		}
	}
}
