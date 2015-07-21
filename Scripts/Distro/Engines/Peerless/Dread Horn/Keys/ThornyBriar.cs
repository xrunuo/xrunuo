using System;
using Server;

namespace Server.Items
{
	public class ThornyBriar : TransientItem
	{
		public override int LabelNumber { get { return 1074334; } } // thorny briar

		[Constructable]
		public ThornyBriar()
			: base( 0x3020, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 1.0;
			Hue = 0x214;
			LootType = LootType.Blessed;
		}

		public ThornyBriar( Serial serial )
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