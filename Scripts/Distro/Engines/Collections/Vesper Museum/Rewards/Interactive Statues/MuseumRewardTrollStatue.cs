using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardTrollStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1073242; } } // G'Thunk the Troll - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardTrollStatue()
			: base( 0x20E9 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardTrollStatue( Serial serial )
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