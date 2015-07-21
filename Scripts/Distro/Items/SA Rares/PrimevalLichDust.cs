using System;
using Server;

namespace Server.Items
{
	public class PrimevalLichDust : Item
	{
		public override int LabelNumber { get { return 1031701; } } // Primeval Lich Dust

		[Constructable]
		public PrimevalLichDust()
			: base( 0x2DB5 )
		{
			Weight = 1.0;
			Stackable = false;
		}

		public PrimevalLichDust( Serial serial )
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