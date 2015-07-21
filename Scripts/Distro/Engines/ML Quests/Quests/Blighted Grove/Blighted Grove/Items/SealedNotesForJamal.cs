using System;
using Server;

namespace Server.Items
{
	public class SealedNotesForJamal : Item
	{
		public override int LabelNumber { get { return 1074998; } } // sealed notes for Jamal
		public override int QuestItemHue { get { return PrivateHue; } }

		[Constructable]
		public SealedNotesForJamal()
			: base( 0xEF9 )
		{
			LootType = LootType.Blessed;
			Weight = 5;
		}

		public SealedNotesForJamal( Serial serial )
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