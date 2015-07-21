using System;

namespace Server.Items
{
	public class CocoaPulp : Item
	{
		public override int LabelNumber { get { return 1080530; } } // cocoa pulp

		[Constructable]
		public CocoaPulp()
			: base( 0xF7C )
		{
			Weight = 1.0;
			Stackable = true;
			Hue = 0x73E;
		}

		public CocoaPulp( Serial serial )
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