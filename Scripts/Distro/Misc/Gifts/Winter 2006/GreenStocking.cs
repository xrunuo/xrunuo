using System;

namespace Server.Items
{
	[Flipable( 0x2BD9, 0x2BDA )]
	public class GreenStocking : BaseContainer
	{
		[Constructable]
		public GreenStocking()
			: base( Utility.RandomBool() ? 0x2BD9 : 0x2BDA )
		{
			Weight = 1.0;
		}

		public GreenStocking( Serial serial )
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