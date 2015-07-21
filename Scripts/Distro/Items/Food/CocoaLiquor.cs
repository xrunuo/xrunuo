using System;

namespace Server.Items
{
	public class CocoaLiquor : Item
	{
		public override int LabelNumber { get { return 1079999; } } // cocoa liquor

		[Constructable]
		public CocoaLiquor()
			: base( 0x103F )
		{
			Weight = 1.0;
			Stackable = false;
			Hue = 0x73E;
		}

		public CocoaLiquor( Serial serial )
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