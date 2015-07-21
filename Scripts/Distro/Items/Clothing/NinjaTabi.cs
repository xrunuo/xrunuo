using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2797, 0x27E2 )]
	public class NinjaTabi : BaseShoes
	{
		[Constructable]
		public NinjaTabi()
			: this( 0 )
		{
		}

		[Constructable]
		public NinjaTabi( int hue )
			: base( 0x2797, hue )
		{
			Weight = 2.0;
		}

		public NinjaTabi( Serial serial )
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