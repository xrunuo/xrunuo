using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	public class PhysicalResist : ResistanceAttrInfo
	{
		// Physical Resist
		public override int Name { get { return 1079764; } }

		// Increases the wearer's Physical Resistance.
		public override int Description { get { return 1112010; } }

		public override ElementAttribute Attribute { get { return ElementAttribute.Physical; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Diamond; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BouraPelt; } }

		public override int Modify( Item item, int value )
		{
			if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;

				return value + armor.BasePhysicalResistance;
			}
			else
				return value;
		}

		public override void SetValue( Item item, int value )
		{
			base.SetValue( item, value );

			if ( item is BaseArmor )
				( (BaseArmor) item ).PhysicalBonus = 0;
		}
	}

	public class FireResist : ResistanceAttrInfo
	{
		// Fire Resist
		public override int Name { get { return 1079763; } }

		// Increases the wearer's Fire Resistance.
		public override int Description { get { return 1112009; } }

		public override ElementAttribute Attribute { get { return ElementAttribute.Fire; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Ruby; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BouraPelt; } }

		public override int Modify( Item item, int value )
		{
			if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;

				return value + armor.BaseFireResistance;
			}
			else
				return value;
		}

		public override void SetValue( Item item, int value )
		{
			base.SetValue( item, value );

			if ( item is BaseArmor )
				( (BaseArmor) item ).FireBonus = 0;
		}
	}

	public class ColdResist : ResistanceAttrInfo
	{
		// Cold Resist
		public override int Name { get { return 1079761; } }

		// Increases the wearer's Cold Resistance.
		public override int Description { get { return 1112007; } }

		public override ElementAttribute Attribute { get { return ElementAttribute.Cold; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Sapphire; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BouraPelt; } }

		public override int Modify( Item item, int value )
		{
			if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;

				return value + armor.BaseColdResistance;
			}
			else
				return value;
		}

		public override void SetValue( Item item, int value )
		{
			base.SetValue( item, value );

			if ( item is BaseArmor )
				( (BaseArmor) item ).ColdBonus = 0;
		}
	}

	public class PoisonResist : ResistanceAttrInfo
	{
		// Poison Resist
		public override int Name { get { return 1079765; } }

		// Increases the wearer's Poison Resistance.
		public override int Description { get { return 1112011; } }

		public override ElementAttribute Attribute { get { return ElementAttribute.Poison; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Emerald; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BouraPelt; } }

		public override int Modify( Item item, int value )
		{
			if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;

				return value + armor.BasePoisonResistance;
			}
			else
				return value;
		}

		public override void SetValue( Item item, int value )
		{
			base.SetValue( item, value );

			if ( item is BaseArmor )
				( (BaseArmor) item ).PoisonBonus = 0;
		}
	}

	public class EnergyResist : ResistanceAttrInfo
	{
		// Energy Resist
		public override int Name { get { return 1079762; } }

		// Increases the wearer's Energy Resistance.
		public override int Description { get { return 1112008; } }

		public override ElementAttribute Attribute { get { return ElementAttribute.Energy; } }
		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.ValuePercentage; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons | ImbuingFlag.Armor | ImbuingFlag.Jewelry; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.MagicalResidue; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.Amethyst; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.BouraPelt; } }

		public override int Modify( Item item, int value )
		{
			if ( item is BaseArmor )
			{
				BaseArmor armor = item as BaseArmor;

				return value + armor.BaseEnergyResistance;
			}
			else
				return value;
		}

		public override void SetValue( Item item, int value )
		{
			base.SetValue( item, value );

			if ( item is BaseArmor )
				( (BaseArmor) item ).EnergyBonus = 0;
		}
	}
}