using System;

namespace Server.Items
{
	public class Corruption : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032676; } } // corruption

		[Constructable]
		public Corruption()
			: this( 1 )
		{
		}

		[Constructable]
		public Corruption( int amount )
			: base( 0x3184 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public Corruption( Serial serial )
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