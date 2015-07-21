using System;
using Server;

namespace Server.Engines.Imbuing
{
	public enum DisplayValue
	{
		NoShow,
		Value,
		ValuePercentage,
		PlusValue,
		MinusValue
	}

	/// <summary>
	/// This is the base class that represents all the properties that can be imbued
	/// to any items. Provides info about the Name, Description, Weight of the property,
	/// needed info to show it in the gump, the type of items that can hold the property,
	/// the intensity ranges and possible values and the resources needed for imbuing it.
	/// </summary>
	public abstract class BaseAttrInfo
	{
		// TID of the Property Name.
		public abstract int Name { get; }

		// TID of the Property Description.
		public abstract int Description { get; }

		// Weight of the Property.
		public abstract double Weight { get; }

		// How is the value displayed in the Imbuing gump?
		public abstract DisplayValue Display { get; }

		// What kind of item can this property be imbued in?
		public abstract ImbuingFlag Flags { get; }

		// Intensity Info
		public abstract double MinIntensity { get; }
		public abstract double MaxIntensity { get; }
		public abstract double IntensityInterval { get; }

		// Category of this property
		public abstract int Category { get; }

		// Resources needed to imbue this property.
		public abstract ImbuingResource PrimaryResource { get; }
		public abstract ImbuingResource SecondaryResource { get; }
		public abstract ImbuingResource FullResource { get; }

		// Methods to get and set the property to an item.
		// Should only be called after ensure CanHold( item ) return true,
		// otherwise they may throw an InvalidCastException.
		public abstract int GetValue( Item item );
		public abstract void SetValue( Item item, int value );

		// Return true if this property replaces otherAttr property.
		public virtual bool Replaces( BaseAttrInfo otherAttr )
		{
			return this.GetType() == otherAttr.GetType();
		}

		// Return true if the given item can hold this property.
		public abstract bool CanHold( Item item );

		// Returns if makes sense that the given item hold this property.
		// For example, for a meditable item and the Mage Weapon property,
		// it may return false.
		public virtual bool Validate( Item item )
		{
			return true;
		}

		// Returns the value that is displayed in the gump.
		public virtual int Modify( Item item, int value )
		{
			return value;
		}

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}