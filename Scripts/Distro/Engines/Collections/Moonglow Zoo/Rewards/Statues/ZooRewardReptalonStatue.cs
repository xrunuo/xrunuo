using System;
using Server;

namespace Server.Items
{
	public class ZooRewardReptalonStatue : Item
	{
		public override int LabelNumber { get { return 1073192; } } // A Reptalon Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardReptalonStatue()
			: base( 0x2D95 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardReptalonStatue( Serial serial )
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