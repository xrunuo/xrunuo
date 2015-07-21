using System;
using Server;

namespace Server.Items
{
	public class ZooRewardSilverSteedIntStatue : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1073219; } } // Interactive Silver Steed Contribution Statue

		[Constructable]
		public ZooRewardSilverSteedIntStatue()
			: base( 0x259D )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ZooRewardSilverSteedIntStatue( Serial serial )
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