using System;
using Server;

namespace Server.Items
{
	public class EssenceOfPrecision : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113327; } } // essence of precision

		[Constructable]
		public EssenceOfPrecision()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfPrecision( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 22;
			Amount = amount;
		}

		public EssenceOfPrecision( Serial serial )
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
