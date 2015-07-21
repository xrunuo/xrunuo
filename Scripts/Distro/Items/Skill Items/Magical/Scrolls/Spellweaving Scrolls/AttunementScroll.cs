using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class AttunementScroll : SpellScroll
	{
		[Constructable]
		public AttunementScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public AttunementScroll( int amount )
			: base( 603, 0x2D54, amount )
		{
			Hue = 2301;
		}

		public AttunementScroll( Serial serial )
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