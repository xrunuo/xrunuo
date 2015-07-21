using System;
using Server;

namespace Server.Items
{
	public class ZooRewardFloppyHat : FloppyHat
	{
		public override int LabelNumber { get { return 1073221; } } // Britannia Royal Zoo Member

		[Constructable]
		public ZooRewardFloppyHat()
		{
			Hue = 1365;
		}

		public ZooRewardFloppyHat( Serial serial )
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