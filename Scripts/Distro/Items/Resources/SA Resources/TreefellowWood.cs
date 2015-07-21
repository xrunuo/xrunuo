using System;
using Server;

namespace Server.Items
{
	public class TreefellowWood : Item
	{
		public override int LabelNumber { get { return 1112908; } } // treefellow wood

		[Constructable]
		public TreefellowWood()
			: this( 1 )
		{
		}

		[Constructable]
		public TreefellowWood( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 0x2B;
		}

		public TreefellowWood( Serial serial )
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