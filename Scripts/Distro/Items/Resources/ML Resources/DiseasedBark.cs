using System;

namespace Server.Items
{
	public class DiseasedBark : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032683; } } // Diseased Bark

		[Constructable]
		public DiseasedBark()
			: this( 1 )
		{
		}

		[Constructable]
		public DiseasedBark( int amount )
			: base( 0x318B )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public DiseasedBark( Serial serial )
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