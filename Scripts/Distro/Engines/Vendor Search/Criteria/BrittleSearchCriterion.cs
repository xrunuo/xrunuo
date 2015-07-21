using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public class BrittleSearchCriterion : SearchCriterion
	{
		public override int LabelNumber { get { return 1116209; } } // Brittle

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IDurability && ( (IDurability) item.Item ).Brittle;
		}
	}
}
