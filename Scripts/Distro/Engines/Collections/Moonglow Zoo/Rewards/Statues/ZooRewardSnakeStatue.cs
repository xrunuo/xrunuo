using System;
using Server;

namespace Server.Items
{
	public class ZooRewardSnakeStatue : Item
	{
		public override int LabelNumber { get { return 1073194; } } // A Snake Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardSnakeStatue()
			: base( 0x20FC )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardSnakeStatue( Serial serial )
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