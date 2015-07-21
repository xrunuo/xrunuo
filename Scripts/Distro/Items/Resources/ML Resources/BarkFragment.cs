using System;

namespace Server.Items
{
	public class BarkFragment : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032687; } } // Bark Fragment

		[Constructable]
		public BarkFragment()
			: this( 1 )
		{
		}

		[Constructable]
		public BarkFragment( int amount )
			: base( 0x318F )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public BarkFragment( Serial serial )
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