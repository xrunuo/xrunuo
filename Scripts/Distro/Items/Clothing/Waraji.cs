using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2796, 0x27E1 )]
	public class Waraji : BaseShoes
	{
		[Constructable]
		public Waraji()
			: this( 0 )
		{
		}

		[Constructable]
		public Waraji( int hue )
			: base( 0x2796, hue )
		{
			Weight = 2.0;
		}

		public Waraji( Serial serial )
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