using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1C20, 0x1C21 )]
	public class MummifiedCorpse : Item
	{
		public override int LabelNumber { get { return 1112400; } } // a mummified corpse

		[Constructable]
		public MummifiedCorpse()
			: base( 0x1C20 )
		{
			Weight = 5.0;
		}

		public MummifiedCorpse( Serial serial )
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
