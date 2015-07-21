using System;
using Server;

namespace Server.Items
{
	public class RoyalCirclet : Circlet
	{
		public override int LabelNumber { get { return 1031119; } } // Royal Circlet 

		[Constructable]
		public RoyalCirclet()
		{
			ItemID = 11119;
		}

		public RoyalCirclet( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}