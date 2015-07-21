using System;
using Server;

namespace Server.Items
{
	public class EssenceOfFeeling : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113339; } } // essence of feeling

		[Constructable]
		public EssenceOfFeeling()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfFeeling( int amount )
			: base( 0x571C )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 52;
			Amount = amount;
		}

		public EssenceOfFeeling( Serial serial )
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
