using System;

namespace Server.Items
{
	public class BlueDiamond : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032696; } } // Blue Diamond

		[Constructable]
		public BlueDiamond()
			: this( 1 )
		{
		}

		[Constructable]
		public BlueDiamond( int amount )
			: base( 0x3198 )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public BlueDiamond( Serial serial )
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