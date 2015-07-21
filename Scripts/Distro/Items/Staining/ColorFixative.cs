using System;
using Server;

namespace Server.Items
{
	public class ColorFixative : Item
	{
		public override int LabelNumber { get { return 1112135; } } // color fixative

		[Constructable]
		public ColorFixative()
			: base( 0x182D )
		{
			Hue = 0x505;
			Stackable = true;
			Weight = 1.0;
		}

		public ColorFixative( Serial serial )
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