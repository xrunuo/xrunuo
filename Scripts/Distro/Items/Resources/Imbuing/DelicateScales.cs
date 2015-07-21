using System;
using Server;

namespace Server.Items
{
	public class DelicateScales : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113349; } } // delicate scales

		[Constructable]
		public DelicateScales()
			: this( 1 )
		{
		}

		[Constructable]
		public DelicateScales( int amount )
			: base( 0x573A )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public DelicateScales( Serial serial )
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
