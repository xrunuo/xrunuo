using System;
using Server;

namespace Server.Items
{
	public class CrateForSledge : Item
	{
		public override int LabelNumber { get { return 1074520; } } // Crate for Sledge
		public override int QuestItemHue { get { return PrivateHue; } }

		[Constructable]
		public CrateForSledge()
			: base( 0x1FFF )
		{
			Weight = 5.0;
			LootType = LootType.Blessed;
		}

		public CrateForSledge( Serial serial )
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

