using System;
using Server;
using Server.Factions;

namespace Server.Engines.VendorSearch
{
	public class FactionItemSearchCriterion : SearchCriterion
	{
		public override int LabelNumber { get { return 1154661; } } // Faction Item

		public override bool Matches( IVendorSearchItem item )
		{
			if ( item is IFactionArtifact )
				return true;

			return item is IFactionItem && ( (IFactionItem) item ).FactionItemState != null;
		}
	}
}
