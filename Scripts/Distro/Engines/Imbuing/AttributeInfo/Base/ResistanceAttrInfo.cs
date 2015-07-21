using System;
using Server;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the properties from <see cref="ElementAttribute" /> enum.
	/// </summary>
	public abstract class ResistanceAttrInfo : BaseAttrInfo
	{
		public abstract ElementAttribute Attribute { get; }

		public override int Category { get { return 1114254; } } // Resists

		public override bool CanHold( Item item )
		{
			return item is IResistances;
		}

		public override int GetValue( Item item )
		{
			return ( (IResistances) item ).Resistances[Attribute];
		}

		public override void SetValue( Item item, int value )
		{
			( (IResistances) item ).Resistances[Attribute] = (short) value;
		}
	}
}