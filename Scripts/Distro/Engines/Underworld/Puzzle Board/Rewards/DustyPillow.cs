using System;
using Server;

namespace Server.Items
{
	public class DustyPillow : Item
	{
		public override int LabelNumber { get { return 1113638; } } // dusty pillow

		[Constructable]
		public DustyPillow()
			: base( Utility.RandomList( 0x163A, 0x163B ) )
		{
			Weight = 1.0;
		}

		public DustyPillow( Serial serial )
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