using System;
using Server;

namespace Server.Items
{
	public class YellowFur : Item
	{
		[Constructable]
		public YellowFur()
			: this( 1 )
		{
		}

		[Constructable]
		public YellowFur( int amount )
			: base( 0x1875 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x99;
		}

		public YellowFur( Serial serial )
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