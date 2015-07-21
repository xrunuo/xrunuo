using System;
using Server;

namespace Server.Items
{
	public class SpiderCarapace : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113329; } } // spider carapace

		[Constructable]
		public SpiderCarapace()
			: this( 1 )
		{
		}

		[Constructable]
		public SpiderCarapace( int amount )
			: base( 0x5720 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public SpiderCarapace( Serial serial )
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
