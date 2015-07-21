using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ArcaneEmpowermentScroll : SpellScroll
	{
		[Constructable]
		public ArcaneEmpowermentScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public ArcaneEmpowermentScroll( int amount )
			: base( 615, 0x2D60, amount )
		{
			Hue = 2301;
		}

		public ArcaneEmpowermentScroll( Serial serial )
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