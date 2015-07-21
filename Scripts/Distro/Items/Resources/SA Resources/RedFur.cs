using System;
using Server;

namespace Server.Items
{
	public class RedFur : Item
	{
		[Constructable]
		public RedFur()
			: this( 1 )
		{
		}

		[Constructable]
		public RedFur( int amount )
			: base( 0x1875 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x605;
		}

		public RedFur( Serial serial )
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