using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public class RedKeyFragment : BaseKeyFragment
	{
		public override int LabelNumber { get { return 1111647; } } // Red Key Fragment

		public RedKeyFragment()
			: base( 0x22 )
		{
		}

		public RedKeyFragment( Serial serial )
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