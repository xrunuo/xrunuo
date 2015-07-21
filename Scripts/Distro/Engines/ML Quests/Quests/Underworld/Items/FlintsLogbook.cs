using System;
using Server;

namespace Server.Items
{
	public class FlintsLogbook : Item
	{
		public override int LabelNumber { get { return 1095000; } } // Flint's Logbook

		[Constructable]
		public FlintsLogbook()
			: base( 0x1C11 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public FlintsLogbook( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1005420 ); // You cannot use this.
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