using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardLadyOfTheSnowStatue : BaseMuseumInteractiveStatue
	{
		public override int LabelNumber { get { return 1075016; } } // Dangerous Creatures Replica: Lady of the Snow - Museum of Vesper

		[Constructable]
		public MuseumRewardLadyOfTheSnowStatue()
			: base( 0x276C )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public MuseumRewardLadyOfTheSnowStatue( Serial serial )
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