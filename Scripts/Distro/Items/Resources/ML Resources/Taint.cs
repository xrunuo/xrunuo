using System;

namespace Server.Items
{
	public class Taint : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032679; } } // taint

		[Constructable]
		public Taint()
			: this( 1 )
		{
		}

		[Constructable]
		public Taint( int amount )
			: base( 0x3187 )
		{
			Stackable = true;
			Weight = 1;
			Hue = 731;
			Amount = amount;
		}

		public Taint( Serial serial )
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