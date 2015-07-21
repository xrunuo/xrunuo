using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public class ArtifactRaritySearchCriterion : ValuedSearchCriterion
	{
		public override int LabelNumber { get { return 1061078; } } // artifact rarity ~1_val~

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item is IArtifactRarity && ( (IArtifactRarity) item.Item ).ArtifactRarity == Value;
		}
	}
}
