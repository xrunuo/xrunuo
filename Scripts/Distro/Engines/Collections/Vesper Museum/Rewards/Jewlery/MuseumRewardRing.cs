using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardRing : GoldRing
	{
		public override int LabelNumber { get { return 1073234; } } // A Souvenir from the Museum of Vesper

		[Constructable]
		public MuseumRewardRing()
		{
			Hue = 42;
		}

		public MuseumRewardRing( Serial serial )
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