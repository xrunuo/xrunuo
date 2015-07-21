using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x27A0, 0x27EB )]
	public class Obi : BaseWaist
	{
		[Constructable]
		public Obi()
			: this( 0 )
		{
		}

		[Constructable]
		public Obi( int hue )
			: base( 0x27A0, hue )
		{
			Weight = 1.0;
		}

		public Obi( Serial serial )
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