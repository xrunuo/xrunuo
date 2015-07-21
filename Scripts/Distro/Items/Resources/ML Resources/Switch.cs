using System;

namespace Server.Items
{
	public class SwitchItem : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032127; } } // switch

		[Constructable]
		public SwitchItem()
			: this( 1 )
		{
		}

		[Constructable]
		public SwitchItem( int amount )
			: base( 0x2F5F )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
		}

		public SwitchItem( Serial serial )
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