using System;

namespace Server.Items
{
	[Flipable( 0x2BDB, 0x2BDC )]
	public class RedStocking : BaseContainer
	{
		[Constructable]
		public RedStocking()
			: base( Utility.RandomBool() ? 0x2BDB : 0x2BDC )
		{
			Weight = 1.0;
		}

		public RedStocking( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}