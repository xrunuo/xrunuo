using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class DamageIncrease : MagicalAttrInfo
	{
		// Damage Increase
		public override int Name { get { return 1079399; } }

		// Increases the maximum and minimum damage the wielder deals with wrestling, melee and ranged weapons.
		public override int Description { get { return 1112005; } }

		// Combat
		public override int Category { get { return 1114249; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.WeaponDamage; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Citrine; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.CrystalShards; } }

		public override bool Validate( Item item )
		{
			return item is IMagicalItem && !( item is BaseJewel );
		}
	}

	public class DamageIncreaseJewel : DamageIncrease
	{
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 25; } }
		public override double IntensityInterval { get { return 1; } }

		public override bool Validate( Item item )
		{
			return item is BaseJewel;
		}
	}

	public class SwingSpeedIncrease : MagicalAttrInfo
	{
		// Swing Speed Increase
		public override int Name { get { return 1075629; } }

		// Increases the weapon's swing rate by a percentage, allowing the wielder to swing more frequently in combat.
		public override int Description { get { return 1112045; } }

		// Combat
		public override int Category { get { return 1114249; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.WeaponSpeed; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 5; } }
		public override double MaxIntensity { get { return 30; } }
		public override double IntensityInterval { get { return 5; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfControl; } }
	}

	public class Luck : MagicalAttrInfo
	{
		// Luck
		public override int Name { get { return 1061153; } }

		// Raises the wearer's Luck statistic, which increases the potency and number of magical properties associated with loot on monsters the player kills.
		public override int Description { get { return 1112051; } }

		// Misc.
		public override int Category { get { return 1114252; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.Luck; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Weapon | ImbuingFlag.Jewelry | ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 100; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Citrine; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ChagaMushroom; } }

		public override bool Validate( Item item )
		{
			return item is IMagicalItem && !( item is BaseRanged ) && !( item is BaseThrowing );
		}
	}

	public class LuckRanged : Luck
	{
		public override ImbuingFlag Flags { get { return ImbuingFlag.Ranged | ImbuingFlag.Throwing; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 120; } }
		public override double IntensityInterval { get { return 1; } }

		public override bool Validate( Item item )
		{
			return item is BaseRanged || item is BaseThrowing;
		}
	}

	public class HitChanceIncrease : MagicalAttrInfo
	{
		// Hit Chance Increase
		public override int Name { get { return 1075616; } }

		// Increases the player's chance to hit a target with wrestling, melee and ranged weapons.
		public override int Description { get { return 1111958; } }

		// Combat
		public override int Category { get { return 1114249; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.AttackChance; } }
		public override double Weight { get { return 1.3; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Weapon | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfPrecision; } }

		public override bool Validate( Item item )
		{
			return item is IMagicalItem && !( item is BaseRanged ) && !( item is BaseThrowing );
		}
	}

	public class HitChanceIncreaseRanged : HitChanceIncrease
	{
		// Ranged
		public override int Category { get { return 1114253; } }

		public override ImbuingFlag Flags { get { return ImbuingFlag.Ranged | ImbuingFlag.Throwing; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 25; } }
		public override double IntensityInterval { get { return 1; } }

		public override bool Validate( Item item )
		{
			return item is BaseRanged || item is BaseThrowing;
		}
	}

	public class DefenseChanceIncrease : MagicalAttrInfo
	{
		// Defense Chance Increase
		public override int Name { get { return 1075620; } }

		// Increases the wearer's chance that his opponents' swings (or arrows/bolts) will miss.
		public override int Description { get { return 1111947; } }

		// Combat
		public override int Category { get { return 1114249; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.DefendChance; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Jewelry | ImbuingFlag.Shield; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfSingularity; } }

		public override bool Validate( Item item )
		{
			return item is IMagicalItem && !( item is BaseRanged ) && !( item is BaseThrowing );
		}
	}

	public class DefenseChanceIncreaseRanged : DefenseChanceIncrease
	{
		// Ranged
		public override int Category { get { return 1114253; } }

		public override ImbuingFlag Flags { get { return ImbuingFlag.Ranged | ImbuingFlag.Throwing; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 25; } }
		public override double IntensityInterval { get { return 1; } }

		public override bool Validate( Item item )
		{
			return item is BaseRanged || item is BaseThrowing;
		}
	}

	public class FasterCasting : MagicalAttrInfo
	{
		// Faster Casting
		public override int Name { get { return 1075617; } }

		// Lowers the amount of time required to cast a spell.
		public override int Description { get { return 1111951; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.CastSpeed; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Jewelry | ImbuingFlag.Shield; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfAchievement; } }
	}

	public class FasterCastRecovery : MagicalAttrInfo
	{
		// Faster Cast Recovery
		public override int Name { get { return 1075618; } }

		// Lowers the amount of time to recover after casting a spell.
		public override int Description { get { return 1111952; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.CastRecovery; } }
		public override double Weight { get { return 1.2; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 3; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amethyst; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfDiligence; } }
	}

	public class SpellChanneling : MagicalAttrInfo
	{
		// Spell Channeling
		public override int Name { get { return 1079766; } }

		// Allows a mage to wield a weapon while casting a spell, but increases the mage's spell casting time.
		public override int Description { get { return 1112040; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.SpellChanneling; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Shield; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SilverSnakeSkin; } }
	}

	public class LowerManaCost : MagicalAttrInfo
	{
		// Lower Mana Cost
		public override int Name { get { return 1075621; } }

		// Lowers the amount of Mana required to cast a spell by a percentage.
		public override int Description { get { return 1111996; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.LowerManaCost; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfOrder; } }
	}

	public class HitPointIncrease : MagicalAttrInfo
	{
		// Hit Point Increase
		public override int Name { get { return 1075630; } }

		// Increases a player's maximum Hit Points while the object is equipped.
		public override int Description { get { return 1111993; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusHits; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 5; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.LuminescentFungi; } }
	}

	public class ManaIncrease : MagicalAttrInfo
	{
		// Mana Increase
		public override int Name { get { return 1075631; } }

		// Increases the wearer's maximum Mana while the object is equipped.
		public override int Description { get { return 1112002; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusMana; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Sapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.LuminescentFungi; } }
	}

	public class StaminaIncrease : MagicalAttrInfo
	{
		// Stamina Increase
		public override int Name { get { return 1075632; } }

		// Increases a player's maximum Stamina while the object is equipped.
		public override int Description { get { return 1112042; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusStam; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.LuminescentFungi; } }
	}

	public class ReflectPhysicalDamage : MagicalAttrInfo
	{
		// Reflect Physical Damage
		public override int Name { get { return 1075626; } }

		// Reflects a portion of Physical Damage the player receives back at the source of the damage. Does not reduce damage taken by the player.
		public override int Description { get { return 1112006; } }

		// Misc.
		public override int Category { get { return 1114252; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.ReflectPhysical; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor | ImbuingFlag.Shield; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Citrine; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ReflectiveWolfEye; } }
	}

	public class NightSight : MagicalAttrInfo
	{
		// Night Sight
		public override int Name { get { return 1112187; } }

		// A permanent, non-charged, light source that raises the ambient light level for the wearer.
		public override int Description { get { return 1112004; } }

		// Misc.
		public override int Category { get { return 1114252; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.NightSight; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BottleOfIchor; } }
	}

	public class ManaRegeneration : MagicalAttrInfo
	{
		// Mana Regeneration
		public override int Name { get { return 1079410; } }

		// Increases the wearer's natural Mana regeneration rate.
		public override int Description { get { return 1112003; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.RegenMana; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 2; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Sapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SeedOfRenewal; } }
	}

	public class HitPointRegeneration : MagicalAttrInfo
	{
		// Hit Point Regeneration
		public override int Name { get { return 1075627; } }

		// Increases the wearer's natural Hit Point regeneration rate.
		public override int Description { get { return 1111994; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.RegenHits; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 2; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SeedOfRenewal; } }
	}

	public class StaminaRegeneration : MagicalAttrInfo
	{
		// Stamina Regeneration
		public override int Name { get { return 1079411; } }

		// Increases the wearer's natural Stamina regeneration rate.
		public override int Description { get { return 1112043; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.RegenStam; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 3; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SeedOfRenewal; } }
	}

	public class StrengthBonus : MagicalAttrInfo
	{
		// Strength Bonus
		public override int Name { get { return 1079767; } }

		// Increases a player's strength while the object is equipped.
		public override int Description { get { return 1112044; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusStr; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.FireRuby; } }
	}

	public class DexterityBonus : MagicalAttrInfo
	{
		// Dexterity Bonus
		public override int Name { get { return 1079732; } }

		// Increases a player's dexterity while the object is equipped.
		public override int Description { get { return 1111948; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusDex; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BlueDiamond; } }
	}

	public class IntelligenceBonus : MagicalAttrInfo
	{
		// Intelligence Bonus
		public override int Name { get { return 1079756; } }

		// Increases a player's intelligence while the object is equipped.
		public override int Description { get { return 1111995; } }

		// Stats
		public override int Category { get { return 1114262; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.BonusInt; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.Value; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 8; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.Turquoise; } }
	}

	public class EnhancePotions : MagicalAttrInfo
	{
		// Enhance Potions
		public override int Name { get { return 1075624; } }

		// Increases the effectiveness of potions used by the wearer by a percentage.
		public override int Description { get { return 1111950; } }

		// Misc.
		public override int Category { get { return 1114252; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.EnhancePotions; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 5; } }
		public override double MaxIntensity { get { return 25; } }
		public override double IntensityInterval { get { return 5; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Citrine; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.CrushedGlass; } }
	}

	public class LowerReagentCost : MagicalAttrInfo
	{
		// Lower Reagent Cost
		public override int Name { get { return 1075625; } }

		// Percentage rate that any spell cast by the wearer will not consume reagents.
		public override int Description { get { return 1111997; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.LowerRegCost; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry | ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 20; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.FaeryDust; } }
	}

	public class SpellDamageIncrease : MagicalAttrInfo
	{
		// Spell Damage Increase
		public override int Name { get { return 1075628; } }

		// Increases the damage of the item wearer's spells.
		public override int Description { get { return 1112041; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override MagicalAttribute Attribute { get { return MagicalAttribute.SpellDamage; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 12; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.CrystalShards; } }
	}
}