using System;
using Server;

namespace Server.Items
{
	public class GnawsFang : TransientItem
	{
		public override int LabelNumber { get { return 1074332; } } // Gnaw's Fang

		[Constructable]
		public GnawsFang()
			: base( 0x10E8, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 0x4F5;
			LootType = LootType.Blessed;
		}

		public GnawsFang( Serial serial )
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