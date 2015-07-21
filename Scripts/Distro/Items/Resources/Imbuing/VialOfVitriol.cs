using System;
using Server;

namespace Server.Items
{
	public class VialOfVitriol : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113331; } } // vial of vitriol

		[Constructable]
		public VialOfVitriol()
			: this( 1 )
		{
		}

		[Constructable]
		public VialOfVitriol( int amount )
			: base( 0x5722 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public VialOfVitriol( Serial serial )
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
