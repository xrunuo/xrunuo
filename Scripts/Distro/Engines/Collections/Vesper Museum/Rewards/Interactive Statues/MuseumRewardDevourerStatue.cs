using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardDevourerStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1073245; } } // Dangerous Creatures Replica: Devourer of Souls - Museum of Vesper

		[Constructable]
		public MuseumRewardDevourerStatue()
			: base( 0x2623 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardDevourerStatue( Serial serial )
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