using System;
using Server;

namespace Server.Items
{
	public class SilverSnakeSkin : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113357; } } // silver snake skin

		[Constructable]
		public SilverSnakeSkin()
			: this( 1 )
		{
		}

		[Constructable]
		public SilverSnakeSkin( int amount )
			: base( 0x5744 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public SilverSnakeSkin( Serial serial )
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
