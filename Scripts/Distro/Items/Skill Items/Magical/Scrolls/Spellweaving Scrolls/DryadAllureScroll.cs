using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DryadAllureScroll : SpellScroll
	{
		[Constructable]
		public DryadAllureScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public DryadAllureScroll( int amount )
			: base( 611, 0x2D5C, amount )
		{
			Hue = 2301;
		}

		public DryadAllureScroll( Serial serial )
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