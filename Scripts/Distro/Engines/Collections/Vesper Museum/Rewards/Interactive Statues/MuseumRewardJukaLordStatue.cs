using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardJukaLordStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075019; } } // Dangerous Creatures Replica: Juka Lord- Museum of Vesper

		[Constructable]
		public MuseumRewardJukaLordStatue()
			: base( 0x25FC )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardJukaLordStatue( Serial serial )
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