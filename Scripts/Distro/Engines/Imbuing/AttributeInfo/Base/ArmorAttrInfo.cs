using System;
using Server;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the properties from <see cref="AosArmorAttribute" /> enum.
	/// </summary>
	public abstract class ArmorAttrInfo : BaseAttrInfo
	{
		public abstract AosArmorAttribute Attribute { get; }

		public override bool CanHold( Item item )
		{
			return item is IArmor;
		}

		public override int GetValue( Item item )
		{
			return ( (IArmor) item ).ArmorAttributes[Attribute];
		}

		public override void SetValue( Item item, int value )
		{
			( (IArmor) item ).ArmorAttributes[Attribute] = (short) value;
		}
	}
}