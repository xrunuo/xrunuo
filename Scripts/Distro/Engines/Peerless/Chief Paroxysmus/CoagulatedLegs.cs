using System;
using Server;

namespace Server.Items
{
	public class CoagulatedLegs : TransientItem
	{
		public override int LabelNumber { get { return 1074327; } } // coagulated legs

		[Constructable]
		public CoagulatedLegs()
			: base( 0x1CDF, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public CoagulatedLegs( Serial serial )
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