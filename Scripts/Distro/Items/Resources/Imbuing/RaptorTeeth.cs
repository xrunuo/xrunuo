using System;
using Server;

namespace Server.Items
{
	public class RaptorTeeth : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113360; } } // raptor teeth

		[Constructable]
		public RaptorTeeth()
			: this( 1 )
		{
		}

		[Constructable]
		public RaptorTeeth( int amount )
			: base( 0x5747 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public RaptorTeeth( Serial serial )
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
