using System;
using Server;

namespace Server.Items
{
	public class ZooRewardBakeKitsuneStatue : Item
	{
		public override int LabelNumber { get { return 1073189; } } // A Bake Kitsune Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardBakeKitsuneStatue()
			: base( 0x2763 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardBakeKitsuneStatue( Serial serial )
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