using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.VendorSearch
{
	public abstract class RequiredRaceSearchCriterion : SearchCriterion
	{
		public abstract Race RequiredRace { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			return item.Item.RequiredRace == RequiredRace;
		}

		public override string ReplacementKey
		{
			get { return typeof( RequiredRaceSearchCriterion ).FullName; }
		}
	}

	public class GargoylesOnlySearchCriterion : RequiredRaceSearchCriterion
	{
		public override int LabelNumber { get { return 1154648; } } // Gargoyles Only
		public override Race RequiredRace { get { return Race.Gargoyle; } }
	}

	public class ElvesOnlySearchCriterion : RequiredRaceSearchCriterion
	{
		public override int LabelNumber { get { return 1154650; } } // Elves Only
		public override Race RequiredRace { get { return Race.Elf; } }
	}
}
