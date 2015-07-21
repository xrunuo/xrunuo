using System;
using Server;

namespace Server.Items
{
	public class ZooRewardChangelingStatue : Item
	{
		public override int LabelNumber { get { return 1073191; } } // A Changeling Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardChangelingStatue()
			: base( 0x2D8A )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardChangelingStatue( Serial serial )
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