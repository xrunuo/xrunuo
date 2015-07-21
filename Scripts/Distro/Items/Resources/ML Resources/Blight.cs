using System;

namespace Server.Items
{
	public class Blight : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032675; } } // blight

		[Constructable]
		public Blight()
			: this( 1 )
		{
		}

		[Constructable]
		public Blight( int amount )
			: base( 0x3183 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public Blight( Serial serial )
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