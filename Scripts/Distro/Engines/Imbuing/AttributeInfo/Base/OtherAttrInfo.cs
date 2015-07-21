using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents properties that are not included in the Imbuing System, but
	/// are needed to compute the total magical intensity of an item.
	/// </summary>
	public abstract class OtherAttrInfo : BaseAttrInfo
	{
		// Reflect Physical Damage
		public override int Name { get { return 1094717; } }

		// (Not Available)
		public override int Description { get { return 1094717; } }

		// (Not Available)
		public override int Category { get { return 1094717; } }

		public override double Weight { get { return 1.0; } }
		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.None; } }

		public override ImbuingResource PrimaryResource { get { return ImbuingResource.AbyssalCloth; } }
		public override ImbuingResource SecondaryResource { get { return ImbuingResource.AbyssalCloth; } }
		public override ImbuingResource FullResource { get { return ImbuingResource.AbyssalCloth; } }

		public override double IntensityInterval { get { return 1; } }
	}

	public class OtherMagicalAttrInfo : OtherAttrInfo
	{
		private MagicalAttribute m_Attribute;
		private int m_MinIntensity, m_MaxIntensity;

		public override double MinIntensity { get { return m_MinIntensity; } }
		public override double MaxIntensity { get { return m_MaxIntensity; } }

		public OtherMagicalAttrInfo( MagicalAttribute attr, int min, int max )
		{
			m_Attribute = attr;
			m_MinIntensity = min;
			m_MaxIntensity = max;
		}

		public override bool CanHold( Item item )
		{
			return item is IMagicalItem;
		}

		public override int GetValue( Item item )
		{
			return ( (IMagicalItem) item ).Attributes[m_Attribute];
		}

		public override void SetValue( Item item, int value )
		{
		}

		public override string ToString()
		{
			return m_Attribute.ToString();
		}
	}

	public class OtherWeaponAttrInfo : OtherAttrInfo
	{
		private WeaponAttribute m_Attribute;
		private int m_MinIntensity, m_MaxIntensity;

		public override double MinIntensity { get { return m_MinIntensity; } }
		public override double MaxIntensity { get { return m_MaxIntensity; } }

		public OtherWeaponAttrInfo( WeaponAttribute attr, int min, int max )
		{
			m_Attribute = attr;
			m_MinIntensity = min;
			m_MaxIntensity = max;
		}

		public override bool CanHold( Item item )
		{
			return item is IWeapon;
		}

		public override int GetValue( Item item )
		{
			return ( (IWeapon) item ).WeaponAttributes[m_Attribute];
		}

		public override void SetValue( Item item, int value )
		{
		}

		public override string ToString()
		{
			return m_Attribute.ToString();
		}
	}

	public class OtherArmorAttrInfo : OtherAttrInfo
	{
		private ArmorAttribute m_Attribute;
		private int m_MinIntensity, m_MaxIntensity;

		public override double MinIntensity { get { return m_MinIntensity; } }
		public override double MaxIntensity { get { return m_MaxIntensity; } }

		public OtherArmorAttrInfo( ArmorAttribute attr, int min, int max )
		{
			m_Attribute = attr;
			m_MinIntensity = min;
			m_MaxIntensity = max;
		}

		public override bool CanHold( Item item )
		{
			return item is IArmor;
		}

		public override int GetValue( Item item )
		{
			return ( (IArmor) item ).ArmorAttributes[m_Attribute];
		}

		public override void SetValue( Item item, int value )
		{
		}

		public override string ToString()
		{
			return m_Attribute.ToString();
		}
	}

	public class AbsorptionAttrInfo : OtherAttrInfo
	{
		private AbsorptionAttribute m_Attribute;
		private int m_MinIntensity, m_MaxIntensity;

		public override double MinIntensity { get { return m_MinIntensity; } }
		public override double MaxIntensity { get { return m_MaxIntensity; } }

		public AbsorptionAttrInfo( AbsorptionAttribute attr, int min, int max )
		{
			m_Attribute = attr;
			m_MinIntensity = min;
			m_MaxIntensity = max;
		}

		public override bool CanHold( Item item )
		{
			return item is IAbsorption;
		}

		public override int GetValue( Item item )
		{
			return ( (IAbsorption) item ).AbsorptionAttributes[m_Attribute];
		}

		public override void SetValue( Item item, int value )
		{
		}

		public override string ToString()
		{
			return m_Attribute.ToString();
		}
	}

	public class HitLowerDefendGlasses : OtherAttrInfo
	{
		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 15; } }
		public override double IntensityInterval { get { return 1; } }

		public HitLowerDefendGlasses()
		{
		}

		public override bool CanHold( Item item )
		{
			return item is ElvenGlasses;
		}

		public override int GetValue( Item item )
		{
			return ( (ElvenGlasses) item ).HitLowerDefend;
		}

		public override void SetValue( Item item, int value )
		{
		}
	}
}