using System;

namespace Server.Items
{
	public class CocoaButter : Item
	{
		public override int LabelNumber { get { return 1079998; } } // cocoa butter

		[Constructable]
		public CocoaButter()
			: base( 0x2FD6 )
		{
			Weight = 1.0;
			Stackable = true;
			Hue = 0x73E;
		}

		public CocoaButter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}