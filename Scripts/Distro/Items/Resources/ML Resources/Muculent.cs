using System;

namespace Server.Items
{
	public class Muculent : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032680; } } // Muculent

		[Constructable]
		public Muculent()
			: this( 1 )
		{
		}

		[Constructable]
		public Muculent( int amount )
			: base( 0x3188 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public Muculent( Serial serial )
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