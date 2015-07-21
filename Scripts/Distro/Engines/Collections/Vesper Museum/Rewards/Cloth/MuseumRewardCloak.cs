using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardCloak : Cloak
	{
		public override int LabelNumber { get { return 1073252; } } // Baron Lenshire's Cloak - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardCloak()
		{
			ItemID = 0x1530;
			Hue = 641;
		}

		public MuseumRewardCloak( Serial serial )
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