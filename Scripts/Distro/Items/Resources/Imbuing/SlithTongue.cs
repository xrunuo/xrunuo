using System;
using Server;

namespace Server.Items
{
	public class SlithTongue : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113359; } } // slith tongue

		[Constructable]
		public SlithTongue()
			: this( 1 )
		{
		}

		[Constructable]
		public SlithTongue( int amount )
			: base( 0x5746 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public SlithTongue( Serial serial )
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
