using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class UseBestWeaponSkill : WeaponAttrInfo
	{
		// Use Best Weapon Skill
		public override int Name { get { return 1079592; } }

		// Uses the player's highest weapon skill to attack with the weapon, instead of the specific skill needed for the weapon.
		public override int Description { get { return 1111946; } }

		// Combat
		public override int Category { get { return 1114249; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.UseBestSkill; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Weapon; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.DelicateScales; } }
	}

	public class HitDispel : HitSpellAttrInfo
	{
		// Hit Dispel
		public override int Name { get { return 1079702; } }

		// Percentage rate of cast for Dispel (if the target is a summoned creature).
		public override int Description { get { return 1111959; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitDispel; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SlithTongue; } }
	}

	public class HitColdArea : HitAreaAttrInfo
	{
		// Hit Cold Area
		public override int Name { get { return 1079693; } }

		// Percentage chance to do an area-effect Cold attack on a successful strike.
		public override int Description { get { return 1111953; } }

		// Hit Area Effects
		public override int Category { get { return 1114250; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitColdArea; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Sapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.RaptorTeeth; } }
	}

	public class HitFireArea : HitAreaAttrInfo
	{
		// Hit Fire Area
		public override int Name { get { return 1079695; } }

		// Percentage chance to do an area-effect Fire attack on a successful strike.
		public override int Description { get { return 1111955; } }

		// Hit Area Effects
		public override int Category { get { return 1114250; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitFireArea; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.RaptorTeeth; } }
	}

	public class HitEnergyArea : HitAreaAttrInfo
	{
		// Hit Energy Area
		public override int Name { get { return 1079694; } }

		// Percentage chance to do an area-effect Energy attack on a successful strike.
		public override int Description { get { return 1111954; } }

		// Hit Area Effects
		public override int Category { get { return 1114250; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitEnergyArea; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amethyst; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.RaptorTeeth; } }
	}

	public class HitPhysicalArea : HitAreaAttrInfo
	{
		// Hit Physical Area
		public override int Name { get { return 1079696; } }

		// Percentage chance to do an area-effect Physical attack on a successful strike.
		public override int Description { get { return 1111956; } }

		// Hit Area Effects
		public override int Category { get { return 1114250; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitPhysicalArea; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.RaptorTeeth; } }
	}

	public class HitPoisonArea : HitAreaAttrInfo
	{
		// Hit Poison Area
		public override int Name { get { return 1079697; } }

		// Percentage chance to do an area-effect Poison attack on a successful strike.
		public override int Description { get { return 1111957; } }

		// Hit Area Effects
		public override int Category { get { return 1114250; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitPoisonArea; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.RaptorTeeth; } }
	}

	public class HitHarm : HitSpellAttrInfo
	{
		// Hit Harm
		public override int Name { get { return 1079704; } }

		// Percentage rate of cast for Harm.
		public override int Description { get { return 1111961; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitHarm; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ParasiticPlant; } }
	}

	public class HitFireball : HitSpellAttrInfo
	{
		// Hit Fireball
		public override int Name { get { return 1079703; } }

		// Percentage rate of cast for Fireball.
		public override int Description { get { return 1111960; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitFireball; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.FireRuby; } }
	}

	public class HitLightning : HitSpellAttrInfo
	{
		// Hit Lightning
		public override int Name { get { return 1079705; } }

		// Percentage rate of cast for Lightning.
		public override int Description { get { return 1111962; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLightning; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amethyst; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfPassion; } }
	}

	public class HitMagicArrow : HitSpellAttrInfo
	{
		// Hit Magic Arrow
		public override int Name { get { return 1079706; } }

		// Percentage rate of cast for Magic Arrow.
		public override int Description { get { return 1111963; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitMagicArrow; } }
		public override double Weight { get { return 1.2; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfFeeling; } }
	}

	public class HitLowerAttack : WeaponAttrInfo
	{
		// Hit Lower Attack
		public override int Name { get { return 1079699; } }

		// A successful hit with the weapon lowers the attack rating of the target by 25% for 10 seconds.
		public override int Description { get { return 1111965; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLowerAttack; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ParasiticPlant; } }
	}

	public class HitLowerDefense : WeaponAttrInfo
	{
		// Hit Lower Defense
		public override int Name { get { return 1079700; } }

		// A successful hit with the weapon lowers the defense rating of the target by 25% for 8 seconds against all attackers.
		public override int Description { get { return 1111966; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLowerDefend; } }
		public override double Weight { get { return 1.3; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ParasiticPlant; } }
	}

	public class HitManaLeech : WeaponAttrInfo
	{
		// Hit Mana Leech
		public override int Name { get { return 1079701; } }

		// Restores up to 40% of the displayed percentage of the damage dealt as Mana to the weapon's wielder.
		public override int Description { get { return 1111967; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechMana; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Sapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.VoidOrb; } }

		public override int Modify( Item item, int value )
		{
			BaseWeapon weapon = item as BaseWeapon;

			if ( weapon != null )
			{
				value = (int) ( ( value / 2 ) * weapon.GetSpeed() * 0.25 );

				if ( value > 100 )
					value = 100;

				if ( weapon is BaseRanged )
					value /= 2;
			}

			return value;
		}
	}

	public class HitLifeLeech : WeaponAttrInfo
	{
		// Hit Life Leech
		public override int Name { get { return 1079698; } }

		// Restores up to 30% of the displayed percentage of the damage dealt as Hit Points to the weapon's wielder.
		public override int Description { get { return 1111964; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechHits; } }
		public override double Weight { get { return 1.1; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.VoidOrb; } }

		public override int Modify( Item item, int value )
		{
			BaseWeapon weapon = item as BaseWeapon;

			if ( weapon != null )
			{
				value = (int) ( ( value / 2 ) * weapon.GetSpeed() * 0.25 );

				if ( value > 100 )
					value = 100;

				if ( weapon is BaseRanged )
					value /= 2;
			}

			return value;
		}
	}

	public class HitStaminaLeech : WeaponAttrInfo
	{
		// Hit Stamina Leech
		public override int Name { get { return 1079707; } }

		// Restores 100% of the damage dealt as Stamina to the weapon's wielder.
		public override int Description { get { return 1111992; } }

		// Hit Effects
		public override int Category { get { return 1114251; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.HitLeechStam; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 2; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 2; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.VoidOrb; } }
	}

	public class MageWeapon : WeaponAttrInfo
	{
		// Mage Weapon
		public override int Name { get { return 1079759; } }

		// Uses the wielder's Magery skill as a combat skill for the weapon. However, the wielder's Magery skill is lowered while wielding such a weapon.
		public override int Description { get { return 1112001; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.MageWeapon; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.MinusValue; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 10; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ArcanicRuneStone; } }

		public override int Modify( Item item, int value )
		{
			return 30 - value;
		}
	}

	public class Balanced : WeaponAttrInfo
	{
		// Balanced
		public override int Name { get { return 1072792; } }

		// Allows the wearer to drink potions or throw darts or shurikens while a missile weapon is equipped.
		public override int Description { get { return 1112047; } }

		// Ranged
		public override int Category { get { return 1114253; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.Balanced; } }
		public override double Weight { get { return 1.5; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Ranged; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amber; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfBalance; } }
	}

	public class Velocity : WeaponAttrInfo
	{
		// Velocity
		public override int Name { get { return 1080416; } }

		// Adds a bonus to damage based on the distance between the archer or bladeweaver and his target.
		public override int Description { get { return 1112048; } }

		// Ranged
		public override int Category { get { return 1114253; } }

		public override WeaponAttribute Attribute { get { return WeaponAttribute.Velocity; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Ranged; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 50; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Tourmaline; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.EssenceOfDirection; } }
	}
}