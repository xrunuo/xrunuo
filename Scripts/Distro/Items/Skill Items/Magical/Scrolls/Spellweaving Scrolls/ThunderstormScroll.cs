using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ThunderstormScroll : SpellScroll
	{
		[Constructable]
		public ThunderstormScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public ThunderstormScroll( int amount )
			: base( 604, 0x2D55, amount )
		{
			Hue = 2301;
		}

		public ThunderstormScroll( Serial serial )
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