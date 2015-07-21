using System;
using Server;

namespace Server.Items
{
	public class SabrixsEye : TransientItem
	{
		public override int LabelNumber { get { return 1074336; } } // Sabrix's Eye

		[Constructable]
		public SabrixsEye()
			: base( 0x0F87, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 1152;
			LootType = LootType.Blessed;
		}

		public SabrixsEye( Serial serial )
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