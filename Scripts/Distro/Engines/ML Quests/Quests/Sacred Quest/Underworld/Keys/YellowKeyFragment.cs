using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public class YellowKeyFragment : BaseKeyFragment
	{
		public override int LabelNumber { get { return 1111648; } } // Yellow Key Fragment

		public YellowKeyFragment()
			: base( 0x36 )
		{
		}

		public YellowKeyFragment( Serial serial )
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