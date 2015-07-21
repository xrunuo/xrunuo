using System;
using Server;

namespace Server.Items
{
	public class DarkGreenFur : Item
	{
		[Constructable]
		public DarkGreenFur()
			: this( 1 )
		{
		}

		[Constructable]
		public DarkGreenFur( int amount )
			: base( 0x1875 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x3E;
		}

		public DarkGreenFur( Serial serial )
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