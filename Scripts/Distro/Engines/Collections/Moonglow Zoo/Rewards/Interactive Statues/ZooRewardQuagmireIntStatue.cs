using System;
using Server;

namespace Server.Items
{
	public class ZooRewardQuagmireIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1074848; } } // Interactive Quagmire Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardQuagmireIntStatue()
			: base( 0x2614 )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardQuagmireIntStatue( Serial serial )
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