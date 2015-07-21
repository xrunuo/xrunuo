using System;
using Server;

namespace Server.Items
{
	public class EssenceOfBalance : Item
	{
		public override int LabelNumber { get { return 1113324; } } // essence of balance

		[Constructable]
		public EssenceOfBalance()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfBalance( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 67;
			Amount = amount;
		}

		public EssenceOfBalance( Serial serial )
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
