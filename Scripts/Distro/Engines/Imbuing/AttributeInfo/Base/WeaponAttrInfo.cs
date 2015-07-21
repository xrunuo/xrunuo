using System;
using Server;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the properties from <see cref="WeaponAttribute" /> enum.
	/// </summary>
	public abstract class WeaponAttrInfo : BaseAttrInfo
	{
		public abstract WeaponAttribute Attribute { get; }

		public override bool CanHold( Item item )
		{
			return item is IWeapon;
		}

		public override int GetValue( Item item )
		{
			return ( (IWeapon) item ).WeaponAttributes[Attribute];
		}

		public override void SetValue( Item item, int value )
		{
			( (IWeapon) item ).WeaponAttributes[Attribute] = (short) value;
		}
	}

	public abstract class HitSpellAttrInfo : WeaponAttrInfo
	{
		public override bool Replaces( BaseAttrInfo otherAttr )
		{
			Type otherType = otherAttr.GetType();

			foreach ( Type type in Imbuing.HitSpellTypes )
			{
				if ( otherType == type )
					return true;
			}

			return false;
		}
	}

	public abstract class HitAreaAttrInfo : WeaponAttrInfo
	{
		public override bool Replaces( BaseAttrInfo otherAttr )
		{
			Type otherType = otherAttr.GetType();

			foreach ( Type type in Imbuing.HitAreaTypes )
			{
				if ( otherType == type )
					return true;
			}

			return false;
		}
	}
}