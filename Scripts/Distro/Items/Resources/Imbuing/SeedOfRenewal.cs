using System;
using Server;

namespace Server.Items
{
	public class SeedOfRenewal : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113345; } } // seed of renewal

		[Constructable]
		public SeedOfRenewal()
			: this( 1 )
		{
		}

		[Constructable]
		public SeedOfRenewal( int amount )
			: base( 0x5736 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public SeedOfRenewal( Serial serial )
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
