using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;

namespace Server
{
	public class BaseLootHelperSE
	{
		public static Item RandomFootWears()
		{
			switch ( Utility.Random( 4 ) )
			{
				case 0:
					return new Sandals();
				case 1:
					return new Shoes();
				case 2:
					return new Boots();
				case 3:
					return new ThighBoots();
			}

			return null;
		}

		public static Item RandomBodyPart()
		{
			switch ( Utility.Random( 6 ) )
			{
				case 0:
					return new Torso();
				case 1:
					return new LeftLeg();
				case 2:
					return new LeftArm();
				case 3:
					return new RightLeg();
				case 4:
					return new RightArm();
				case 5:
					return new Head();
			}

			return null;
		}
	}
}