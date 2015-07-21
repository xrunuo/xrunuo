using System;
using Server;
using Server.Engines.Quests;

namespace Server.Items
{
	public class MapFragment : BaseQuestItem
	{
		public override int LabelNumber { get { return 1074533; } } // Fragment of a Map

		[Constructable]
		public MapFragment()
			: base( 0x14ED )
		{
			LootType = LootType.Blessed;
			Weight = 1;
		}

		public MapFragment( Serial serial )
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

			int version = reader.ReadInt();
		}
	}
}