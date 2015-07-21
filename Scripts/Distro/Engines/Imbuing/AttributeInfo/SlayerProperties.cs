using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class AirElementalSlayer : SlayerPropInfo
	{
		// Air Elemental Slayer
		public override int Name { get { return 1079733; } }

		// Inflicts triple damage versus air elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111968; } }

		public override SlayerName Slayer { get { return SlayerName.AirElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class ArachnidSlayer : SlayerPropInfo
	{
		// Arachnid Slayer
		public override int Name { get { return 1079747; } }

		// Inflicts double damage versus arachnids. Beware - this item property will make you more vulnerable to damage from reptiles.
		public override int Description { get { return 1111983; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Arachnid; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.SpiderCarapace; } }
	}

	public class DemonSlayer : SlayerPropInfo
	{
		// Demon Slayer
		public override int Name { get { return 1079748; } }

		// Inflicts double damage versus demons. Beware - this item property will make you more vulnerable to damage from elementals and fey creatures.
		public override int Description { get { return 1111984; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Demon; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.DaemonClaw; } }
	}

	public class BloodElementalSlayer : SlayerPropInfo
	{
		// Blood Elemental Slayer
		public override int Name { get { return 1079734; } }

		// Inflicts triple damage versus blood elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111969; } }

		public override SlayerName Slayer { get { return SlayerName.BloodElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class EarthElementalSlayer : SlayerPropInfo
	{
		// Earth Elemental Slayer
		public override int Name { get { return 1079735; } }

		// Inflicts triple damage versus earth elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111971; } }

		public override SlayerName Slayer { get { return SlayerName.EarthElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class DragonSlayer : SlayerPropInfo
	{
		// Dragon Slayer
		public override int Name { get { return 1061284; } }

		// Inflicts triple damage versus dragons. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111970; } }

		public override SlayerName Slayer { get { return SlayerName.Dragon; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class FireElementalSlayer : SlayerPropInfo
	{
		// Fire Elemental Slayer
		public override int Name { get { return 1079736; } }

		// Inflicts triple damage versus fire elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111972; } }

		public override SlayerName Slayer { get { return SlayerName.FireElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class ElementalSlayer : SlayerPropInfo
	{
		// Elemental Slayer
		public override int Name { get { return 1079749; } }

		// Inflicts double damage versus elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111985; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Elemental; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.VialOfVitriol; } }
	}

	public class LizardmanSlayer : SlayerPropInfo
	{
		// Lizardman Slayer
		public override int Name { get { return 1079738; } }

		// Inflicts triple damage versus lizardmen. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111974; } }

		public override SlayerName Slayer { get { return SlayerName.Lizardman; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class GargoyleSlayer : SlayerPropInfo
	{
		// Gargoyle Slayer
		public override int Name { get { return 1079737; } }

		// Inflicts triple damage versus gargoyles. Beware - this item property will make you more vulnerable to damage from elementals.
		public override int Description { get { return 1111973; } }

		public override SlayerName Slayer { get { return SlayerName.Gargoyle; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class OphidianSlayer : SlayerPropInfo
	{
		// Ophidian Slayer
		public override int Name { get { return 1079740; } }

		// Inflicts triple damage versus ophidians. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111976; } }

		public override SlayerName Slayer { get { return SlayerName.Ophidian; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class OgreSlayer : SlayerPropInfo
	{
		// Ogre Slayer
		public override int Name { get { return 1079739; } }

		// Inflicts triple damage versus ogres. Beware - this item property will make you more vulnerable to damage from undead.
		public override int Description { get { return 1111975; } }

		public override SlayerName Slayer { get { return SlayerName.Ogre; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class PoisonElementalSlayer : SlayerPropInfo
	{
		// Poison Elemental Slayer
		public override int Name { get { return 1079742; } }

		// Inflicts triple damage versus poison elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111978; } }

		public override SlayerName Slayer { get { return SlayerName.PoisonElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class OrcSlayer : SlayerPropInfo
	{
		// Orc Slayer
		public override int Name { get { return 1079741; } }

		// Inflicts triple damage versus orcs. Beware - this item property will make you more vulnerable to damage from undead.
		public override int Description { get { return 1111977; } }

		public override SlayerName Slayer { get { return SlayerName.Orc; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class ReptileSlayer : SlayerPropInfo
	{
		// Reptile Slayer
		public override int Name { get { return 1079751; } }

		// Inflicts double damage versus reptiles. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111987; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Reptile; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.LavaSerpentCrust; } }
	}

	public class RepondSlayer : SlayerPropInfo
	{
		// Repond Slayer
		public override int Name { get { return 1079750; } }

		// Inflicts double damage versus repond creatures. Beware - this item property will make you more vulnerable to damage from undead.
		public override int Description { get { return 1111986; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Repond; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.GoblinBlood; } }
	}

	public class SnakeSlayer : SlayerPropInfo
	{
		// Snake Slayer
		public override int Name { get { return 1079744; } }

		// Inflicts triple damage versus snakes. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111980; } }

		public override SlayerName Slayer { get { return SlayerName.Snake; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class ScorpionSlayer : SlayerPropInfo
	{
		// Scorpion Slayer
		public override int Name { get { return 1079743; } }

		// Inflicts triple damage versus scorpions. Beware - this item property will make you more vulnerable to damage from reptiles.
		public override int Description { get { return 1111979; } }

		public override SlayerName Slayer { get { return SlayerName.Scorpion; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class SpiderSlayer : SlayerPropInfo
	{
		// Spider Slayer
		public override int Name { get { return 1079746; } }

		// Inflicts triple damage versus spiders. Beware - this item property will make you more vulnerable to damage from reptiles.
		public override int Description { get { return 1111982; } }

		public override SlayerName Slayer { get { return SlayerName.Spider; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class SnowElementalSlayer : SlayerPropInfo
	{
		// Snow Elemental Slayer
		public override int Name { get { return 1079745; } }

		// Inflicts triple damage versus snow elementals. Beware - this item property will make you more vulnerable to damage from demons.
		public override int Description { get { return 1111981; } }

		public override SlayerName Slayer { get { return SlayerName.SnowElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class TrollSlayer : SlayerPropInfo
	{
		// Troll Slayer
		public override int Name { get { return 1079754; } }

		// Inflicts triple damage versus trolls. Beware - this item property will make you more vulnerable to damage from undead.
		public override int Description { get { return 1111990; } }

		public override SlayerName Slayer { get { return SlayerName.Troll; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class TerathanSlayer : SlayerPropInfo
	{
		// Terathan Slayer
		public override int Name { get { return 1079753; } }

		// Inflicts triple damage versus terathans. Beware - this item property will make you more vulnerable to damage from reptiles.
		public override int Description { get { return 1111989; } }

		public override SlayerName Slayer { get { return SlayerName.Terathan; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class WaterElementalSlayer : SlayerPropInfo
	{
		// Water Elemental Slayer
		public override int Name { get { return 1079755; } }

		// Inflicts triple damage versus water elementals. Beware - this item property will make you more vulnerable to damage from arachnids.
		public override int Description { get { return 1111991; } }

		public override SlayerName Slayer { get { return SlayerName.WaterElemental; } }
		public override double Weight { get { return 1.1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.WhitePearl; } }
	}

	public class UndeadSlayer : SlayerPropInfo
	{
		// Undead Slayer
		public override int Name { get { return 1079752; } }

		// Inflicts double damage versus undead. Beware - this item property will make you more vulnerable to damage from repond creatures.
		public override int Description { get { return 1111988; } }

		public override int Category { get { return 1114264; } } // Super Slayers

		public override SlayerName Slayer { get { return SlayerName.Undead; } }
		public override double Weight { get { return 1.3; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.RelicFragment; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.UndyingFlesh; } }
	}
}