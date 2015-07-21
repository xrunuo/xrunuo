using System;
using Server;

namespace Server.Items
{
	public class EssenceOfControl : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113340; } } // essence of control

		[Constructable]
		public EssenceOfControl()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfControl( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 1165;
			Amount = amount;
		}

		public EssenceOfControl( Serial serial )
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
