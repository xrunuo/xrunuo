using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public class BlueKeyFragmentSpawner : KeyFragmentSpawner
	{
		public override int LabelNumber { get { return 1111646; } } // Blue Key Fragment

		public override Type KeyType { get { return typeof( BlueKeyFragment ); } }

		public BlueKeyFragmentSpawner()
			: base( 0x5F )
		{
		}

		public BlueKeyFragmentSpawner( Serial serial )
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