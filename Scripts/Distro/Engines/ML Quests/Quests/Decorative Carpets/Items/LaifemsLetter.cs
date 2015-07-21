using System;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class LaifemsLetter : Item
	{
		public override int LabelNumber { get { return 1113243; } } // Laifem's Letter of Introduction

		[Constructable]
		public LaifemsLetter()
			: base( 0x1F23 )
		{
			Hue = 0x48F;
			Weight = 2.0;
		}

		public override int QuestItemHue { get { return 0x48F; } }
		public override bool NonTransferable { get { return true; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public LaifemsLetter( Serial serial )
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
