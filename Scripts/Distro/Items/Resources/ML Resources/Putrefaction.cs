using System;

namespace Server.Items
{
	public class Putrefaction : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032678; } } // putrefaction

		[Constructable]
		public Putrefaction()
			: this( 1 )
		{
		}

		[Constructable]
		public Putrefaction( int amount )
			: base( 0x3186 )
		{
			Stackable = true;
			Weight = 1;
			Hue = 883;
			Amount = amount;
		}

		public Putrefaction( Serial serial )
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