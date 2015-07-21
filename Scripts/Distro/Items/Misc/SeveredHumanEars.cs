using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x312F, 0x3130 )]
	public class SeveredHumanEars : Item
	{
		[Constructable]
		public SeveredHumanEars()
			: base( Utility.RandomBool() ? 0x312F : 0x3130 )
		{
			Weight = 1;
			Stackable = true;
		}

		public SeveredHumanEars( Serial serial )
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