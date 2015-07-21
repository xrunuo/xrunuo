using System;

namespace Server.Items
{
	[Furniture]
	public class RoseInVase : Item
	{
		public override int LabelNumber { get { return 1023760; } } // A Rose in a Vase

		[Constructable]
		public RoseInVase()
			: base( 3760 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 32;
		}

		public RoseInVase( Serial serial )
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
