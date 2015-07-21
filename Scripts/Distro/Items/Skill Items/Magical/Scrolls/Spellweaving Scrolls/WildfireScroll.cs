using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WildfireScroll : SpellScroll
	{
		[Constructable]
		public WildfireScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public WildfireScroll( int amount )
			: base( 609, 0x2D5A, amount )
		{
			Hue = 2301;
		}

		public WildfireScroll( Serial serial )
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