using System;

namespace Server.Items
{
	public class Arrow : Item, ICommodity
	{
		[Constructable]
		public Arrow()
			: this( 1 )
		{
		}

		[Constructable]
		public Arrow( int amount )
			: base( 0xF3F )
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public Arrow( Serial serial )
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