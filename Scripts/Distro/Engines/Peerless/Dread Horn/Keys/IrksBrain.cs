using System;
using Server;

namespace Server.Items
{
	public class IrksBrain : TransientItem
	{
		public override int LabelNumber { get { return 1074335; } } // irk's brain

		[Constructable]
		public IrksBrain()
			: base( 0x1CF0, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 804;
			LootType = LootType.Blessed;
		}

		public IrksBrain( Serial serial )
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