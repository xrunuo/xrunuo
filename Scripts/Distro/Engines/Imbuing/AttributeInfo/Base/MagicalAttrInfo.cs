using System;
using Server;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the properties from <see cref="AosAttribute" /> enum.
	/// </summary>
	public abstract class MagicalAttrInfo : BaseAttrInfo
	{
		public abstract AosAttribute Attribute { get; }

		public override bool CanHold( Item item )
		{
			return item is IMagicalItem;
		}

		public override int GetValue( Item item )
		{
			return ( (IMagicalItem) item ).Attributes[Attribute];
		}

		public override void SetValue( Item item, int value )
		{
			( (IMagicalItem) item ).Attributes[Attribute] = (short) value;
		}
	}
}