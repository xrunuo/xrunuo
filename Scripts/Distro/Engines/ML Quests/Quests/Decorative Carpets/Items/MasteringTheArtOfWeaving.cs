using System;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class MasteringTheArtOfWeaving : Item
	{
		public override int LabelNumber { get { return 1113244; } } // Mastering the Art of Weaving

		[Constructable]
		public MasteringTheArtOfWeaving()
			: base( 0x1E20 )
		{
			Hue = 0x2E8;
			Weight = 2.0;
		}

		public override int QuestItemHue { get { return 0x2E8; } }
		public override bool NonTransferable { get { return true; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public MasteringTheArtOfWeaving( Serial serial )
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
