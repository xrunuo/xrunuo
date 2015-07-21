using System;
using Server;

namespace Server.Items
{
	public class EssenceOfDiligence : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113338; } } // essence of diligence

		[Constructable]
		public EssenceOfDiligence()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfDiligence( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 1166;
			Amount = amount;
		}

		public EssenceOfDiligence( Serial serial )
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
