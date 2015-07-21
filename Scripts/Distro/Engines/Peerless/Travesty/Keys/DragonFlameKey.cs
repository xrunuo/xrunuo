using System;
using Server;

namespace Server.Items
{
	public class DragonFlameKey : TransientItem
	{
		public override int LabelNumber { get { return 1074343; } } // dragon flame key

		[Constructable]
		public DragonFlameKey()
			: base( 0x100F, TimeSpan.FromSeconds( 21600.0 ) )
		{
			Weight = 2.0;
			LootType = LootType.Blessed;
			Hue = 42;
		}

		public DragonFlameKey( Serial serial )
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