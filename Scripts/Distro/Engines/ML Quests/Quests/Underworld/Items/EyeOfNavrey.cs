using System;
using Server;

namespace Server.Items
{
	public class EyeOfNavrey : Item
	{
		public override int LabelNumber { get { return 1095154; } } // Eye of Navrey Night-Eyes

		[Constructable]
		public EyeOfNavrey()
			: base( 0xF87 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			Hue = 0x8FD;
		}

		public EyeOfNavrey( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}