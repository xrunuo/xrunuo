using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2799, 0x27E4 )]
	public class Kamishimo : BaseOuterTorso
	{
		[Constructable]
		public Kamishimo()
			: this( 0 )
		{
		}

		[Constructable]
		public Kamishimo( int hue )
			: base( 0x2799, hue )
		{
			Weight = 3.0;
		}

		public Kamishimo( Serial serial )
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