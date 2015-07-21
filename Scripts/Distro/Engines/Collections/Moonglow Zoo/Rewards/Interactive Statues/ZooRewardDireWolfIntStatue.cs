using System;
using Server;

namespace Server.Items
{
	public class ZooRewardDireWolfIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1073196; } } // Interactive Dire Wolf Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardDireWolfIntStatue()
			: base( 0x25D0 )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardDireWolfIntStatue( Serial serial )
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