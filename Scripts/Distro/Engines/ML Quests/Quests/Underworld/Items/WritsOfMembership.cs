using System;
using Server;

namespace Server.Items
{
	public class WritsOfMembership : Item
	{
		public override int LabelNumber { get { return 1094998; } } // Ariel Haven Writ of Membership

		[Constructable]
		public WritsOfMembership()
			: base( 0x14ED )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public WritsOfMembership( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}