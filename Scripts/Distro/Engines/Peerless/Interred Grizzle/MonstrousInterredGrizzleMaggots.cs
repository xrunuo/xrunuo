using System;
using Server;

namespace Server.Items
{
	public class MonstrousInterredGrizzleMaggots : Item
	{
		public override int LabelNumber { get { return 1075090; } } // Monstrous Interred Grizzle Maggots

		[Constructable]
		public MonstrousInterredGrizzleMaggots()
			: base( 0x2633 )
		{
			Weight = 1.0;
		}

		public MonstrousInterredGrizzleMaggots( Serial serial )
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