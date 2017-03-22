using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class LowerRequirements : ArmorAttrInfo
	{
		// Lower Requirements
		public override int Name { get { return 1079757; } }

		// Lowers the strength, dexterity, and intelligence requirements to wear an object.
		public override int Description { get { return 1111998; } }

		// Misc.
		public override int Category { get { return 1114252; } }

		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.LowerStatReq; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Shield; } }

		public override double MinIntensity { get { return 10; } }
		public override double MaxIntensity { get { return 100; } }
		public override double IntensityInterval { get { return 10; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amethyst; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.ElvenFletching; } }
	}

	public class MageArmor : ArmorAttrInfo
	{
		// Mage Armor
		public override int Name { get { return 1079758; } }

		// Eliminates an item's Meditation and Stealth penalties.
		public override int Description { get { return 1112000; } }

		// Casting
		public override int Category { get { return 1114248; } }

		public override AosArmorAttribute Attribute { get { return AosArmorAttribute.MageArmor; } }
		public override double Weight { get { return 1.4; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.Armor; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.EnchantedEssence; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.AbyssalCloth; } }

		public override bool Validate( Item item )
		{
			if ( item is BaseArmor )
				return !( (BaseArmor) item ).Meditable;

			return true;
		}
	}
}