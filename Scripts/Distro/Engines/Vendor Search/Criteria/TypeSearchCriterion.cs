using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class TypeSearchCriterion : SearchCriterion
	{
		public abstract Type ItemType { get; }

		public override sealed bool Matches( IVendorSearchItem item )
		{
			return ItemType.IsAssignableFrom( item.Item.GetType() );
		}
	}
}
