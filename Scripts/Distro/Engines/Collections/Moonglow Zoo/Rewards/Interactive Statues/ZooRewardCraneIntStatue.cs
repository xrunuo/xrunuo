using System;
using Server;

namespace Server.Items
{
	public class ZooRewardCraneIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1073197; } } // An Interactive Crane Contribution Statue from the Britannia Royal Zoo.

		[Constructable]
		public ZooRewardCraneIntStatue()
			: base( 0x2764 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardCraneIntStatue( Serial serial )
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