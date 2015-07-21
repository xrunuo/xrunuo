using System;
using Server;

namespace Server.Items
{
	public class ShortStool : Item
	{
		public override int LabelNumber { get { return 1075776; } } // Short Stool

		[Constructable]
		public ShortStool()
			: base( 0x11FC )
		{
			LootType = LootType.Blessed;
			Weight = 3.0;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public ShortStool( Serial serial )
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