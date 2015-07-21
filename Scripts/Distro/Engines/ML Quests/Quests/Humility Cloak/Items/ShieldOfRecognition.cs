using System;
using Server;

namespace Server.Items
{
	public class ShieldOfRecognition : OrderShield
	{
		public override int LabelNumber { get { return 1075851; } } // Shield of Recognition

		[Constructable]
		public ShieldOfRecognition()
		{
			ItemID = 0x1BC5;
			Hue = 0x32;
			ArmorAttributes.LowerStatReq = 80;
		}

		public ShieldOfRecognition( Serial serial )
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