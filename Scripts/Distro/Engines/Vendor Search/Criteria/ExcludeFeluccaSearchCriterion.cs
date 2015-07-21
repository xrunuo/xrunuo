using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public class ExcludeFeluccaSearchCriterion : SearchCriterion
	{
		public override int LabelNumber { get { return 1154646; } } // Exclude items on Felucca

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Vendor.Map != Map.Felucca;
		}
	}
}
