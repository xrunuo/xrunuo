using System;
using Server;

namespace Server.Items
{
	public class BottleOfPinkChampagne : Item
	{
		[Constructable]
		public BottleOfPinkChampagne()
			: base( 0x9C7 )
		{
			Name = "Bottle Of Pink Champagne";

			Hue = 0xEC;

			Weight = 1.0;
		}

		public BottleOfPinkChampagne( Serial serial )
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