using System;
using Server;

namespace Server.Items
{
	public class LavaSerpentCrust : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113336; } } // lava serpent crust

		[Constructable]
		public LavaSerpentCrust()
			: this( 1 )
		{
		}

		[Constructable]
		public LavaSerpentCrust( int amount )
			: base( 0x572D )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public LavaSerpentCrust( Serial serial )
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
