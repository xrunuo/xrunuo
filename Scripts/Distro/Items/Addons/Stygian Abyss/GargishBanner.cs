using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x4038, 0x4037 )]
	public class GargishBanner : Item
	{
		public override int LabelNumber { get { return 1095311; } } // gargish banner

		[Constructable]
		public GargishBanner()
			: base( 0x4038 )
		{
			Weight = 1.0;
		}

		public GargishBanner( Serial serial )
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