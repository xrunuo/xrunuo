using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardMeerEternalStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075021; } } // Dangerous Creatures Replica: Meer Eternal - Museum of Vesper

		[Constructable]
		public MuseumRewardMeerEternalStatue()
			: base( 0x25F8 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardMeerEternalStatue( Serial serial )
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