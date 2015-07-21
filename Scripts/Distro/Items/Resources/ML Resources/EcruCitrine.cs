using System;

namespace Server.Items
{
	public class EcruCitrine : Item, ICommodity
	{
		public override int LabelNumber { get { return 1026252; } } // Ecru Citrine

		[Constructable]
		public EcruCitrine()
			: this( 1 )
		{
		}

		[Constructable]
		public EcruCitrine( int amount )
			: base( 0x3195 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public EcruCitrine( Serial serial )
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
