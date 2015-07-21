using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class MurkyMilk : BaseBeverage
	{
		public override int MaxQuantity { get { return 10; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
			{
				switch ( Content )
				{
					case BeverageType.Milk:
						return 0x1F97;
				}
			}

			return 0;
		}

		[Constructable]
		public MurkyMilk( BeverageType type )
			: base( type )
		{
			Name = "Murky Milk";
			Weight = 1.0;
			Hue = 2306;
			LootType = LootType.Blessed;
		}

		public MurkyMilk( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
				Label1 = "Halloween 2011";
		}
	}
}

