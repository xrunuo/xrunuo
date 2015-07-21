using System;

namespace Server.Items
{
	public class Scourge : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032677; } } // scourge

		[Constructable]
		public Scourge()
			: this( 1 )
		{
		}

		[Constructable]
		public Scourge( int amount )
			: base( 0x3185 )
		{
			Stackable = true;
			Weight = 1;
			Hue = 150;
			Amount = amount;
		}

		public Scourge( Serial serial )
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