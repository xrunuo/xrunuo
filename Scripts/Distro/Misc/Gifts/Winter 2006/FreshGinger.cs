using System;

namespace Server.Items
{
	public class FreshGinger : Item
	{
		public override int LabelNumber { get { return 1031235; } } // Fresh Ginger
		
		[Constructable]
		public FreshGinger()
			: base( 0x2BE3 )
		{
			Weight = 1.0;
		}

		public FreshGinger( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}