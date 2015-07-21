using System;
using Server;

namespace Server.Items
{
	public class PadsOfTheCuSidhe : FurBoots
	{
		public override int LabelNumber { get { return 1075048; } } // Pads of the Cu Sidhe

		[Constructable]
		public PadsOfTheCuSidhe()
			: base()
		{
			Hue = 1150;
		}

		public PadsOfTheCuSidhe( Serial serial )
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