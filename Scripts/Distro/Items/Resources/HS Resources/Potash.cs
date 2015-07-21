using System;
using Server;

namespace Server.Items
{
	public class Potash : Item
	{
		public override int LabelNumber { get { return 1116319; } } // potash

		[Constructable]
		public Potash()
			: base( 0x423A )
		{
			Weight = 0.1;
			Stackable = false;
		}

		public Potash( Serial serial )
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
