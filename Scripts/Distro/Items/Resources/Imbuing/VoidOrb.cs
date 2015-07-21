using System;
using Server;

namespace Server.Items
{
	public class VoidOrb : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113354; } } // void orb

		[Constructable]
		public VoidOrb()
			: this( 1 )
		{
		}

		[Constructable]
		public VoidOrb( int amount )
			: base( 0x573E )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public VoidOrb( Serial serial )
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
