using System;
using Server;

namespace Server.Items
{
	public class FlouredBreadBoard : Item
	{
		public override int LabelNumber { get { return 1113639; } } // floured bread board

		[Constructable]
		public FlouredBreadBoard()
			: base( 0x14E9 )
		{
			Weight = 3.0;
		}

		public FlouredBreadBoard( Serial serial )
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