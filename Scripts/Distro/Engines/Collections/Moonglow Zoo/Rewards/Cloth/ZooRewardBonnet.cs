using System;
using Server;

namespace Server.Items
{
	public class ZooRewardBonnet : Bonnet
	{
		public override int LabelNumber { get { return 1073221; } } // Britannia Royal Zoo Member

		[Constructable]
		public ZooRewardBonnet()
		{
			Hue = 1365;
		}

		public ZooRewardBonnet( Serial serial )
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