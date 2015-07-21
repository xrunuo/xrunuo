using System;

namespace Server.Items
{
	public class WhitePearl : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032694; } } // white pearl

		[Constructable]
		public WhitePearl()
			: this( 1 )
		{
		}

		[Constructable]
		public WhitePearl( int amount )
			: base( 0x3196 )
		{
			Stackable = true;
			Weight = 0.2;
			Amount = amount;
		}

		public WhitePearl( Serial serial )
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