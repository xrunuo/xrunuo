using System;
using Server;

namespace Server.Items
{
	public class ZooRewardReptalonIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1074852; } } // Interactive Reptalon Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardReptalonIntStatue()
			: base( 0x2D95 )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardReptalonIntStatue( Serial serial )
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