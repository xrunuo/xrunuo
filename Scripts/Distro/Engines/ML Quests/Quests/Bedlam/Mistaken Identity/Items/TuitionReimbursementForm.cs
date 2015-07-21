using System;
using Server;

namespace Server.Items
{
	public class TuitionReimbursementForm : Item
	{
		public override int LabelNumber { get { return 1074610; } } // Tuition Reimbursement Form (in triplicate)

		[Constructable]
		public TuitionReimbursementForm()
			: base( 0xE3A )
		{
			LootType = LootType.Blessed;
			Weight = 1;
			Hue = 0x395;
		}

		public TuitionReimbursementForm( Serial serial )
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