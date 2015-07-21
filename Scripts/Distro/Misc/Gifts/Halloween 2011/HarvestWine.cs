using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class HarvestWine : BaseBeverage
	{
		public override int MaxQuantity { get { return 10; } }

		public override int ComputeItemID()
		{
			if ( !IsEmpty )
			{
				switch ( Content )
				{
					case BeverageType.Gluhwein:
						return 0x99B;
				}
			}

			return 0;
		}

		[Constructable]
		public HarvestWine()
			: base( BeverageType.Gluhwein )
		{
			Name = "Harvest Wine";
			Weight = 1.0;
			Hue = 326;
			LootType = LootType.Blessed;
		}

		public HarvestWine( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
