using System;
using Server;
using Server.Items;

namespace Server.Factions
{
	interface IFactionArtifact// : IWearableDurability
	{
		Mobile Owner { get; set; }
	}
}
