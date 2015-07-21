using System;
using Server;

namespace Server.Items
{
	public class MedusaBlood : Item, ICommodity
	{
		[Constructable]
		public MedusaBlood()
			: this( 1 )
		{
		}

		[Constructable]
		public MedusaBlood( int amount )
			: base( 0x2DB6 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public MedusaBlood( Serial serial )
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