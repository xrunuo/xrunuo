using System;
using Server;

namespace Server.Items
{
	public class BouraSkin : Item
	{
		public override int LabelNumber { get { return 1112900; } } // Boura Skin

		[Constructable]
		public BouraSkin()
			: this( 1 )
		{
		}

		[Constructable]
		public BouraSkin( int amount )
			: base( 0x11F4 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x22B;
		}

		public BouraSkin( Serial serial )
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