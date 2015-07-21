using System;
using Server;

namespace Server.Items
{
	public class ReginasLetter : Item
	{
		public override int LabelNumber { get { return 1075306; } } // Regina's Letter
		public override int QuestItemHue { get { return PrivateHue; } }

		[Constructable]
		public ReginasLetter()
			: base( 0x14ED )
		{
			LootType = LootType.Blessed;
			Weight = 1;
		}

		public ReginasLetter( Serial serial )
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