using System;
using Server;

namespace Server.Items
{
	public class ZooRewardPolarBearStatue : Item
	{
		public override int LabelNumber { get { return 1073193; } } // A Polar Bear Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardPolarBearStatue()
			: base( 0x20E1 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardPolarBearStatue( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}