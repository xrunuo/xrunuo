using System;
using Server;

namespace Server.Items
{
	public class VillageCauldron : Item
	{
		public override int LabelNumber { get { return 1075775; } } // Village Cauldron

		[Constructable]
		public VillageCauldron()
			: base( 0x9ED )
		{
			LootType = LootType.Blessed;
			Weight = 30.0;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public VillageCauldron( Serial serial )
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