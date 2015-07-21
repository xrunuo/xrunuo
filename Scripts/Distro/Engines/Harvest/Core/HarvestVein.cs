using System;

namespace Server.Engines.Harvest
{
	public class HarvestVein
	{
		public double Chance { get; set; }
		public HarvestResource Resource { get; set; }

		public HarvestVein( double chance, HarvestResource resource )
		{
			Chance = chance;
			Resource = resource;
		}
	}
}