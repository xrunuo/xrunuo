using System;
using Server;

namespace Server.Items
{
	public class CrushedGlass : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113351; } } // crushed glass

		[Constructable]
		public CrushedGlass()
			: this( 1 )
		{
		}

		[Constructable]
		public CrushedGlass( int amount )
			: base( 0x573B )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public CrushedGlass( Serial serial )
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
