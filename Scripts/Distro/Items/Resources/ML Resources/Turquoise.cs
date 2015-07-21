using System;

namespace Server.Items
{
	public class Turquoise : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032691; } } // Turquoise

		[Constructable]
		public Turquoise()
			: this( 1 )
		{
		}

		[Constructable]
		public Turquoise( int amount )
			: base( 0x3193 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public Turquoise( Serial serial )
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