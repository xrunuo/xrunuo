using System;
using Server;

namespace Server.Items
{
	public class ToxicVenomSac : Item, ICommodity
	{
		public override int LabelNumber { get { return 1112291; } } // toxic venom sac

		[Constructable]
		public ToxicVenomSac()
			: this( 1 )
		{
		}

		[Constructable]
		public ToxicVenomSac( int amount )
			: base( 0x4005 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 0x170;
		}

		public ToxicVenomSac( Serial serial )
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