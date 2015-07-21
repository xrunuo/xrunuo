using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EtherealVoyageScroll : SpellScroll
	{
		[Constructable]
		public EtherealVoyageScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public EtherealVoyageScroll( int amount )
			: base( 612, 0x2D5D, amount )
		{
			Hue = 2301;
		}

		public EtherealVoyageScroll( Serial serial )
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