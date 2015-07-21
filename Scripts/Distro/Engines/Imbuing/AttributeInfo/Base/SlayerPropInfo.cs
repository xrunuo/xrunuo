using System;
using Server;
using Server.Items;

namespace Server.Engines.Imbuing
{
	/// <summary>
	/// Represents the Slayer properties from <see cref="SlayerName" /> enum.
	/// </summary>
	public abstract class SlayerPropInfo : BaseAttrInfo
	{
		public abstract SlayerName Slayer { get; }

		public override DisplayValue Display { get { return DisplayValue.NoShow; } }
		public override ImbuingFlag Flags { get { return ImbuingFlag.AllWeapons; } }

		public override double MinIntensity { get { return 1; } }
		public override double MaxIntensity { get { return 1; } }
		public override double IntensityInterval { get { return 1; } }

		public override int Category { get { return 1114263; } } // Slayers

		public override bool CanHold( Item item )
		{
			return item is ISlayer;
		}

		public override int GetValue( Item item )
		{
			ISlayer sl = item as ISlayer;
			return ( Slayer == sl.Slayer || Slayer == sl.Slayer2 ) ? 1 : 0;
		}

		public override void SetValue( Item item, int value )
		{
			if ( value != 0 )
			{
				ISlayer sl = item as ISlayer;

				if ( sl.Slayer == SlayerName.None )
					sl.Slayer = Slayer;
				else if ( sl.Slayer2 == SlayerName.None )
					sl.Slayer2 = Slayer;
			}
		}

		public override bool Replaces( BaseAttrInfo otherAttr )
		{
			return otherAttr is SlayerPropInfo;
		}
	}
}