using System;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class EgwexemsWrit : Item
	{
		public override int LabelNumber { get { return 1112520; } } // Egwexem's Writ

		[Constructable]
		public EgwexemsWrit()
			: base( 0x14EF )
		{
			LootType = LootType.Blessed;
			Hue = 0x22C;
			Weight = 1.0;
		}

		public override int QuestItemHue { get { return 0x22C; } }
		public override bool NonTransferable { get { return true; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public EgwexemsWrit( Serial serial )
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
