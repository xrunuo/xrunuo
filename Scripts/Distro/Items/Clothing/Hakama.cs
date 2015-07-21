using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x279A, 0x27E5 )]
	public class Hakama : BaseOuterLegs
	{
		[Constructable]
		public Hakama()
			: this( 0 )
		{
		}

		[Constructable]
		public Hakama( int hue )
			: base( 0x279A, hue )
		{
			Weight = 2.0;
		}

		public Hakama( Serial serial )
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