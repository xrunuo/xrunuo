using System;
using Server;

namespace Server.Items
{
	public class EssenceOfOrder : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113342; } } // essence of order

		[Constructable]
		public EssenceOfOrder()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfOrder( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 1153;
			Amount = amount;
		}

		public EssenceOfOrder( Serial serial )
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
