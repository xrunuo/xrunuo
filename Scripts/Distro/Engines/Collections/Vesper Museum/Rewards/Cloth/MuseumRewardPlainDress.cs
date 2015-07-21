using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardPlainDress : PlainDress
	{
		public override int LabelNumber { get { return 1073251; } } // Malabelle's Dress - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardPlainDress()
		{
			Hue = 553;
		}

		public MuseumRewardPlainDress( Serial serial )
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