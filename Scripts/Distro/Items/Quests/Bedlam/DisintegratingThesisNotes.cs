using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class DisintegratingThesisNotes : TransientItem
	{
		public override int LabelNumber { get { return 1074440; } } // Disintegrating Thesis Notes

		[Constructable]
		public DisintegratingThesisNotes()
			: base( 0xE36, TimeSpan.FromSeconds( 7200.0 ) )
		{
			LootType = LootType.Blessed;
			Weight = 1.0;
			Stackable = false;
		}

		public DisintegratingThesisNotes( Serial serial )
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