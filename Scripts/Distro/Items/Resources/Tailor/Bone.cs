using System;
using Server.Items;

namespace Server.Items
{
	public class Bone : Item, ICommodity
	{
		[Constructable]
		public Bone()
			: this( 1 )
		{
		}

		[Constructable]
		public Bone( int amount )
			: base( 0xf7e )
		{
			Stackable = true;
			Amount = amount;
			Weight = 0.1;
		}

		public Bone( Serial serial )
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

			if ( Weight == 1 )
				Weight = 0.1;
		}
	}
}