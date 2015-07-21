using System;
using Server.Items;

namespace Server.Items
{
	public class MagicalDoor : Item
	{
		public override int LabelNumber { get { return 1112410; } } // Magical Door [Replica]

		[Constructable]
		public MagicalDoor()
			: base( 0x1EE1 )
		{
			Weight = 1.0;
		}

		public MagicalDoor( Serial serial )
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
