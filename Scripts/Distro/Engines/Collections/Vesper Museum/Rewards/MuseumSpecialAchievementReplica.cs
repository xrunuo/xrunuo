using System;
using Server;

namespace Server.Items
{
	public class MuseumSpecialAchievementReplica : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1073265; } } // Museum of Vesper Special Achievement Replica

		[Constructable]
		public MuseumSpecialAchievementReplica()
			: base( 0x2D4E )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumSpecialAchievementReplica( Serial serial )
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