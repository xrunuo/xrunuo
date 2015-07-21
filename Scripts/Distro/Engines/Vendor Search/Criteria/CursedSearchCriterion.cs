using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public class CursedSearchCriterion : SearchCriterion
	{
		public override int LabelNumber { get { return 1116639; } } // Cursed

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.LootType == LootType.Cursed;
		}
	}
}
