using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class PrimitiveFetish : Item
	{
		public override int LabelNumber { get { return 1074675; } } // Primitive Fetish

		[Constructable]
		public PrimitiveFetish()
			: base( 0x23F )
		{
			Hue = 580;
			Stackable = true;
			LootType = LootType.Blessed;
			Weight = 1.0;
		}

		public PrimitiveFetish( Serial serial )
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
