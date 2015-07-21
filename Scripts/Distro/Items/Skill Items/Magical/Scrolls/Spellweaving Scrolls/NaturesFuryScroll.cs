using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class NaturesFuryScroll : SpellScroll
	{
		[Constructable]
		public NaturesFuryScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public NaturesFuryScroll( int amount )
			: base( 605, 0x2D56, amount )
		{
			Hue = 2301;
		}

		public NaturesFuryScroll( Serial serial )
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