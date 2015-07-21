using System;
using Server;

namespace Server.Items
{
	public class MuseumRewardBodySash : BodySash
	{
		public override int LabelNumber { get { return 1073253; } } // Lord Rourke's Sash - Museum of Vesper Replica

		[Constructable]
		public MuseumRewardBodySash()
		{
			Hue = 641;
		}

		public MuseumRewardBodySash( Serial serial )
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