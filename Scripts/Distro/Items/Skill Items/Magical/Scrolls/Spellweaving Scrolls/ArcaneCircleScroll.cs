using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ArcaneCircleScroll : SpellScroll
	{
		[Constructable]
		public ArcaneCircleScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public ArcaneCircleScroll( int amount )
			: base( 600, 0x2D51, amount )
		{
			Hue = 2301;
		}

		public ArcaneCircleScroll( Serial serial )
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