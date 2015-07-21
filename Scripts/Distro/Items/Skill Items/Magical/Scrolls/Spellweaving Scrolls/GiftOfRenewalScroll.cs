using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GiftOfRenewalScroll : SpellScroll
	{
		[Constructable]
		public GiftOfRenewalScroll()
			: this( 1 )
		{
		}

		[Constructable]
		public GiftOfRenewalScroll( int amount )
			: base( 601, 0x2D52, amount )
		{
			Hue = 2301;
		}

		public GiftOfRenewalScroll( Serial serial )
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