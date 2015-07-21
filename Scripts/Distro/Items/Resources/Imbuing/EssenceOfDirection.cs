using System;
using Server;

namespace Server.Items
{
	public class EssenceOfDirection : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113328; } } // essence of direction

		[Constructable]
		public EssenceOfDirection()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfDirection( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 97;
			Amount = amount;
		}

		public EssenceOfDirection( Serial serial )
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
