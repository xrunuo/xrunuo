using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardRobe : Robe
	{
		public override int LabelNumber { get { return 1073250; } } // Odric's Robe - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardRobe()
		{
			Hue = 1409;
		}

		public MuseumRewardRobe( Serial serial )
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