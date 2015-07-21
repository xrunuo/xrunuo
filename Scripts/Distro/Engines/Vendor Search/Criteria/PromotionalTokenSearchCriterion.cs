using System;
using Server;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public class PromotionalTokenSearchCriterion : TypeSearchCriterion
	{
		public override Type ItemType { get { return typeof( PromotionalToken ); } }
		public override int LabelNumber { get { return 1154682; } } // Promotional Token
	}
}
