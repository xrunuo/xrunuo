using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GiftOfLifeScroll : SpellScroll
	{
		[Constructable]
		public GiftOfLifeScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public GiftOfLifeScroll( int amount )
			: base( 614, 0x2D5F, amount )
		{
			Hue = 2301;
		}

		public GiftOfLifeScroll( Serial serial )
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