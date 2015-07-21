using System;
using Server;

namespace Server.Items
{
	public class PartiallyDigestedTorso : TransientItem
	{
		public override int LabelNumber { get { return 1074326; } } // partially digested torso

		[Constructable]
		public PartiallyDigestedTorso()
			: base( 0x1D9F, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public PartiallyDigestedTorso( Serial serial )
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