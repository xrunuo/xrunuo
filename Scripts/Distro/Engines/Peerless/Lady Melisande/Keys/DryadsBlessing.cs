using System;
using Server;

namespace Server.Items
{
	public class DryadsBlessing : TransientItem
	{
		public override int LabelNumber { get { return 1020540; } } // dryad's blessing

		[Constructable]
		public DryadsBlessing()
			: base( 0x021C, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 89;
			LootType = LootType.Blessed;
		}

		public DryadsBlessing( Serial serial )
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