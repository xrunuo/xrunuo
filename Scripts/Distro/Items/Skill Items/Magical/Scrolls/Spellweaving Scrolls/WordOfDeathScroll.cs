using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WordOfDeathScroll : SpellScroll
	{
		[Constructable]
		public WordOfDeathScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public WordOfDeathScroll( int amount )
			: base( 613, 0x2D5E, amount )
		{
			Hue = 2301;
		}

		public WordOfDeathScroll( Serial serial )
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