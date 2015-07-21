using System;

namespace Server.Items
{
	public enum WeaponType
	{
		Axe,		// Axes, Hatches, etc. These can give concussion blows
		Slashing,	// Katana, Broadsword, Longsword, etc. Slashing weapons are poisonable
		Staff,		// Staves
		Bashing,	// War Hammers, Maces, Mauls, etc. Two-handed bashing delivers crushing blows
		Piercing,	// Spears, Warforks, Daggers, etc. Two-handed piercing delivers paralyzing blows
		Polearm,	// Halberd, Bardiche
		Ranged,		// Bow, Crossbows
		Fists		// Fists
	}

	public enum WeaponAnimation
	{
		Wrestle,
		ShootBow,
		ShootXBow,
		Bash1H,
		Slash1H,
		Pierce1H,
		Bash2H,
		Slash2H,
		Pierce2H
	}
}