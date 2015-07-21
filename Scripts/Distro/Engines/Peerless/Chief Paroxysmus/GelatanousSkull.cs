using System;
using Server;

namespace Server.Items
{
	public class GelatanousSkull : TransientItem
	{
		public override int LabelNumber { get { return 1074328; } } // gelatanous skull

		[Constructable]
		public GelatanousSkull()
			: base( 0x1AE0, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public GelatanousSkull( Serial serial )
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