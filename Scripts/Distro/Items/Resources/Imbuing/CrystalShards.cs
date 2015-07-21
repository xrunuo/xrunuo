using System;
using Server;

namespace Server.Items
{
	public class CrystalShards : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113347; } } // crystal shards

		[Constructable]
		public CrystalShards()
			: this( 1 )
		{
		}

		[Constructable]
		public CrystalShards( int amount )
			: base( 0x5738 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public CrystalShards( Serial serial )
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
