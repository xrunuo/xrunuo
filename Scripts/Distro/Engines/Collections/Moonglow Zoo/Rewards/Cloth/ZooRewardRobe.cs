using System;
using Server;

namespace Server.Items
{
	public class ZooRewardRobe : Robe
	{
		public override int LabelNumber { get { return 1073221; } } // Britannia Royal Zoo Member

		[Constructable]
		public ZooRewardRobe()
		{
			ItemID = 0x1F04;
			Hue = 1365;
		}

		public ZooRewardRobe( Serial serial )
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