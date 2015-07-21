using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.VendorSearch
{
	public abstract class SlayerSearchCriterion : SearchCriterion
	{
		public abstract SlayerName Slayer { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			var slayer = item.Item as ISlayer;

			if ( slayer != null )
				return slayer.Slayer == Slayer || slayer.Slayer2 == Slayer;
			else
				return false;
		}
	}

	public abstract class TalismanSlayerSearchCriterion : SearchCriterion
	{
		public abstract TalisSlayerName Slayer { get; }

		public override bool Matches( IVendorSearchItem item )
		{
			var talisman = item.Item as BaseTalisman;

			if ( talisman != null )
				return talisman.TalisSlayer == Slayer;
			else
				return false;
		}
	}

	public class ReptileSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079751; } } // Reptile Slayer
		public override SlayerName Slayer { get { return SlayerName.Reptile; } }
	}

	public class DragonSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1061284; } } // Dragon Slayer
		public override SlayerName Slayer { get { return SlayerName.Dragon; } }
	}

	public class LizardmanSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079738; } } // Lizardman Slayer
		public override SlayerName Slayer { get { return SlayerName.Lizardman; } }
	}

	public class OphidianSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079740; } } // Ophidian Slayer
		public override SlayerName Slayer { get { return SlayerName.Ophidian; } }
	}

	public class SnakeSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079744; } } // Snake Slayer
		public override SlayerName Slayer { get { return SlayerName.Snake; } }
	}

	public class ArachnidSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079747; } } // Arachnid Slayer
		public override SlayerName Slayer { get { return SlayerName.Arachnid; } }
	}

	public class ScorpionSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079743; } } // Scorpion Slayer
		public override SlayerName Slayer { get { return SlayerName.Scorpion; } }
	}

	public class SpiderSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079746; } } // Spider Slayer
		public override SlayerName Slayer { get { return SlayerName.Spider; } }
	}

	public class TerathanSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079753; } } // Terathan Slayer
		public override SlayerName Slayer { get { return SlayerName.Terathan; } }
	}

	public class RepondSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079750; } } // Repond Slayer
		public override SlayerName Slayer { get { return SlayerName.Repond; } }
	}

	public class BatSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072506; } } // Bat Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Bat; } }
	}

	public class BearSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072504; } } // Bear Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Bear; } }
	}

	public class BeetleSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072508; } } // Beetle Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Beetle; } }
	}

	public class BirdSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072509; } } // Bird Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Bird; } }
	}

	public class BovineSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072512; } } // Bovine Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Bovine; } }
	}

	public class FlameSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072511; } } // Flame Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Flame; } }
	}

	public class GoblinSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1095010; } } // Goblin Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Goblin; } }
	}

	public class IceSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072510; } } // Ice Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Ice; } }
	}

	public class MageSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072507; } } // Mage Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Mage; } }
	}

	public class OgreSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079739; } } // Ogre Slayer
		public override SlayerName Slayer { get { return SlayerName.Ogre; } }
	}

	public class OrcSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079741; } } // Orc Slayer
		public override SlayerName Slayer { get { return SlayerName.Orc; } }
	}

	public class TrollSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079754; } } // Troll Slayer
		public override SlayerName Slayer { get { return SlayerName.Troll; } }
	}

	public class VerminSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1072505; } } // Vermin Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Vermin; } }
	}

	public class UndeadSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079752; } } // Undead Slayer
		public override SlayerName Slayer { get { return SlayerName.Undead; } }
	}

	public class WolfSlayerSearchCriterion : TalismanSlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1075462; } } // Wolf Slayer
		public override TalisSlayerName Slayer { get { return TalisSlayerName.Wolf; } }
	}

	public class DemonSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079748; } } // Demon Slayer
		public override SlayerName Slayer { get { return SlayerName.Demon; } }
	}

	public class GargoyleSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079737; } } // Gargoyle Slayer
		public override SlayerName Slayer { get { return SlayerName.Gargoyle; } }
	}

	public class FeySlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1154652; } } // Fey Slayer
		public override SlayerName Slayer { get { return SlayerName.Fey; } }
	}

	public class ElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079749; } } // Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.Elemental; } }
	}

	public class AirElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079733; } } // Air Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.AirElemental; } }
	}

	public class BloodElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079734; } } // Blood Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.BloodElemental; } }
	}

	public class EarthElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079735; } } // Earth Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.EarthElemental; } }
	}

	public class FireElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079736; } } // Fire Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.FireElemental; } }
	}

	public class PoisonElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079742; } } // Poison Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.PoisonElemental; } }
	}

	public class SnowElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override int LabelNumber { get { return 1079745; } } // Snow Elemental Slayer
		public override SlayerName Slayer { get { return SlayerName.SnowElemental; } }
	}

	public class WaterElementalSlayerSearchCriterion : SlayerSearchCriterion
	{
		public override SlayerName Slayer { get { return SlayerName.WaterElemental; } }
		public override int LabelNumber { get { return 1079755; } } // Water Elemental Slayer
	}
}
