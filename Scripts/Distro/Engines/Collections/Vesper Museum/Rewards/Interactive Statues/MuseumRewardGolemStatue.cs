using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardGolemStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075017; } } // Dangerous Creatures Replica: Golem - Museum of Vesper

		[Constructable]
		public MuseumRewardGolemStatue()
			: base( 0x2610 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardGolemStatue( Serial serial )
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