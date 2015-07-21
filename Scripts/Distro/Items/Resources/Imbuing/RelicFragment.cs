using System;
using Server;

namespace Server.Items
{
	public class RelicFragment : Item, ICommodity
	{
		[Constructable]
		public RelicFragment()
			: this( 1 )
		{
		}

		[Constructable]
		public RelicFragment( int amount )
			: base( 0x2DB3 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public RelicFragment( Serial serial )
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