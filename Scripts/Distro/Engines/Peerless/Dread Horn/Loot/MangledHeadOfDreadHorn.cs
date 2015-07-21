using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class MangledHeadOfDreadHorn : Item
	{
		public override int LabelNumber { get { return 1032631; } } // mangled head of dread horn

		[Constructable]
		public MangledHeadOfDreadHorn()
			: base( 0x3156 )
		{
			Weight = 20.0;
		}

		public MangledHeadOfDreadHorn( Serial serial )
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