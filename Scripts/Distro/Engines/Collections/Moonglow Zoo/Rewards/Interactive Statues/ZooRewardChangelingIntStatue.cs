using System;
using Server;

namespace Server.Items
{
	public class ZooRewardChangelingIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1074850; } } // Interactive Changeling Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardChangelingIntStatue()
			: base( 0x2D8A )
		{
			Hue = 52;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardChangelingIntStatue( Serial serial )
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