using System;
using Server;

namespace Server.Items
{
	public class CrystalGranules : Item
	{
		public override int LabelNumber { get { return 1112329; } } // crystal granules

		[Constructable]
		public CrystalGranules()
			: base( 0x4008 )
		{
			Hue = 0xA0; // TODO (SA): check hue
			Stackable = false;
			Weight = 1.0;
		}

		public CrystalGranules( Serial serial )
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
