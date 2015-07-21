using System;
using Server;

namespace Server.Items
{
	public class ZooRewardBakeKitsuneIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1074849; } } // Interactive Bake Kitsune Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardBakeKitsuneIntStatue()
			: base( 0x2763 )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardBakeKitsuneIntStatue( Serial serial )
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