using System;
using Server;

namespace Server.Items
{
	public class Plinth : Item
	{
		public override int LabelNumber { get { return 1024643; } } // pedestal

		[Constructable]
		public Plinth()
			: base( 0x32F2 )
		{
			Weight = 5.0;
		}

		public Plinth( Serial serial )
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