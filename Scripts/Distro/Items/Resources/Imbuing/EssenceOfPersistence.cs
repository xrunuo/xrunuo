using System;
using Server;

namespace Server.Items
{
	public class EssenceOfPersistence : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113343; } } // essence of persistence

		[Constructable]
		public EssenceOfPersistence()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfPersistence( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 37;
			Amount = amount;
		}

		public EssenceOfPersistence( Serial serial )
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
