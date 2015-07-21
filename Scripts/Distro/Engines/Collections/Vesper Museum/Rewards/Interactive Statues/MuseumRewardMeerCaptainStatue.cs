using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardMeerCaptainStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075020; } } // Dangerous Creatures Replica: Meer Captain - Museum of Vesper

		[Constructable]
		public MuseumRewardMeerCaptainStatue()
			: base( 0x25FA )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardMeerCaptainStatue( Serial serial )
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