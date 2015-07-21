using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SummonFiendScroll : SpellScroll
	{
		[Constructable]
		public SummonFiendScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public SummonFiendScroll( int amount )
			: base( 607, 0x2D58, amount )
		{
			Hue = 2301;
		}

		public SummonFiendScroll( Serial serial )
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