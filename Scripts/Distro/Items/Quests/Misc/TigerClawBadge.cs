using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class TigerClawBadge : Item
	{
		public override int LabelNumber { get { return 1073140; } } // A Tiger Claw Sect Badge

		[Constructable]
		public TigerClawBadge()
			: base( 0x23D )
		{
			Stackable = true;
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public TigerClawBadge( Serial serial )
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
