using System;
using Server;

namespace Server.Items
{
	public class ZooSpecialAchievementAward : BaseZooInteractiveStatue
	{
		public override int LabelNumber { get { return 1073226; } } // Britannia Royal Zoo Special Achievement Award

		[Constructable]
		public ZooSpecialAchievementAward()
			: base( 0x2FF6 )
		{
			Weight = 10.0;
			LootType = LootType.Blessed;
		}

		public ZooSpecialAchievementAward( Serial serial )
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