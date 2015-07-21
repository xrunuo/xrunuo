using System;
using Server;

namespace Server.Items
{
	public class FeyWings : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113332; } } // fey wings

		[Constructable]
		public FeyWings()
			: this( 1 )
		{
		}

		[Constructable]
		public FeyWings( int amount )
			: base( 0x5726 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = 1;
		}

		public FeyWings( Serial serial )
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
