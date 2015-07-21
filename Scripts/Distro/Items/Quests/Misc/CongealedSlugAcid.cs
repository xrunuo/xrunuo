using System;
using Server;

namespace Server.Items
{
	public class CongealedSlugAcid : Item
	{
		public override int LabelNumber { get { return 1112901; } } // Congealed Slug Acid

		[Constructable]
		public CongealedSlugAcid()
			: base( 0x122A )
		{
			Weight = 1.0;
			Hue = 0x51;
		}

		public CongealedSlugAcid( Serial serial )
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
