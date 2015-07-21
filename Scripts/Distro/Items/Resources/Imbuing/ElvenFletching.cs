using System;
using Server;

namespace Server.Items
{
	public class ElvenFletching : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113346; } } // elven fletching

		[Constructable]
		public ElvenFletching()
			: this( 1 )
		{
		}

		[Constructable]
		public ElvenFletching( int amount )
			: base( 0x5737 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public ElvenFletching( Serial serial )
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
