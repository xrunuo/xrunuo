using System;
using Server;

namespace Server.Items
{
	public class BarrelOfBarley : Container
	{
		public override int LabelNumber { get { return 1094999; } } // Barrel of Barley

		[Constructable]
		public BarrelOfBarley()
			: base( 0xFAE )
		{
			Weight = 25.0;
			LootType = LootType.Blessed;
		}

		public BarrelOfBarley( Serial serial )
			: base( serial )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			from.SendLocalizedMessage( 1095214 ); // This container cannot hold that.
			return false;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			from.SendLocalizedMessage( 1005420 ); // You cannot use this.
			return false;
		}

		public override void OnDoubleClickSecureTrade( Mobile from )
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