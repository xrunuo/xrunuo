using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EssenceOfWindScroll : SpellScroll
	{
		[Constructable]
		public EssenceOfWindScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public EssenceOfWindScroll( int amount )
			: base( 610, 0x2D5B, amount )
		{
			Hue = 2301;
		}

		public EssenceOfWindScroll( Serial serial )
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