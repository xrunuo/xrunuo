using System;
using Server;

namespace Server.Items
{
	interface IDurability
	{
		bool CanLoseDurability { get; }

		int InitMinHits { get; }
		int InitMaxHits { get; }

		int HitPoints { get; set; }
		int MaxHitPoints { get; set; }

		bool Brittle { get; }
		bool CannotBeRepaired { get; }

		// Maybe a scale/unscale durability?
	}

	interface IWearableDurability : IDurability
	{
		int OnHit( int damageTaken );
	}
}