using System;
using Server;

namespace Server.Items
{
	public class SeasonedSkillet : Skillet
	{
		public override int LabelNumber { get { return 1075774; } } // Seasoned Skillet

		[Constructable]
		public SeasonedSkillet()
		{
			LootType = LootType.Blessed;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public SeasonedSkillet( Serial serial )
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