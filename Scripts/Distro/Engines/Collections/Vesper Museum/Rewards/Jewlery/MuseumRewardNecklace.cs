using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardNecklace : GoldNecklace
	{
		public override int LabelNumber { get { return 1073234; } } // A Souvenir from the Museum of Vesper

		[Constructable]
		public MuseumRewardNecklace()
		{
			Hue = 42;
		}

		public MuseumRewardNecklace( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}