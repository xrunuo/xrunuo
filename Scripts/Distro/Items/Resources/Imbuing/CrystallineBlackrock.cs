using System;
using Server;

namespace Server.Items
{
	public class CrystallineBlackrock : Item, ICommodity
	{
		public override int LabelNumber { get { return 1113344; } } // crystalline blackrock

		[Constructable]
		public CrystallineBlackrock()
			: this( 1 )
		{
		}

		[Constructable]
		public CrystallineBlackrock( int amount )
			: base( 0x5732 )
		{
			Weight = 0.1;
			Stackable = true;
			Amount = amount;
		}

		public CrystallineBlackrock( Serial serial )
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
