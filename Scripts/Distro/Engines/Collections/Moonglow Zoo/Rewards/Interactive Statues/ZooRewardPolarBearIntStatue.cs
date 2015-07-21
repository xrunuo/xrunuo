using System;
using Server;

namespace Server.Items
{
	public class ZooRewardPolarBearIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1074851; } } // Interactive Polar Bear Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardPolarBearIntStatue()
			: base( 0x20E1 )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardPolarBearIntStatue( Serial serial )
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