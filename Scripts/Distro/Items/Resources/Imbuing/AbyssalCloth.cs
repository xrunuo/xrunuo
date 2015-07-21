using System;
using Server;

namespace Server.Items
{
	public class AbyssalCloth : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113350; } } // abyssal cloth

		[Constructable]
		public AbyssalCloth()
			: this( 1 )
		{
		}

		[Constructable]
		public AbyssalCloth( int amount )
			: base( 0x1767 )
		{
			Weight = 0.1;
			Stackable = true;
			Hue = 2075;
			Amount = amount;
		}

		public AbyssalCloth( Serial serial )
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
