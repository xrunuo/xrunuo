using System;

namespace Server.Items
{
	public class LardOfParoxysmus : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032681; } } // Lard of Paroxysmus

		[Constructable]
		public LardOfParoxysmus()
			: this( 1 )
		{
		}

		[Constructable]
		public LardOfParoxysmus( int amount )
			: base( 0x3189 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public LardOfParoxysmus( Serial serial )
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