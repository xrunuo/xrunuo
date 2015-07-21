using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardSolenQueenStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075022; } } // Dangerous Creatures Replica: Solen Queen - Museum of Vesper

		[Constructable]
		public MuseumRewardSolenQueenStatue()
			: base( 0x2602 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardSolenQueenStatue( Serial serial )
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