using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardExodusStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075018; } } // Dangerous Creatures Replica: Exodus Overseer - Museum of Vesper

		[Constructable]
		public MuseumRewardExodusStatue()
			: base( 0x260C )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardExodusStatue( Serial serial )
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