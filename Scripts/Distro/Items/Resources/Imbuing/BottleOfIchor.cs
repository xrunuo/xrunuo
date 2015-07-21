using System;
using Server;

namespace Server.Items
{
	public class BottleOfIchor : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113361; } } // bottle of ichor

		[Constructable]
		public BottleOfIchor()
			: this( 1 )
		{
		}

		[Constructable]
		public BottleOfIchor( int amount )
			: base( 0x5748 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public BottleOfIchor( Serial serial )
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
