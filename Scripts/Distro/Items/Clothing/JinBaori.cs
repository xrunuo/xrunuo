using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x27A1, 0x27EC )]
	public class JinBaori : BaseMiddleTorso
	{
		[Constructable]
		public JinBaori()
			: this( 0 )
		{
		}

		[Constructable]
		public JinBaori( int hue )
			: base( 0x27A1, hue )
		{
			Weight = 3.0;
		}

		public JinBaori( Serial serial )
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