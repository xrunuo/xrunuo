using System;
using Server;

namespace Server.Items
{
	public class GreenFur : Item
	{
		[Constructable]
		public GreenFur()
			: this( 1 )
		{
		}

		[Constructable]
		public GreenFur( int amount )
			: base( 0x1875 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x3A;
		}

		public GreenFur( Serial serial )
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